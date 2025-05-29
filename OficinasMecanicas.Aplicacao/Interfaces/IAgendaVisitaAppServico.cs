using OficinasMecanicas.Aplicacao.DTO.Agenda;
using OficinasMecanicas.Aplicacao.DTO.Oficinas;
using OficinasMecanicas.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OficinasMecanicas.Aplicacao.Interfaces
{
    public interface IAgendaVisitaAppServico
    {
        Task<EditarAgendamentoVisitaDTO?> Adicionar(CadastrarAgendamentoVisitaDTO dto);
        Task<EditarAgendamentoVisitaDTO?> Atualizar(Guid id , CadastrarAgendamentoVisitaDTO? dto);
        Task<bool> Excluir(Guid id);

        Task<IList<AgendamentosVisitasTelaInicialDTO>> ListarAgendamentoVisitasTelaInicial(string? filtro);
        Task<IEnumerable<AgendamentosVisitasTelaInicialDTO>> BuscarTodos();
        Task<EditarAgendamentoVisitaDTO?> BuscarPorId(Guid id);
        Task<IEnumerable<AgendamentosVisitasTelaInicialDTO>?> BuscarPorDatas(DateTime dtInicio, DateTime dtfinal);
        Task<IEnumerable<AgendamentosVisitasTelaInicialDTO>?> BuscarPorDescricao(string descricao);
    }
}
