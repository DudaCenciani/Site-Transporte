using AgendamentoTransporte.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.DataProtection;

var builder = WebApplication.CreateBuilder(args);

// ✅ Configuração do banco de dados
builder.Services.AddDbContext<DataProtectionKeyContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// ✅ Configuração de proteção de dados
builder.Services.AddDataProtection()
    .PersistKeysToDbContext<DataProtectionKeyContext>();

// ✅ Adiciona serviços MVC
builder.Services.AddControllersWithViews();

// ✅ Adiciona serviço de autorização
builder.Services.AddAuthorization();

var app = builder.Build();

var port = Environment.GetEnvironmentVariable("PORT") ?? "5000";
app.Urls.Add($"http://*:{port}");

// ✅ Configuração do pipeline HTTP
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
