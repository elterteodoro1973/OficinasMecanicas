using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OficinasMecanicas.Dados.Contexto;
using OficinasMecanicas.Dominio.Entidades;
using OficinasMecanicas.Dominio.Interfaces.Repositorios;

namespace OficinasMecanicas.Dados.Repositorios
{
    public class AgendamentoVisitaRepositorio : IAgendamentoVisitaRepositorio
    {
        private readonly DbContexto _contexto;
        readonly ILogger<UsuarioRepositorio> _logger;
        public AgendamentoVisitaRepositorio(DbContexto contexto, ILogger<UsuarioRepositorio> _logger)
        {
            _contexto = contexto;
            _logger = _logger;
        }

        public async Task<AgendamentoVisita> Adicionar(AgendamentoVisita agenda, CancellationToken cancellationToken = default)
        {
            await _contexto.AgendamentoVisita.AddAsync(agenda, cancellationToken);
            await _contexto.SaveChangesAsync(cancellationToken);
            return agenda;
        }

        public async Task<AgendamentoVisita> Atualizar(AgendamentoVisita agenda, CancellationToken cancellationToken = default)
        {
            _contexto.AgendamentoVisita.Update(agenda);
            await _contexto.SaveChangesAsync(cancellationToken);
            return agenda;
        }

        public async Task<AgendamentoVisita> Excluir(Guid id, CancellationToken cancellationToken = default)
        {
            var agenda = await _contexto.AgendamentoVisita.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
            if (agenda == null)
            {
                throw new InvalidOperationException("Oficina não encontrada.");
            }
            _contexto.AgendamentoVisita.Remove(agenda);
            await _contexto.SaveChangesAsync(cancellationToken);

            return agenda;
        }

        public async Task<AgendamentoVisita?> BuscarPorId(Guid id)
        => await _contexto.AgendamentoVisita.Where(c => c.Id == id).AsNoTracking().FirstOrDefaultAsync();
        public async Task<IEnumerable<AgendamentoVisita>> BuscarTodos()=> await _contexto.AgendamentoVisita.AsNoTracking().ToListAsync();

        public async Task<IList<AgendamentoVisita>?> BuscarPorDescricao(string descricao) => await _contexto.AgendamentoVisita.
            Where(c => c.Descricao.ToLower().Contains(descricao.ToLower())).ToListAsync();       
        
        public async Task<IList<AgendamentoVisita>?> BuscarPorDatas(DateTime dtInicio, DateTime dtfinal)=> await _contexto.AgendamentoVisita
                .Where(c => c.DataHora.Value.Date >= dtInicio.Date && c.DataHora.Value.Date <= dtfinal.Date).ToListAsync();
    }

}
