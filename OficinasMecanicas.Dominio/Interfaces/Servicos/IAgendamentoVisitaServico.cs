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
        Task Atualizar(Guid id,AgendamentoVisita agenda);
        Task Excluir(Guid id);
        Task<IEnumerable<AgendamentoVisita>> BuscarTodos();

        Task<AgendamentoVisita?> BuscarPorId(Guid id);
        Task<IEnumerable<AgendamentoVisita>?> BuscarPorDatas(DateTime dtInicio, DateTime dtfinal);
        Task<IEnumerable<AgendamentoVisita>?> BuscarPorDescricao(string descricao);
        
    }
}
