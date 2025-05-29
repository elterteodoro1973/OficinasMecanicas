using System.ComponentModel.DataAnnotations;

namespace OficinasMecanicas.Web.ViewModels.Usuarios
{
    public class OficinaViewModel
    {
        public Guid? Id { get; set; }        
        public string Nome { get; set; } = null!;        
        public string Endereco { get; set; } = null!;       
        public string Servicos { get; set; } = null!;
    }
}
