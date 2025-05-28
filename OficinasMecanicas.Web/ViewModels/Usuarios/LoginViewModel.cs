using System.ComponentModel.DataAnnotations;

namespace OficinasMecanicas.Web.ViewModels.Usuarios
{
    public class LoginViewModel
    {
        [Display(Name = "Email")]
        [Required(ErrorMessage = "{0} é obrigatório !")]
        public string Email { get; set; } = null!;


        [Display(Name = "Senha")]
        [Required(ErrorMessage = "{0} é obrigatório !")]
        [DataType(DataType.Password)]
        public string Senha { get; set; } = null!;
    }
}
