using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using OficinasMecanicas.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OficinasMecanicas.Dados.Mapeamentos
{
   

    public class ServicosPrestadosMapeamento : IEntityTypeConfiguration<ServicosPrestados>
    {
        public void Configure(EntityTypeBuilder<ServicosPrestados> builder)
        {
            builder.ToTable("ServicosPrestados");
            builder.HasKey(e => e.Id).HasName("PK_Usuario");
            builder.Property(c => c.Nome).HasMaxLength(256);
        }
    }
}
