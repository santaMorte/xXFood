using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using xFood.Application.DTOs;
using xFood.Application.Interfaces;

namespace xFood.Web.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductRepository _products;
        private readonly ICategoryRepository _categories;
        private readonly IWebHostEnvironment _env;

        public ProductsController(IProductRepository products, ICategoryRepository categories, IWebHostEnvironment env)
            => (_products, _categories, _env) = (products, categories, env);

        public async Task<IActionResult> Index(int? categoryId, string? q, int page = 1, int size = 50)
        {
            var cats = await _categories.GetAllAsync();
            ViewBag.Categories = new SelectList(cats, "Id", "Name", categoryId);

            var (total, items) = await _products.GetAllAsync(categoryId, q, page, size);
            return View(items);
        }

        public async Task<IActionResult> Create()
        {
            var cats = await _categories.GetAllAsync();
            ViewBag.Categories = new SelectList(cats, "Id", "Name");
            return View(new ProductCreateUpdateDto { Stock = 0, Price = 0m });
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductCreateUpdateDto model, IFormFile? imageFile)
        {
            if (!ModelState.IsValid)
            {
                var cats = await _categories.GetAllAsync();
                ViewBag.Categories = new SelectList(cats, "Id", "Name", model.CategoryId);
                return View(model);
            }

            if (imageFile is { Length: > 0 })
            {
                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(imageFile.FileName)}";
                var folder = Path.Combine(_env.WebRootPath, "uploads");
                Directory.CreateDirectory(folder);
                var path = Path.Combine(folder, fileName);
                using var stream = new FileStream(path, FileMode.Create);
                await imageFile.CopyToAsync(stream);
                model.ImageUrl = $"/uploads/{fileName}";
            }

            var id = await _products.CreateAsync(model);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var dto = await _products.GetByIdAsync(id);
            if (dto is null) return NotFound();

            var cats = await _categories.GetAllAsync();
            ViewBag.Categories = new SelectList(cats, "Id", "Name", dto.CategoryId);

            var vm = new ProductCreateUpdateDto
            {
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                Stock = dto.Stock,
                ImageUrl = dto.ImageUrl,
                CategoryId = dto.CategoryId
            };
            ViewBag.ProductId = dto.Id;
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, ProductCreateUpdateDto model, IFormFile? imageFile)
        {
            if (!ModelState.IsValid)
            {
                var cats = await _categories.GetAllAsync();
                ViewBag.Categories = new SelectList(cats, "Id", "Name", model.CategoryId);
                return View(model);
            }

            if (imageFile is { Length: > 0 })
            {
                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(imageFile.FileName)}";
                var folder = Path.Combine(_env.WebRootPath, "uploads");
                Directory.CreateDirectory(folder);
                var path = Path.Combine(folder, fileName);
                using var stream = new FileStream(path, FileMode.Create);
                await imageFile.CopyToAsync(stream);
                model.ImageUrl = $"/uploads/{fileName}";
            }

            await _products.UpdateAsync(id, model);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var dto = await _products.GetByIdAsync(id);
            if (dto is null) return NotFound();
            return View(dto);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> ConfirmDelete(int id)
        {
            await _products.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
