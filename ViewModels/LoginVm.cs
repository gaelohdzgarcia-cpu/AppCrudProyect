using System.ComponentModel.DataAnnotations;

namespace AppCrud.ViewModels
{
    public class LoginVm
    {
        [Required(ErrorMessage = "Crreo requerido")]
        public string Correo { get; set; } = string.Empty;

        //Clave permite que venga vacia
        [Required(ErrorMessage = "Contrasña requerido")]
        public string? Clave { get; set; } = string.Empty;
    }
}
