using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OficinasMecanicas.Dados.Contexto;
using OficinasMecanicas.Dominio.Entidades;
using OficinasMecanicas.Dominio.Interfaces.Repositorios;

namespace OficinasMecanicas.Dados.Repositorios
{
    public class ResetarSenhaRepositorio :  IResetarSenhaRepositorio
    {
        private readonly DbContexto _contexto;
        readonly ILogger<UsuarioRepositorio> _logger;
        public ResetarSenhaRepositorio(DbContexto contexto, ILogger<UsuarioRepositorio> _logger)
        {
            _contexto = contexto;
            _logger = _logger;
        }

        public async Task<ResetarSenha> Adicionar(ResetarSenha resetarSenha, CancellationToken cancellationToken)
        {
            await _contexto.ResetarSenha.AddAsync(resetarSenha, cancellationToken);
            await _contexto.SaveChangesAsync(cancellationToken);
            return resetarSenha;
        }

        public async Task<ResetarSenha> Atualizar(ResetarSenha resetarSenha, CancellationToken cancellationToken = default)
        {
            _contexto.ResetarSenha.Update(resetarSenha);
            await _contexto.SaveChangesAsync(cancellationToken);
            return resetarSenha;
        }

        public async Task<ResetarSenha> Excluir(Guid id, CancellationToken cancellationToken = default)
        {
            var resetarSenha = await _contexto.ResetarSenha.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            if (resetarSenha == null)
            {
                throw new InvalidOperationException("resetar Senha não encontrado.");
            }

            // Remove o usuário do Data Context
            _contexto.ResetarSenha.Remove(resetarSenha);

            // Persiste as mudanças
            await _contexto.SaveChangesAsync(cancellationToken);

            return resetarSenha;
        }

        public async Task<ResetarSenha?> BuscarResetarSenhaPorId(Guid id)
        => await _contexto.ResetarSenha.Where(c => c.Id == id && !c.Excluido.Value).AsNoTracking().FirstOrDefaultAsync();

        public async Task<ResetarSenha?> BuscarResetarSenhaPorToken(string token)
       => await _contexto.ResetarSenha.Where(c => c.Token == token && !c.Excluido.Value).AsNoTracking().FirstOrDefaultAsync();

        public async Task<IList<ResetarSenha>> ListarResetarSenha()
        => await _contexto.ResetarSenha.Where(c => !c.Excluido.Value).AsNoTracking().ToListAsync();

        
    }
}
