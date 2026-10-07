using AppCrud.Data;
using AppCrud.Models;
using Microsoft.EntityFrameworkCore;

namespace AppCrud.Services
{
    public class AutenticacionService : IAutenticacionService
    {
        private readonly AppDBContext _appDBContext;
        private readonly IPasswordService _passwordService;

        public AutenticacionService(
            AppDBContext appDBContext,
            IPasswordService passwordService)
        {
            _appDBContext = appDBContext;
            _passwordService = passwordService;
        }

        public async Task<Empleado?> ValidarUsuario(string correo, string clave)
        {
            // Buscar al empleado por correo y verificar que esté activo.
            Empleado? empleado = await _appDBContext.Empleados
                .FirstOrDefaultAsync(e =>
                    e.Correo == correo &&
                    e.Activo);

            if (empleado == null)
            {
                return null;
            }

            // Comprobar la contraseña.
            bool claveCorrecta = _passwordService.VerifyPassword(
                clave,
                empleado.Clave);

            if (!claveCorrecta)
            {
                return null;
            }

            // convertirla a hash.
            if (empleado.Clave == clave)
            {
                empleado.Clave = _passwordService.HashPassword(clave);

                await _appDBContext.SaveChangesAsync();
            }

            return empleado;
        }
    }
}