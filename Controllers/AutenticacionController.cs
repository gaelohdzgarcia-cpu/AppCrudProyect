using AppCrud.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AppCrud.Controllers
{
    public class AutenticacionController : Controller
    {
        private readonly IAutenticacionService _autenticacionService;

        public AutenticacionController(
            IAutenticacionService autenticacionService)
        {
            _autenticacionService = autenticacionService;
        }

        // Si ya tiene sesión, no debe volver a ver el Login
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login()
        {
            // Ya tiene una sesión autenticada
            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction("Lista", "Empleado");
            }

            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(string correo, string clave)
        {
            // Verificar 
            if (string.IsNullOrWhiteSpace(correo) ||
                string.IsNullOrWhiteSpace(clave))
            {
                ViewBag.Error = "Por favor, completa todos los campos.";
                return View();
            }

            // Validar
            var empleado = await _autenticacionService
                .ValidarUsuario(correo, clave);

            
            if (empleado == null)
            {
                ViewBag.Error = "Correo o contraseña incorrectos.";
                return View();
            }

            // Crear 
            var claims = new List<Claim>
            {
                new Claim(
                    ClaimTypes.NameIdentifier,
                    empleado.IdEmpleado.ToString()),

                new Claim(
                    ClaimTypes.Name,
                    empleado.NombreCompleto),

                new Claim(
                    ClaimTypes.Email,
                    empleado.Correo)
            };

            // identidad
            var identidad = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme);

            // usuario autenticado
            var principal = new ClaimsPrincipal(identidad);

            // cookie de autenticación
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal);

            return RedirectToAction("Lista", "Empleado");
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            // Eliminar la cookie
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);

            // Regresar al Login
            return RedirectToAction("Login", "Autenticacion");
        }
    }
}