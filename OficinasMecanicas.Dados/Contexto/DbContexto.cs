using System;

using Microsoft.EntityFrameworkCore;
using OficinasMecanicas.Dados.Mapeamentos;
using OficinasMecanicas.Dominio.Entidades;
using OficinasMecanicas.Dominio.Interfaces.Servicos;

namespace OficinasMecanicas.Dados.Contexto
{
    public class DbContexto : DbContext
    {
        //private readonly ILogServico _logServico;
        public DbContexto(DbContextOptions<DbContexto> options) : base(options)
        {
           // _logServico = logServico;
        }

        
        public virtual DbSet<Usuarios> Usuarios { get; set; }
        public virtual DbSet<ResetarSenha> ResetarSenha { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new UsuarioMapeamento());
           
            builder.ApplyConfiguration(new ResetarSenhaMapeamento());
        }

        public override int SaveChanges()
        {
            SalvarLog().ConfigureAwait(false).GetAwaiter().GetResult();

            foreach (var item in ChangeTracker.Entries()
               .Where(e => e.State == EntityState.Deleted &&
               e.Metadata.GetProperties().Any(x => x.Name == "Excluido")))
            {
                item.State = EntityState.Unchanged;
                item.CurrentValues["Excluido"] = true;
            }
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await SalvarLog();
            foreach (var item in ChangeTracker.Entries()
              .Where(e => e.State == EntityState.Deleted &&
              e.Metadata.GetProperties().Any(x => x.Name == "Excluido")))
            {
                item.State = EntityState.Unchanged;
                item.CurrentValues["Excluido"] = true;
            }

            return await base.SaveChangesAsync(cancellationToken);
        }

        private async Task SalvarLog()
        {
            try
            {
                ChangeTracker.DetectChanges();

                
            }
            catch (Exception)
            {

            }
        }
    }
}