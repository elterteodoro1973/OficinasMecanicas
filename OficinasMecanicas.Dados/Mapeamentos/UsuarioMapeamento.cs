using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OficinasMecanicas.Dominio.Entidades;

namespace OficinasMecanicas.Dados.Mapeamentos
{
    public class UsuarioMapeamento : IEntityTypeConfiguration<Usuarios>
    {
        public void Configure(EntityTypeBuilder<Usuarios> builder)
        {
            builder.ToTable("Usuarios");
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id).HasDefaultValueSql("(newid())");
            builder.Property(c => c.Username).HasMaxLength(256);            
            builder.Property(c => c.Email).HasMaxLength(512);            
            builder.Property(c => c.PasswordHash);
        }
    }
}
