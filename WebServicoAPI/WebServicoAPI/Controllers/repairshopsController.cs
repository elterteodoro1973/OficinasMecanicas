using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using OficinasMecanicas.Aplicacao.DTO.Oficinas;
using OficinasMecanicas.Aplicacao.Interfaces;
using OficinasMecanicas.Aplicacao.Model;
using OficinasMecanicas.Dominio.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebServicoAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class repairshopsController : ControllerBase
    {
        private readonly IOficinaAppServico _oficinaAppServico;
        private readonly ILogger<authController> _logger;        
        private readonly INotificador _notificador;

        public repairshopsController(ILogger<authController> logger, IOficinaAppServico oficinaAppServico, INotificador notificador)
        {
            _logger = logger;
            _oficinaAppServico = oficinaAppServico;            
            _notificador = notificador;
        }

        [HttpPost] //Cria nova oficina
        [ProducesResponseType(typeof(Resposta<EditarOficinaDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Resposta<object>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Post([FromBody] CadastrarOficinaDTO oficina)
        {            
            try
            {
                var oficinaResult = await _oficinaAppServico.Adicionar(oficina);
                if (oficinaResult == null)
                    throw new Exception("a oficina não pode ser cadastrada.");                

                if (_notificador.TemNotificacao())
                    throw new Exception(_notificador.ObterNotificacoes().FirstOrDefault()?.Mensagem);

                return Ok(new Resposta<EditarOficinaDTO>
                {
                    sucesso = true,
                    mensagem = "Oficina cadastrada com sucesso.",
                    dados = oficinaResult
                });
            }
            catch (Exception e)
            {
                return BadRequest(new Resposta<EditarOficinaDTO>
                {
                    sucesso = false,
                    mensagem = "Erro no cadastro de oficina => " + e.Message,
                    dados = null
                });
            }
        }


        [HttpPut("{id}")] //Atualiza dados da oficina
        [ProducesResponseType(typeof(Resposta<EditarOficinaDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Resposta<object>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Put(Guid id, [FromBody] CadastrarOficinaDTO oficina)
        {
            try
            {
                var oficinaResult = await _oficinaAppServico.Atualizar(id,oficina);
                if (oficinaResult == null)
                    throw new Exception("A oficina não pode ser alterada.");

                if (_notificador.TemNotificacao())
                    throw new Exception(_notificador.ObterNotificacoes().FirstOrDefault()?.Mensagem);

                return Ok(new Resposta<EditarOficinaDTO>
                {
                    sucesso = true,
                    mensagem = "Oficina alterada com sucesso.",
                    dados = oficinaResult
                });
            }
            catch (Exception e)
            {
                return BadRequest(new Resposta<EditarOficinaDTO>
                {
                    sucesso = false,
                    mensagem = "Erro na alteração da oficina => " + e.Message,
                    dados = null
                });
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(Resposta<EditarOficinaDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Resposta<object>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Delete(Guid id)//Remove uma oficina
        {
            try
            {
                var oficinaResult = await _oficinaAppServico.BuscarPorId(id);
                await _oficinaAppServico.Excluir(id);
                return Ok(new Resposta<EditarOficinaDTO>
                {
                    sucesso = true,
                    mensagem = "Oficina removida com sucesso",
                    dados = oficinaResult
                });
            }
            catch (Exception e)
            {
                return BadRequest(new Resposta<EditarOficinaDTO>
                {
                    sucesso = false,
                    mensagem = "Erro ao remover oficina=> " + e.Message,
                    dados = null
                });
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(Resposta<IEnumerable<OficinasTelaInicialDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Resposta<object>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Get()//Lista todas as oficinas
        {
            try
            {
                var oficinaResult = await _oficinaAppServico.BuscarTodos();
                if (oficinaResult == null)
                    throw new Exception("As oficinas não poderam serem listadas.");

                return Ok(new Resposta<IEnumerable<OficinasTelaInicialDTO>>
                {
                    sucesso = true,
                    mensagem = "Oficina listada com sucesso.",
                    dados = oficinaResult
                });
            }
            catch (Exception e)
            {
                return BadRequest(new Resposta<IEnumerable<OficinasTelaInicialDTO>>
                {
                    sucesso = false,
                    mensagem = "Erro ao listar oficinas => " + e.Message,
                    dados = null
                });
            }
        }



        [HttpGet("{id}")]//Detalhes de uma oficina
        [ProducesResponseType(typeof(Resposta<EditarOficinaDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Resposta<object>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Get(Guid id)
        {
            try
            {
                var oficinaResult = await _oficinaAppServico.BuscarPorId(id);
                if (oficinaResult == null)
                    throw new Exception("Oficina não encontrada.");

                return Ok(new Resposta<EditarOficinaDTO>
                {
                    sucesso = true,
                    mensagem = "Oficina encontrada com sucesso.",
                    dados = oficinaResult
                });
            }
            catch (Exception e)
            {
                return BadRequest(new Resposta<EditarOficinaDTO>
                {
                    sucesso = false,
                    mensagem = "Erro na busca da oficina => " + e.Message,
                    dados = null
                });
            }
        }
    }
}
