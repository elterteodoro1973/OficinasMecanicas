using System.ComponentModel.DataAnnotations;

namespace OficinasMecanicas.Web.ViewModels.Agenda
{
    public class CadastrarEditarAgendamentoVisitaViewModel
    {
        public Guid? Id { get; set; }

        [Display(Name = "Nome")]      
        [StringLength(256, ErrorMessage = "{0} possui tamanho máximo de 256 caracteres !")]
        public string Nome { get; set; } = null!;

        [Required(ErrorMessage = "O Cliente é obrigatório !")]        
        public Guid IdUsuario { get; set; }

        [Display(Name = "Cliente")]
        public string NomeCliente { get; set; } = null!;

        [Required(ErrorMessage = "O nome da Oficina é obrigatório !")]
        public Guid IdOficina { get; set; }

        [Display(Name = "Oficina")]
        public string NomeOficina { get; set; } = null!;

        [Display(Name = "Data/Hora")]
        public DateTime DataHora { get; set; }

        [Display(Name = "Objetivo da visita")]
        [Required(ErrorMessage = "O Objetivo é obrigatório !")]
        public string Descricao { get; set; } = null!;

    }
}
