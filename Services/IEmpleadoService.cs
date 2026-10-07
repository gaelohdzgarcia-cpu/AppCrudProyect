using AppCrud.Models;
using AppCrud.ViewModels;

namespace AppCrud.Services
{
    //obtener, crear, editar y eliminar empleados
    public interface IEmpleadoService
    {
        Task<List<Empleado>> ObtenerTodos();

        Task<EmpleadoVM?> ObtenerPorId(int id);

        Task Crear(Empleado empleado);

        Task<bool> Editar(int id, Empleado empleado);

        Task<bool> Eliminar(int id);
    }
}