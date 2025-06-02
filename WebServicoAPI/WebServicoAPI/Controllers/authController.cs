using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using OficinasMecanicas.Aplicacao.DTO.Usuarios;
using OficinasMecanicas.Aplicacao.Interfaces;
using OficinasMecanicas.Aplicacao.Model;
using OficinasMecanicas.Dominio.Entidades;
using OficinasMecanicas.Dominio.Interfaces;
using WebServicoAPI.JWT;
using Microsoft.AspNetCore.Authorization;

namespace WebServicoAPI.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class authController : ControllerBase
    {
        private readonly IUsuarioAppServico _usuarioAppServico;
        private readonly ILogger<authController> _logger;
        private readonly IJWToken _jwToken;
        private readonly INotificador _notificador;

        public authController(ILogger<authController> logger, IUsuarioAppServico usuarioAppServico, INotificador notificador, IJWToken jwToken)
        {
            _logger = logger;
            _usuarioAppServico = usuarioAppServico;
            _jwToken = jwToken;
            _notificador = notificador;
        }

        [HttpPost("login")]
        [ProducesResponseType(typeof(Resposta<UserToken>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Resposta<object>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> logar(LogarDTO loginDTO)
        {
            try
            {
                var usuario = await _usuarioAppServico.BuscarPorEmail(loginDTO.Email);
                if (usuario == null)
                    throw new Exception("Email não cadastrado.");

                if (!await _usuarioAppServico.SenhaValidaLogin(loginDTO.Email, loginDTO.Senha))
                    throw new Exception("Senha inválida.");               

                return Ok(new Resposta<UserToken>
                {
                    sucesso = true,
                    mensagem = "Login realizado com sucesso",
                    dados = new UserToken { Id = usuario.Id.ToString(), Username = usuario.Username, Email = usuario.Email, 
                                            Token = _jwToken.GenerateToken(usuario.Id, usuario.Email).ToString()
                    }
                });
            }
            catch (Exception e)
            {
                return BadRequest(new Resposta<UserToken>
                {
                    sucesso = false,
                    mensagem = "Erro no login => " + e.Message,
                    dados = null
                });
            }
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserToken?>> Register([FromBody] CadastrarUsuarioDTO usuarioDTO)
        {
            if (usuarioDTO == null)
                return BadRequest("Dados inválido.");

            try
            {
                if (await _usuarioAppServico.EmailPrincipalJaCadastrado(usuarioDTO.Email, null))
                    return BadRequest("Email ja cadastrado.");

                if (await _usuarioAppServico.NomePrincipalJaCadastrado(usuarioDTO.Nome, null))
                    return BadRequest("usuário ja cadastrado.");

                var usuario = await _usuarioAppServico.Adicionar("", usuarioDTO);

                if (_notificador.TemNotificacao())
                {                    
                    throw new Exception(_notificador.ObterNotificacoes().FirstOrDefault()?.Mensagem);
                }

                return Ok(new Resposta<UserToken>
                {
                    sucesso = true,
                    mensagem = "Usuário criado com sucesso",
                    dados = new UserToken
                    {
                        Id = usuario.Id.ToString(),
                        Username = usuario.Nome,
                        Email = usuario.Email,
                        Token = _jwToken.GenerateToken(usuario.Id.Value, usuarioDTO.Email).ToString()
                    }
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new Resposta<UserToken>
                {
                    sucesso = false,
                    mensagem = "Erro na criação de usuário=> " + ex.Message,
                    dados = null
                });
            }
        }
    }
}
