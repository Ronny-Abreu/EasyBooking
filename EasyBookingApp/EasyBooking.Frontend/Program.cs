using EasyBooking.Frontend.Services;
using EasyBooking.Frontend.Models;

var builder = WebApplication.CreateBuilder(args);

// Configuración de servicios ANTES de builder.Build()
builder.Services.AddHttpClient(); // HttpClient
builder.Services.AddScoped<HttpClientService>(); // Servicios personalizados
builder.Services.AddScoped<UsuarioClientService>();
builder.Services.Configure<ApiSettings>(builder.Configuration.GetSection("ApiSettings")); // Configuración

// Otros servicios
builder.Services.AddControllersWithViews();

var app = builder.Build(); // A partir de aquí, NO se pueden registrar más servicios

// Configuración del middleware (esto sí va después de Build())
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}







app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();