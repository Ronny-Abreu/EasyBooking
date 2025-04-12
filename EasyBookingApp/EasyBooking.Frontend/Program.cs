using EasyBooking.Frontend.Services;
using EasyBooking.Frontend.Models;

var builder = WebApplication.CreateBuilder(args);

// Configuraci�n de servicios ANTES de builder.Build()
builder.Services.AddHttpClient(); // HttpClient
builder.Services.AddScoped<HttpClientService>(); // Servicios personalizados
builder.Services.AddScoped<UsuarioClientService>();
builder.Services.Configure<ApiSettings>(builder.Configuration.GetSection("ApiSettings")); // Configuraci�n

// Otros servicios
builder.Services.AddControllersWithViews();

var app = builder.Build(); // A partir de aqu�, NO se pueden registrar m�s servicios

// Configuraci�n del middleware (esto s� va despu�s de Build())
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