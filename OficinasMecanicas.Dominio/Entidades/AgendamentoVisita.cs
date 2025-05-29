#nullable enable
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OficinasMecanicas.Dominio.Entidades;

public partial class AgendamentoVisita
{
    [Key]
    public Guid Id { get; set; }

    public Guid IdUsuario { get; set; }

    public Guid IdOficina { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime DataHora { get; set; }

    [StringLength(256)]
    [Unicode(false)]
    public string? Descricao { get; set; }

    [ForeignKey("IdUsuario")]
    [InverseProperty("AgendamentoVisita")]
    public virtual Usuarios? IdUsuarioNavigation { get; set; }

}