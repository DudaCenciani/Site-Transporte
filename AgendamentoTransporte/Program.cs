using AgendamentoTransporte.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 🔽 Aqui registramos o DbContext com a connection string do appsettings.json
builder.Services.AddDbContext<AppDbContext>(options =>
   options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"))) ;


builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles(); // Corrige MapStaticAssets para .NET 9

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}");

app.Run();
