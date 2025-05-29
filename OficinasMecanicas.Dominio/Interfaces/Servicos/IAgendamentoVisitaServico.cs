using OficinasMecanicas.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OficinasMecanicas.Dominio.Interfaces.Servicos
{
    public interface IAgendamentoVisitaServico
    {
        Task<AgendamentoVisita?> Adicionar(AgendamentoVisita agenda);
        Task Atualizar(AgendamentoVisita agenda);
        Task Excluir(Guid id);
        Task<IEnumerable<AgendamentoVisita>> BuscarTodos();
        Task<AgendamentoVisita?> BuscarPorId(Guid id);
        Task<IList<AgendamentoVisita>?> BuscarPorDatas(DateTime dtInicio, DateTime dtfinal);
        Task<IList<AgendamentoVisita>?> BuscarPorDescricao(string descricao);
        
    }
}
