using Microsoft.EntityFrameworkCore;
using EasyBooking.Application.Contracts;
using EasyBooking.Application.Services;
using EasyBooking.Persistence.Interfaces;
using EasyBooking.Persistence.Context;
using EasyBooking.Persistence.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

//Configuracion Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configurar la base de datos
builder.Services.AddDbContext<EasyBookingDbContext>(options =>
  options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


// Registrar Servicios
builder.Services.AddScoped<IUsuarioService, UsuarioService>();  // Servicio de usuario


// Registrar repositorios
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();

// Registrar el servicio de correo electrónico
builder.Services.AddScoped<IEmailService, EmailService>();





// Configurar CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();

app.Run();
