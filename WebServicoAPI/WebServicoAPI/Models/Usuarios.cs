using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebServicoAPI.Models;

public partial class Usuarios
{
    [Key]
    public Guid Id { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? Username { get; set; }

    [StringLength(512)]
    [Unicode(false)]
    public string? PasswordHash { get; set; }

    [StringLength(256)]
    [Unicode(false)]
    public string? Email { get; set; }

    [InverseProperty("IdUsuarioNavigation")]
    public virtual ICollection<AgendamentoVisita> AgendamentoVisita { get; set; } = new List<AgendamentoVisita>();

    [InverseProperty("Usuario")]
    public virtual ICollection<ResetarSenha> ResetarSenha { get; set; } = new List<ResetarSenha>();
}
