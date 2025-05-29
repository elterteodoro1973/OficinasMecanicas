using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebServicoAPI.Models;

public partial class ResetarSenha
{
    [Key]
    public Guid Id { get; set; }

    [StringLength(150)]
    [Unicode(false)]
    public string? Token { get; set; }

    public Guid UsuarioId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime DataSolicitacao { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime DataExpiracao { get; set; }

    public bool? Efetivado { get; set; }

    public bool? Excluido { get; set; }

    [ForeignKey("UsuarioId")]
    [InverseProperty("ResetarSenha")]
    public virtual Usuarios Usuario { get; set; } = null!;
}
