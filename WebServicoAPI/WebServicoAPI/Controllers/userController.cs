using Microsoft.AspNetCore.Mvc;
using OficinasMecanicas.Aplicacao.DTO.Usuarios;
using OficinasMecanicas.Aplicacao.Interfaces;
using OficinasMecanicas.Aplicacao.Model;
using OficinasMecanicas.Aplicacao.Servicos;
using OficinasMecanicas.Dominio.Interfaces;
using WebServicoAPI.JWT;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebServicoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class userController : ControllerBase
    {
        private readonly IUsuarioAppServico _usuarioAppServico;
        private readonly ILogger<authController> _logger;
        private readonly IJWToken _jwToken;
        private readonly INotificador _notificador;

        public userController(ILogger<authController> logger, IUsuarioAppServico usuarioAppServico, INotificador notificador, IJWToken jwToken)
        {
            _logger = logger;
            _usuarioAppServico = usuarioAppServico;
            _jwToken = jwToken;
            _notificador = notificador;
        }

        [HttpGet]
        [ProducesResponseType(typeof(Resposta<IList<UsuariosTelaInicialDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Resposta<object>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Get()//Lista todas os usuários        
        {
            try
            {
                var usuarios = await _usuarioAppServico.BuscarTodos();
                if (usuarios == null)
                    throw new Exception("Usuários não cadastrados.");


                return Ok(new Resposta<IList<UsuariosTelaInicialDTO>>
                {
                    sucesso = true,
                    mensagem = "Usuários obtidos com sucesso.",
                    dados = usuarios
                });
            }
            catch (Exception e)
            {
                return BadRequest(new Resposta<UserToken>
                {
                    sucesso = false,
                    mensagem = "Erro na busca de usuários => " + e.Message,
                    dados = null
                });
            }
        }

        
    }
}
