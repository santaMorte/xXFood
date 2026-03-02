using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using xFood.Domain.Entities;
using xFood.Infrastructure.Persistence;


namespace xFood.Web.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")] // /api/products
    [Produces("application/json")]
    public class ProductsApiController : ControllerBase
    {
        private readonly xFoodDbContext _ctx;
        public ProductsApiController(xFoodDbContext ctx) => _ctx = ctx;

        // GET: /api/products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetAll()
        {
            var items = await _ctx.Products
                .AsNoTracking()
                .Include(p => p.Category)
                .ToListAsync();

            return Ok(items);
        }

        // GET: /api/products/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Product>> GetById(int id)
        {
            var entity = await _ctx.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (entity is null) return NotFound();
            return Ok(entity);
        }

        // POST: /api/products
        [HttpPost]
        public async Task<ActionResult<Product>> Create([FromBody] Product model)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            // checa CategoryId válido (didático)
            var catExists = await _ctx.Categories.AnyAsync(c => c.Id == model.CategoryId);
            if (!catExists) return BadRequest("Categoria inválida.");

            _ctx.Products.Add(model);
            await _ctx.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = model.Id }, model);
        }

        // PUT: /api/products/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] Product model)
        {
            if (id != model.Id) return BadRequest("Id do recurso difere do corpo da requisição.");
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            // checa CategoryId válido (didático)
            var catExists = await _ctx.Categories.AnyAsync(c => c.Id == model.CategoryId);
            if (!catExists) return BadRequest("Categoria inválida.");

            // Atualização direta
            _ctx.Entry(model).State = EntityState.Modified;

            try
            {
                await _ctx.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                var exists = await _ctx.Products.AnyAsync(p => p.Id == id);
                if (!exists) return NotFound();
                throw;
            }

            return NoContent();
        }

        // DELETE: /api/products/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _ctx.Products.FindAsync(id);
            if (entity is null) return NotFound();

            _ctx.Products.Remove(entity);
            await _ctx.SaveChangesAsync();
            return NoContent();
        }
    }
}
