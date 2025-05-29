using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WebServicoAPI.Models;

public partial class OficinasMecanicasContext : DbContext
{
    public OficinasMecanicasContext()
    {
    }
    public OficinasMecanicasContext(DbContextOptions<OficinasMecanicasContext> options)  : base(options)
    {
    }

    public virtual DbSet<AgendamentoVisita> AgendamentoVisita { get; set; }

    public virtual DbSet<OficinaMecanica> OficinaMecanica { get; set; }

    public virtual DbSet<ResetarSenha> ResetarSenha { get; set; }

    public virtual DbSet<ServicosPrestados> ServicosPrestados { get; set; }

    public virtual DbSet<Usuarios> Usuarios { get; set; }

    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //   => optionsBuilder.UseSqlServer("Data Source=localhost\\SQL2019;Initial Catalog=OficinasMecanicas;Persist Security Info=True;User ID=sa;Password=1478;Encrypt=False;Trust Server Certificate=True;Command Timeout=300", x => x.UseHierarchyId());

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AgendamentoVisita>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_AgendamentosVisita");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.IdOficinaNavigation).WithMany(p => p.AgendamentoVisita).HasConstraintName("FK_AgendamentoVisita_OficinaMecanica");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.AgendamentoVisita).HasConstraintName("FK_AgendamentoVisita_Usuarios");
        });

        modelBuilder.Entity<Usuarios>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Usuario");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
        });

        modelBuilder.Entity<ResetarSenha>(entity =>
        {
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Efetivado).HasDefaultValue(false);
            entity.Property(e => e.Excluido).HasDefaultValue(false);

            entity.HasOne(d => d.Usuario).WithMany(p => p.ResetarSenha)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ResetarSenha_Usuarios");
        });


        modelBuilder.Entity<OficinaMecanica>(entity =>
        {
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
        });

        

        modelBuilder.Entity<ServicosPrestados>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Servico");
        });

        

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
