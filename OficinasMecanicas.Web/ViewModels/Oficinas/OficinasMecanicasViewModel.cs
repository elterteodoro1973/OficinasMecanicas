using System.ComponentModel.DataAnnotations;

namespace OficinasMecanicas.Web.ViewModels.Oficinas
{
    public class OficinasMecanicasViewModel
    {
        public Guid? Id { get; set; }        
        public string Nome { get; set; } = null!;        
        public string Endereco { get; set; } = null!;       
        public string Servicos { get; set; } = null!;
    }
}
