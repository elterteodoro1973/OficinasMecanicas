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
    //internal class OficinaMecanicaMapeamento
    //{
    //}

    public class OficinaMecanicaMapeamento : IEntityTypeConfiguration<OficinaMecanica>
    {
        public void Configure(EntityTypeBuilder<OficinaMecanica> builder)
        {
            builder.ToTable("OficinaMecanica");
            builder.HasKey(e => e.Id).HasName("PK_OficinaMecanica");
            builder.Property(c => c.Nome).HasMaxLength(256);
            builder.Property(c => c.Endereco).HasMaxLength(512);
            builder.Property(c => c.Servicos);
        }
    }
}
