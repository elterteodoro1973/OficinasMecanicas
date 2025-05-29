using System.ComponentModel.DataAnnotations;

namespace OficinasMecanicas.Web.ViewModels.Agenda
{
    public class CadastrarEditarAgendamentoVisitaViewModel
    {
        public Guid? Id { get; set; }

        [Display(Name = "Nome")]
        [Required(ErrorMessage = "{0} é obrigatório !")]
        [StringLength(256, ErrorMessage = "{0} possui tamanho máximo de 256 caracteres !")]
        public string Nome { get; set; } = null!;

        [Required(ErrorMessage = "O Cliente é obrigatório !")]        
        public Guid IdUsuario { get; set; }

        [Display(Name = "Cliente")]
        public string NomeCliente { get; set; } = null!;
        
        public Guid IdOficina { get; set; }

        [Display(Name = "Oficina")]
        public string NomeOficina { get; set; } = null!;

        [Display(Name = "Data/Hora")]
        public string DataHora { get; set; } = null!;

        [Display(Name = "Objetivo da visita")]
        public string Descricao { get; set; } = null!;

    }
}
