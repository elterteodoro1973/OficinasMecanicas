using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebServicoAPI.Models;

public partial class AgendamentoVisita
{
    [Key]
    public Guid Id { get; set; }

    public Guid? IdUsuario { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DataHora { get; set; }

    [StringLength(256)]
    [Unicode(false)]
    public string? Descricao { get; set; }

    public Guid? IdOficina { get; set; }

    [ForeignKey("IdOficina")]
    [InverseProperty("AgendamentoVisita")]
    public virtual OficinaMecanica? IdOficinaNavigation { get; set; }

    [ForeignKey("IdUsuario")]
    [InverseProperty("AgendamentoVisita")]
    public virtual Usuarios? IdUsuarioNavigation { get; set; }
}
