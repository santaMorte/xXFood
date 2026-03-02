using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using xFood.Application.Interfaces;
using xFood.Infrastructure.Persistence;
using xFood.Infrastructure.Repositories;
using xFood.Infrastructure.Seeding; // <-- para o SeedService

var builder = WebApplication.CreateBuilder(args);


#region 1) Configurações (antes de registrar serviços)
var conn = builder.Configuration.GetConnectionString("DefaultConnection")
           ?? "Server=(localdb)\\MSSQLLocalDB;Database=xFoodDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True";


#endregion

builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();

#region 2) Serviços (Dependency Injection)
builder.Services.AddDbContext<xFoodDbContext>(options =>
    options.UseNpgsql(conn));

builder.Services.AddControllersWithViews();

// ? Swagger + API Explorer
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ? CORS (liberado para estudos; depois pode restringir)
builder.Services.AddCors(opt =>
{
    opt.AddPolicy("AllowAll", p =>
        p.AllowAnyOrigin()
         .AllowAnyHeader()
         .AllowAnyMethod());
});
#endregion

var app = builder.Build();

#region 3) Tratamento de erros / segurança por ambiente
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
else
{
    // ? Swagger só em Dev
    app.UseSwagger();
    app.UseSwaggerUI();
}
#endregion

#region 4) Middlewares globais (ordem importa!)
app.UseHttpsRedirection();

app.UseRouting();

// ? Cultura pt-BR


var defaultCulture = new CultureInfo("pt-BR");
var localizationOptions = new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture(defaultCulture),
    SupportedCultures = new[] { defaultCulture },
    SupportedUICultures = new[] { defaultCulture }
};
app.UseRequestLocalization(localizationOptions);

// ? CORS
app.UseCors("AllowAll");

// (futuro) app.UseAuthentication();
app.UseAuthorization();
#endregion

#region 5) Endpoints (rotas)
app.MapControllers();      // API por atributo
app.MapStaticAssets();
app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}")
   .WithStaticAssets();
#endregion

// ? Seed/Migrate
await SeedService.EnsureSeedAsync(app.Services);

app.Run();
