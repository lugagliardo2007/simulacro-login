using Microsoft.AspNetCore.Authentication.Cookies;
using SistemaLogin.Models;
using SistemaLogin.Services;

//inicializar y configurar entonrno de la aplicacion
var builder = WebApplication.CreateBuilder(args);

// Usuarios de prueba (reemplazan la carga manual del Program original)
var usuarios = new List<Usuario>
{
    new Usuario(1, "yo@gmail.com", "123456", true),
    new Usuario(2, "inactivo@gmail.com", "abcdef", false)
};
builder.Services.AddSingleton(new LoginService(usuarios));

builder.Services.AddControllers();

// Autenticación mediante cookies
builder.Services.AddAuthentication("galletitas")
    .AddCookie("galletitas", options =>
    {
        options.Cookie.Name = "usuarioAutenticado";
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
        options.Cookie.SameSite = SameSiteMode.Lax;

        options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
        options.SlidingExpiration = true;

        // Como es una API, si no hay cookie respondemos 401 en vez de redirigir
        options.Events.OnRedirectToLogin = context =>
        {
            context.Response.StatusCode = 401;
            return Task.CompletedTask;
        };
        options.Events.OnRedirectToAccessDenied = context =>
        {
            context.Response.StatusCode = 403;
            return Task.CompletedTask;
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

