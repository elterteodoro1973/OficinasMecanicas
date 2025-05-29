using System.ComponentModel.DataAnnotations;

namespace OficinasMecanicas.Web.ViewModels.Agenda
{
    public class AgendamentoVisitaViewModel
    {
        public Guid? Id { get; set; }
        public Guid IdUsuario { get; set; }
        public Guid IdOficina { get; set; }
        public string DataHora { get; set; }
        public string Descricao { get; set; } 
    }
}
