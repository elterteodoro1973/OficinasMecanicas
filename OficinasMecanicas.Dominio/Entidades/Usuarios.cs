#nullable disable
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace OficinasMecanicas.Dominio.Entidades
{
    public partial class Usuarios : EntidadeBase
    {
        [StringLength(512)]
        public string Email { get; set; }

        public string PasswordHash { get; set; }

        [StringLength(256)]
        [Unicode(false)]
        public string Username { get; set; }


        //[InverseProperty("IdUsuarioNavigation")]
        //public virtual ICollection<ResetarSenha> ResetarSenha { get; set; } = new List<ResetarSenha>();

        [InverseProperty("IdUsuarioNavigation")]
        public virtual ICollection<AgendamentoVisita> AgendamentoVisita { get; set; } = new List<AgendamentoVisita>();

        [InverseProperty("Usuario")]
        public virtual ICollection<ResetarSenha> ResetarSenha { get; set; } = new List<ResetarSenha>();

    }
}