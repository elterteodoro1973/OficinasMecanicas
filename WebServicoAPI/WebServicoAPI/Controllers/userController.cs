using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OficinasMecanicas.Aplicacao.DTO.Usuarios;
using OficinasMecanicas.Aplicacao.Interfaces;
using OficinasMecanicas.Aplicacao.Model;
using OficinasMecanicas.Dominio.Interfaces;
using WebServicoAPI.JWT;


namespace WebServicoAPI.Controllers
{
    [Authorize] // Moved [Authorize] to the class level where it is valid.  
    [Route("api/[controller]")]
    [ApiController]
    public class userController : ControllerBase
    {
        private readonly IUsuarioAppServico _usuarioAppServico;
        private readonly ILogger<authController> _logger;
        private readonly IJWToken _jwToken;
        private readonly INotificador _notificador;
        private readonly IMapper _mapper; // Add a private field for IMapper.

        public userController(ILogger<authController> logger, IUsuarioAppServico usuarioAppServico, INotificador notificador, IJWToken jwToken, IMapper mapper)
        {
            _logger = logger;
            _usuarioAppServico = usuarioAppServico;
            _jwToken = jwToken;
            _notificador = notificador;
            _mapper = mapper; // Initialize the IMapper instance.
        }

        [HttpGet]
        [ProducesResponseType(typeof(Resposta<IList<UsuariosTelaInicialDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Resposta<object>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Get()
        {
            try
            {
                var usuarios = _mapper.Map<IList<UsuariosTelaInicialDTO>>(await _usuarioAppServico.BuscarTodos()); 
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
                return BadRequest(new Resposta<object>
                {
                    sucesso = false,
                    mensagem = "Erro na busca de usuários => " + e.Message,
                    dados = null
                });
            }
        }
    }
}
