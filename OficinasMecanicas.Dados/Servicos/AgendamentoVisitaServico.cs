using Microsoft.AspNetCore.Http;
using OficinasMecanicas.Dominio.Entidades;
using OficinasMecanicas.Dominio.Entidades.Validacoes.Agenda;
using OficinasMecanicas.Dominio.Interfaces;
using OficinasMecanicas.Dominio.Interfaces.Repositorios;
using OficinasMecanicas.Dominio.Interfaces.Servicos;
using OficinasMecanicas.Dominio.Notificacoes;
using System.Runtime.CompilerServices;

namespace OficinasMecanicas.Dados.Servicos
{
    public class AgendamentoVisitaServico : BaseServico<AgendamentoVisita>, IAgendamentoVisitaServico
    {
        private readonly IHttpContextAccessor _httpContext;
        private readonly IAgendamentoVisitaRepositorio _AgendamentoVisitaRepositorio;
        private readonly INotificador _notificador;

        public AgendamentoVisitaServico(
            IHttpContextAccessor httpContext,
            IAgendamentoVisitaRepositorio AgendamentoVisitaRepositorio,
            INotificador notificador) : base(notificador)
        {
            _httpContext = httpContext;
            _AgendamentoVisitaRepositorio = AgendamentoVisitaRepositorio;
            _notificador = notificador;
        }
        public async Task<AgendamentoVisita?> Adicionar(AgendamentoVisita agenda)
        {
            try
            {
                agenda.Id = Guid.NewGuid();
                await _ValidarInclusao(agenda);
                if (_notificador.TemNotificacao()) return null;
                await _AgendamentoVisitaRepositorio.Adicionar(agenda);
                return agenda;
            }
            catch (Exception ex)
            {
                _notificador.Adicionar(new Notificacao("Erro ao adicionar agenda !" + ex.Message));
                return agenda;
            }
        }

        public async Task Atualizar(Guid id , AgendamentoVisita agenda)
        {
            
            try
            {
                var agendaDB = await _AgendamentoVisitaRepositorio.BuscarPorId(id);

                if (agendaDB == null)
                {
                    _notificador.Adicionar(new Notificacao("Agendamento não encontrada!"));
                    return;
                }
                await _ValidarEdicao(agenda);

                if (_notificador.TemNotificacao()) return;

                agendaDB.Descricao = agenda.Descricao;
                agendaDB.DataHora = agenda.DataHora;
                agendaDB.IdUsuario = agenda.IdUsuario;
                agendaDB.IdOficina = agenda.IdOficina;
                await _AgendamentoVisitaRepositorio.Atualizar(agendaDB);
            }
            catch (Exception ex)
            {
                _notificador.Adicionar(new Notificacao("Erro ao editar a agenda !" + ex.Message));
            }
        }

        public async Task Excluir(Guid id)
        {
            await _ValidarExclusao(id);

            if (_notificador.TemNotificacao()) return;

            var agendaDB = await _AgendamentoVisitaRepositorio.BuscarPorId(id);
            if (agendaDB == null)
            {
                _notificador.Adicionar(new Notificacao("Agendamento não encontrada!"));
                return;
            }

            try
            {
                await _AgendamentoVisitaRepositorio.Excluir(id);
            }
            catch (Exception ex)
            {
                _notificador.Adicionar(new Notificacao("Erro ao excluir o  Agendamento !"));
            }
        }

        private async Task _ValidarInclusao(AgendamentoVisita agenda)
        {
            if (!ExecutarValidacao<CadastrarEditarAgendaValidacao, AgendamentoVisita>(new CadastrarEditarAgendaValidacao(true), agenda)) return;

            if (_notificador.TemNotificacao()) return;

            var agendaUsuarioOficina = await _AgendamentoVisitaRepositorio.BuscarPorDatas(agenda.DataHora.Value, agenda.DataHora.Value);            
            if (agendaUsuarioOficina != null)
            {               
                var agendaUsuario = agendaUsuarioOficina.Where(c => c.IdUsuario == agenda.IdUsuario).Any();
                if (agendaUsuario)
                {
                  _notificador.Adicionar(new Notificacao("O usuário já tem agendamento !"));
                    return;
                }

                var agendaOficinas = agendaUsuarioOficina.Where(c => c.IdOficina == agenda.IdOficina &&  c.DataHora.Value.Hour == agenda.DataHora.Value.Hour).Count();

                if (agendaOficinas > 2 )
                {
                    _notificador.Adicionar(new Notificacao("Não  há agenda disponivel neste horário neste oficina !"));
                    return;
                }
            }
        }
        private async Task _ValidarEdicao(AgendamentoVisita agenda)
        {
            if (!ExecutarValidacao<CadastrarEditarAgendaValidacao, AgendamentoVisita>(new CadastrarEditarAgendaValidacao(true), agenda)) return;

            if (_notificador.TemNotificacao()) return;

            var agendaUsuarioOficina = await _AgendamentoVisitaRepositorio.BuscarPorDatas(agenda.DataHora.Value, agenda.DataHora.Value);
            if (agendaUsuarioOficina != null)
            {
                var agendaUsuario = agendaUsuarioOficina.Where(c => c.IdUsuario == agenda.IdUsuario).Any();
                if (agendaUsuario)
                {
                    _notificador.Adicionar(new Notificacao("O usuário já tem agendamento !"));
                    return;
                }

                var agendaOficinas = agendaUsuarioOficina.Where(c => c.IdOficina == agenda.IdOficina && c.DataHora.Value.Hour == agenda.DataHora.Value.Hour).Count();

                if (agendaOficinas > 2)
                {
                    _notificador.Adicionar(new Notificacao("Não  há agenda disponivel neste horário neste oficina !"));
                    return;
                }
            }

        }
        private async Task _ValidarExclusao(Guid id)
        {
            if (id == Guid.Empty)
            {
                _notificador.Adicionar(new Notificacao("Identificador da agenda obrigatório !"));
            }
        }

        public async Task<IEnumerable<AgendamentoVisita>> BuscarTodos() => await _AgendamentoVisitaRepositorio.BuscarTodos();
        public async Task<AgendamentoVisita?> BuscarPorId(Guid id) => await _AgendamentoVisitaRepositorio.BuscarPorId(id);

        public async Task<IEnumerable<AgendamentoVisita>?> BuscarPorDescricao(string descricao)
        {
            return await _AgendamentoVisitaRepositorio.BuscarPorDescricao(descricao);
        }
        public async Task<IEnumerable<AgendamentoVisita>?> BuscarPorDatas(DateTime dtInicio, DateTime dtfinal)
        {
            return await _AgendamentoVisitaRepositorio.BuscarPorDatas(dtInicio, dtfinal);
        }
    }

}
