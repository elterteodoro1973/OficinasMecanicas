using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OficinasMecanicas.Dominio.Entidades;

namespace OficinasMecanicas.Dados.Mapeamentos
{
    
    public class AgendamentoVisitaMapeamento : IEntityTypeConfiguration<AgendamentoVisita>
    {
        public void Configure(EntityTypeBuilder<AgendamentoVisita> builder)
        {
            builder.ToTable("AgendamentoVisita");
            builder.HasKey(e => e.Id).HasName("PK_AgendamentosVisita");
            builder.Property(c => c.IdUsuario);
            builder.Property(c => c.DataHora);
            builder.Property(c => c.Descricao);
            //builder.Property(e => e.Id).ValueGeneratedNever();

            builder.HasOne(d => d.IdOficinaNavigation).
                WithMany(p => p.AgendamentoVisita).
                HasConstraintName("FK_AgendamentoVisita_OficinaMecanica");

            builder.HasOne(d => d.IdUsuarioNavigation).
                WithMany(p => p.AgendamentoVisita).
                HasConstraintName("FK_AgendamentoVisita_Usuarios");

        }
    }
}
