using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

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

    [InverseProperty("IdOficinaNavigation")]
    public virtual ICollection<AgendamentoVisita> AgendamentoVisita { get; set; } = new List<AgendamentoVisita>();
}
