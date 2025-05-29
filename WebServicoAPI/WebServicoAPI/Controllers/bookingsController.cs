using Microsoft.AspNetCore.Mvc;
using OficinasMecanicas.Aplicacao.DTO.Agenda;
using OficinasMecanicas.Aplicacao.Interfaces;
using OficinasMecanicas.Aplicacao.Model;
using OficinasMecanicas.Dominio.Entidades;
using OficinasMecanicas.Dominio.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebServicoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class bookingsController : ControllerBase
    {

        private readonly IAgendaVisitaAppServico _agendaVisitaAppServico;
        private readonly ILogger<authController> _logger;
        private readonly INotificador _notificador;

        public bookingsController(ILogger<authController> logger, IAgendaVisitaAppServico agendaVisitaAppServico, INotificador notificador)
        {
            _logger = logger;
            _agendaVisitaAppServico = agendaVisitaAppServico;
            _notificador = notificador;
        }

        [HttpPost] //Cria nova oficina
        [ProducesResponseType(typeof(Resposta<EditarAgendamentoVisitaDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Resposta<object>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Post([FromBody] CadastrarAgendamentoVisitaDTO oficina)
        {
            try
            {
                var agendaResult = await _agendaVisitaAppServico.Adicionar(oficina);
                if (agendaResult == null)
                    throw new Exception("O Agendamento não pode ser cadastrada.");

                if (_notificador.TemNotificacao())
                    throw new Exception(_notificador.ObterNotificacoes().FirstOrDefault()?.Mensagem);

                return Ok(new Resposta<EditarAgendamentoVisitaDTO>
                {
                    sucesso = true,
                    mensagem = "Agendamento cadastrado com sucesso.",
                    dados = agendaResult
                });
            }
            catch (Exception e)
            {
                return BadRequest(new Resposta<EditarAgendamentoVisitaDTO>
                {
                    sucesso = false,
                    mensagem = "Erro no cadastro do Agendamento => " + e.Message,
                    dados = null
                });
            }
        }


        [HttpPut("{id}")] //Atualiza dados da oficina
        [ProducesResponseType(typeof(Resposta<EditarAgendamentoVisitaDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Resposta<object>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Put(Guid id, [FromBody] CadastrarAgendamentoVisitaDTO oficina)
        {
            try
            {
                var agendaResult = await _agendaVisitaAppServico.Atualizar(id, oficina);
                if (agendaResult == null)
                    throw new Exception("O Agendamento não pode ser alterada.");

                if (_notificador.TemNotificacao())
                    throw new Exception(_notificador.ObterNotificacoes().FirstOrDefault().Mensagem?? "O Agendamento não pode ser alterada.");

                return Ok(new Resposta<EditarAgendamentoVisitaDTO>
                {
                    sucesso = true,
                    mensagem = "Agendamento alterado com sucesso.",
                    dados = agendaResult
                });
            }
            catch (Exception e)
            {
                return BadRequest(new Resposta<UserToken>
                {
                    sucesso = false,
                    mensagem = "Erro na alteração do agendamento => " + e.Message,
                    dados = null
                });
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(Resposta<EditarAgendamentoVisitaDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Resposta<object>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Delete(Guid id)//Remove uma oficina
        {
            try
            {
                var agendaResult = await _agendaVisitaAppServico.BuscarPorId(id);
                await _agendaVisitaAppServico.Excluir(id);
                return Ok(new Resposta<EditarAgendamentoVisitaDTO>
                {
                    sucesso = true,
                    mensagem = "Agendamento removida com sucesso",
                    dados = agendaResult
                });
            }
            catch (Exception e)
            {
                return BadRequest(new Resposta<EditarAgendamentoVisitaDTO>
                {
                    sucesso = false,
                    mensagem = "Erro ao remover o agendamento => " + e.Message,
                    dados = null
                });
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(Resposta<IEnumerable<AgendamentosVisitasTelaInicialDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Resposta<object>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Get()//Lista todos agendamentos
        {
            try
            {
                var agendaResult = await _agendaVisitaAppServico.BuscarTodos();               

                if (agendaResult == null) 
                    throw new Exception("Os agendamentos não poderam serem listados.");

                return Ok(new Resposta<IEnumerable<AgendamentosVisitasTelaInicialDTO>>
                {
                    sucesso = true,
                    mensagem = "Agendamento listados com sucesso.",
                    dados = agendaResult
                });
            }
            catch (Exception e)
            {
                return BadRequest(new Resposta<IEnumerable<AgendamentosVisitasTelaInicialDTO>>
                {
                    sucesso = false,
                    mensagem = "Erro ao listar agendamentos => " + e.Message,
                    dados = null
                });
            }
        }

        [HttpGet("{id}")]//Detalhes de uma oficina
        [ProducesResponseType(typeof(Resposta<EditarAgendamentoVisitaDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Resposta<object>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Get(Guid id)
        {
            try
            {
                var agendaResult = await _agendaVisitaAppServico.BuscarPorId(id);
                if (agendaResult == null)
                    throw new Exception("Agendamento não encontrado.");

                return Ok(new Resposta<EditarAgendamentoVisitaDTO>
                {
                    sucesso = true,
                    mensagem = "Agendamento encontrado com sucesso.",
                    dados = agendaResult
                });
            }
            catch (Exception e)
            {
                return BadRequest(new Resposta<EditarAgendamentoVisitaDTO>
                {
                    sucesso = false,
                    mensagem = "Erro na busca do Agendamento => " + e.Message,
                    dados = null
                });
            }
        }


    }
}
