using System.ComponentModel.DataAnnotations;

namespace OficinasMecanicas.Dominio.Entidades
{
    public class EntidadeBase
    {
        [Key]
        public Guid Id { get; set; }
    }
}
