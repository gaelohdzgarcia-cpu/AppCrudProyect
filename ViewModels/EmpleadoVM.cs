using System.ComponentModel.DataAnnotations;

namespace AppCrud.ViewModels
{
    public class EmpleadoVM
    {
        public int IdEmpleado { get; set; }
        [Required(ErrorMessage = "Nombre requerido")]
        public string NombreCompleto { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        //Clave permite que venga vacia
        public string? Clave { get; set; } = string.Empty;
        public DateOnly FechaContrato { get; set; }
        public bool Activo { get; set; }
    }
}
