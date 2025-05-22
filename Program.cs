using AgendamentoTransporte.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.DataProtection;
using AgendamentoTransporte.Data;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DataProtectionKeyContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDataProtection()
    .PersistKeysToDbContext<DataProtectionKeyContext>();



var app = builder.Build();

var port = Environment.GetEnvironmentVariable("PORT") ?? "5000";
app.Urls.Add($"http://*:{port}");


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
