using OficinasMecanicas.Web.Configuracoes.Validacoes;
using System.ComponentModel.DataAnnotations;

namespace OficinasMecanicas.Web.ViewModels.Usuarios
{
    public class CadastrarEditarOficinaViewModel
    {
        public Guid? Id { get; set; }

        [Display(Name = "Nome")]
        [Required(ErrorMessage = "{0} é obrigatório !")]
        [StringLength(256, ErrorMessage = "{0} possui tamanho máximo de 256 caracteres !")]
        public string Nome { get; set; } = null!;

        [Display(Name = "Endereco")]
        [Required(ErrorMessage = "{0} é obrigatório !")]
        [StringLength(256, ErrorMessage = "{0} possui tamanho máximo de 256 caracteres !")]
        public string Endereco { get; set; } = null!;

        [Display(Name = "Servicos")]
        [Required(ErrorMessage = "{0} é obrigatório !")]
        [StringLength(512, ErrorMessage = "{0} possui tamanho máximo de 256 caracteres !")]
        public string Servicos { get; set; } = null!;
    }
}
