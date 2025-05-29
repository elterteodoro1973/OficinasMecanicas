using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using OficinasMecanicas.Aplicacao.DTO.Agenda;
using OficinasMecanicas.Aplicacao.DTO.Oficinas;
using OficinasMecanicas.Aplicacao.Interfaces;
using OficinasMecanicas.Dominio.Entidades;
using OficinasMecanicas.Dominio.Interfaces.Servicos;
using OficinasMecanicas.Dominio.Interfaces;
using OficinasMecanicas.Dominio.Notificacoes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace OficinasMecanicas.Aplicacao.Servicos
{
    public class AgendaVisitaAppServico : IAgendaVisitaAppServico
    {
        private readonly IAgendamentoVisitaServico _agendamentoVisitaServico;
        private readonly INotificador _notificador;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContext;
        public AgendaVisitaAppServico(IHttpContextAccessor httpContext, IMapper mapper, INotificador notificador,
                                  IAgendamentoVisitaServico agendamentoVisitaServico, IConfiguration configuration)
        {
            _httpContext = httpContext;
            _mapper = mapper;
            _notificador = notificador;
            _agendamentoVisitaServico = agendamentoVisitaServico;
            _configuration = configuration;
        }
        public async Task<IList<AgendamentosVisitasTelaInicialDTO>> ListarAgendamentoVisitasTelaInicial(string? filtro)
        {
            var dtos = _mapper.Map<IList<AgendamentosVisitasTelaInicialDTO>>(await _agendamentoVisitaServico.BuscarTodos());
            if (!string.IsNullOrEmpty(filtro) && !string.IsNullOrWhiteSpace(filtro))
            {
                dtos = dtos.Where(c => c.Descricao.ToUpper().Contains(filtro.ToUpper())).ToList();
            }
            return dtos;
        }
        public async Task<EditarAgendamentoVisitaDTO> Adicionar(CadastrarAgendamentoVisitaDTO dto)
        {
            var agendamentoVisita = _mapper.Map<AgendamentoVisita>(dto);
            var novoUsuario = await _agendamentoVisitaServico.Adicionar(agendamentoVisita);
            return _mapper.Map<EditarAgendamentoVisitaDTO>(agendamentoVisita);
        }
        public async Task<EditarAgendamentoVisitaDTO?> Atualizar(Guid id, CadastrarAgendamentoVisitaDTO? dto)
        {
            if (dto == null || id == null)
            {
                _notificador.Adicionar(new Notificacao("Oficiona inválida !"));
                return null;
            }
            var oficina = _mapper.Map<AgendamentoVisita>(dto);
            oficina.Id = id;
            await _agendamentoVisitaServico.Atualizar(id,oficina);
            return _mapper.Map<EditarAgendamentoVisitaDTO>(oficina);
        }
        public async Task<bool> Excluir(Guid id)
        {
            await _agendamentoVisitaServico.Excluir(id);
            return !_notificador.TemNotificacao();
        }        
        public async Task<EditarAgendamentoVisitaDTO?> BuscarPorId(Guid id)
        {
            var dtos = _mapper.Map<EditarAgendamentoVisitaDTO>(await _agendamentoVisitaServico.BuscarPorId(id));
            return dtos;
        }
        public async Task<IEnumerable<AgendamentosVisitasTelaInicialDTO>> BuscarTodos()
        {  
            var listaAgenda = await _agendamentoVisitaServico.BuscarTodos();
            var agendaResult = listaAgenda.Select(a => new AgendamentosVisitasTelaInicialDTO
            {
                Id = a.Id,
                IdUsuario = a.IdUsuario.Value,
                IdOficina = a.IdOficina.Value,
                NomeUsuario = a.IdUsuarioNavigation?.Username ?? "Usuário não encontrado",
                NomeOficina = a.IdOficinaNavigation?.Nome ?? "Oficina não encontrada",
                DataHora = a.DataHora.Value,
                Descricao = a.Descricao

            }).ToList();

            return agendaResult;
        }

        public async Task<IEnumerable<AgendamentosVisitasTelaInicialDTO>?> BuscarPorDatas(DateTime dtInicio, DateTime dtfinal)
        {
            var dtos = _mapper.Map<IEnumerable<AgendamentosVisitasTelaInicialDTO>>(await _agendamentoVisitaServico.BuscarPorDatas(dtInicio, dtfinal));
            return dtos;
        }
        public async Task<IEnumerable<AgendamentosVisitasTelaInicialDTO>?> BuscarPorDescricao(string descricao)
        {
            var dtos = _mapper.Map<IEnumerable<AgendamentosVisitasTelaInicialDTO>>(await _agendamentoVisitaServico.BuscarPorDescricao(descricao));
            return dtos;
        }
    }
}
