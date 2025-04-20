using EasyBooking.Application.Contracts;
using EasyBooking.Application.Services;
using EasyBooking.Frontend.Models;
using EasyBooking.Frontend.Services;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddAutoMapper(
    typeof(EasyBooking.Application.Mappings.MappingProfile),
    typeof(EasyBooking.Frontend.Mappings.FrontendMappingProfile)
);

// Add HttpClient
builder.Services.AddHttpClient<HttpClientService>();
builder.Services.AddScoped<HttpClientService>();
builder.Services.AddScoped<UsuarioClientService>();

//Service Email
builder.Services.AddScoped<IEmailService, EmailService>();


// Configure ApiSettings
builder.Services.Configure<ApiSettings>(builder.Configuration.GetSection("ApiSettings"));

// Add Authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Usuarios/Login";
        options.LogoutPath = "/Usuarios/Logout";
        options.AccessDeniedPath = "/Home/AccessDenied";
        options.Cookie.HttpOnly = true;
        options.Cookie.SameSite = SameSiteMode.Strict;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();