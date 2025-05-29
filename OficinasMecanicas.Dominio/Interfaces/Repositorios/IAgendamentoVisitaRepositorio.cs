using OficinasMecanicas.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OficinasMecanicas.Dominio.Interfaces.Repositorios
{
    public interface IAgendamentoVisitaRepositorio
    {
        Task<AgendamentoVisita> Adicionar(AgendamentoVisita agenda, CancellationToken cancellationToken = default);
        Task<AgendamentoVisita> Atualizar(AgendamentoVisita agenda, CancellationToken cancellationToken = default);
        Task<AgendamentoVisita> Excluir(Guid id, CancellationToken cancellationToken = default);

        Task<IEnumerable<AgendamentoVisita>> BuscarTodos();
        Task<AgendamentoVisita?> BuscarPorId(Guid id);
        Task<IList<AgendamentoVisita>?> BuscarPorDatas(DateTime dtInicio, DateTime dtfinal);
        Task<IList<AgendamentoVisita>?> BuscarPorDescricao(string descricao);        
    }
}
