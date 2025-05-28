using OficinasMecanicas.Web.Configuracoes.Validacoes;
using System.ComponentModel.DataAnnotations;

namespace OficinasMecanicas.Web.ViewModels.Usuarios
{
    public class CadastrarEditarUsuarioViewModel
    {
        public Guid? Id { get; set; }

        [Display(Name = "Nome")]
        [Required(ErrorMessage = "{0} é obrigatório !")]
        [StringLength(256, ErrorMessage = "{0} possui tamanho máximo de 256 caracteres !")]
        public string Nome { get; set; } = null!;

        [Display(Name = "Email")]
        [Required(ErrorMessage = "{0} é obrigatório !")]
        public string Email { get; set; } = null!;

        public string? Senha { get; set; } = null!;

        public string? ConfirmarSenha { get; set; } = null!;
    }
}
