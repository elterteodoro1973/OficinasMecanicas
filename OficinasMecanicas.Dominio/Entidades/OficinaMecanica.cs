using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace OficinasMecanicas.Dominio.Entidades;

public partial class OficinaMecanica
{
    [Key]
    public Guid Id { get; set; }

    [StringLength(256)]
    [Unicode(false)]
    public string? Nome { get; set; }

    [StringLength(256)]
    [Unicode(false)]
    public string? Endereco { get; set; }

    [Unicode(false)]
    public string? Servicos { get; set; }
}
