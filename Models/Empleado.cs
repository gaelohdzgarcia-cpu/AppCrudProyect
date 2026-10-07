using Microsoft.EntityFrameworkCore;

namespace AppCrud.Models
{
    public class Empleado
    {
        public int IdEmpleado { get; set; }
        public string NombreCompleto { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public string Clave { get; set; } = string.Empty;
        public DateOnly FechaContrato { get; set; }
        public bool Activo { get; set; }
    }
}
