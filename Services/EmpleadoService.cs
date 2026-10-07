using AppCrud.Data;
using AppCrud.Models;
using AppCrud.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace AppCrud.Services
{
    public class EmpleadoService : IEmpleadoService
    {
        private readonly AppDBContext _appDBContext;
        private readonly IPasswordService _passwordService;

        public EmpleadoService(
            AppDBContext appDBContext,
            IPasswordService passwordService)
        {
            _appDBContext = appDBContext;
            _passwordService = passwordService;
        }

        public async Task<List<Empleado>> ObtenerTodos()
        {
            return await _appDBContext.Empleados
                .ToListAsync();
        }

        public async Task<EmpleadoVM?> ObtenerPorId(int id)
        {
            // Capturar excepciones (errores que ocurren mientras
            // el programa está ejecutándose).
            try
            {
                return await _appDBContext.Empleados
                    .Where(e => e.IdEmpleado == id)
                    .Select(e => new EmpleadoVM()
                    {
                        IdEmpleado = e.IdEmpleado,
                        NombreCompleto = e.NombreCompleto,
                        Activo = e.Activo,
                        Correo = e.Correo,
                        FechaContrato = e.FechaContrato
                    })
                    .FirstOrDefaultAsync();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task Crear(Empleado empleado)
        {
            // Convertir la contraseña en un hash antes de guardarla.
            //Para que en la base de datos no aparezca la contraseña real.
            empleado.Clave = _passwordService.HashPassword(empleado.Clave);

            await _appDBContext.Empleados.AddAsync(empleado);

            await _appDBContext.SaveChangesAsync();
        }

        public async Task<bool> Editar(int id, Empleado empleado)
        {
            Empleado? empleadoBD = await _appDBContext.Empleados
                .FirstOrDefaultAsync(e => e.IdEmpleado == id);

            if (empleadoBD == null)
            {
                return false;
            }

            empleadoBD.NombreCompleto = empleado.NombreCompleto;
            empleadoBD.Correo = empleado.Correo;
            empleadoBD.FechaContrato = empleado.FechaContrato;
            empleadoBD.Activo = empleado.Activo;

            // Solo cambiar la contraseña si el usuario escribió una nueva.
            if (!string.IsNullOrWhiteSpace(empleado.Clave))
            {
                empleadoBD.Clave =
                    _passwordService.HashPassword(empleado.Clave);
            }

            await _appDBContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Eliminar(int id)
        {
            Empleado? empleado = await _appDBContext.Empleados
                .FirstOrDefaultAsync(e => e.IdEmpleado == id);

            if (empleado == null)
            {
                return false;
            }

            _appDBContext.Empleados.Remove(empleado);

            await _appDBContext.SaveChangesAsync();

            return true;
        }
    }
}