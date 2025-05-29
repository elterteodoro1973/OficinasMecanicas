#nullable disable
using System.ComponentModel.DataAnnotations.Schema;

namespace OficinasMecanicas.Dominio.Entidades;

public partial class ResetarSenha : EntidadeBase
{   
    public String Token { get; set; }

    public Guid UsuarioId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DataSolicitacao { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime DataExpiracao { get; set; }

    public bool? Efetivado { get; set; }

    public bool? Excluido { get; set; }

    [ForeignKey("UsuarioId")]
    [InverseProperty("ResetarSenha")]
    public virtual Usuarios IdUsuarioNavigation { get; set; }
}