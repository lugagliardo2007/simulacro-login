using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaLogin.Models;
using SistemaLogin.Services;
using System.Security.Claims;

namespace SistemaLogin.Controllers
{
    public class LoginRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly LoginService _loginService;

        public AuthController(LoginService loginService)
        {
            _loginService = loginService;
        }

        // 1) INICIAR SESIÓN: si es correcto, crea la cookie con el ID del usuario
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            ResultadoLogin resultado = _loginService.IniciarSesion(request.Email, request.Password);

            if (!resultado.Exitoso)
                return Unauthorized(new { mensaje = resultado.Mensaje });

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, resultado.UsuarioId.ToString()!)
            };

            var identity = new ClaimsIdentity(claims, "galletitas");
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync("galletitas", principal);

            return Ok(new { mensaje = resultado.Mensaje });
        }

        // 2) RUTA PROTEGIDA: solo entra quien tenga la cookie
        [HttpGet("protegida")]
        [Authorize]
        public IActionResult Protegida()
        {
            string? id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return Ok(new { mensaje = $"Acceso permitido. Usuario ID: {id}" });
        }

        // 3) CERRAR SESIÓN: elimina la cookie
        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("galletitas");
            return Ok(new { mensaje = "Sesión cerrada correctamente." });
        }
    }
}