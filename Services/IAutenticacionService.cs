using AppCrud.Models;

namespace AppCrud.Services
{
    public interface IAutenticacionService
    {
        //autenticación tendrá un método llamado ValidarUsuario
        Task<Empleado?> ValidarUsuario(string correo, string clave);
    }
}