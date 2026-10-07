using AutoMapper;
using AppCrud.Data;
using AppCrud.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// MVC
builder.Services.AddControllersWithViews();

// Autenticación mediante cookies
builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Autenticacion/Login";
        options.AccessDeniedPath = "/Autenticacion/Login";
    });

// Autorización
builder.Services.AddAuthorization();

// Base de datos
builder.Services.AddDbContext<AppDBContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("CadenaSQL"));
});

// AutoMapper
builder.Services.AddAutoMapper(typeof(Program));

// Servicios
builder.Services.AddScoped<IEmpleadoService, EmpleadoService>();
builder.Services.AddScoped<IAutenticacionService, AutenticacionService>();
builder.Services.AddScoped<IPasswordService, PasswordService>();

var app = builder.Build();

// Configuración del pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Autenticación y autorización
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Autenticacion}/{action=Login}/{id?}");

app.Run();