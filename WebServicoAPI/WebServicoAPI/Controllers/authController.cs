using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OficinasMecanicas.Aplicacao.DTO.Usuarios;
using OficinasMecanicas.Aplicacao.Interfaces;
using OficinasMecanicas.Dominio.Entidades;
using WebServicoAPI.JWT;

namespace WebServicoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class authController : ControllerBase
    {
        private readonly IUsuarioAppServico _usuarioAppServico;
        private readonly ILogger<authController> _logger;
        private readonly IJWToken _jwToken; // Add dependency for IJWToken

        public authController(ILogger<authController> logger, IUsuarioAppServico usuarioAppServico, IJWToken jwToken)
        {
            _logger = logger;
            _usuarioAppServico = usuarioAppServico;
            _jwToken = jwToken; // Initialize the dependency
        }

        [HttpPost(Name = "login")]
        public async Task<ActionResult<UserToken>> Login(LogarDTO loginDTO)
        {
            var usuario = await _usuarioAppServico.BuscarPorEmail(loginDTO.Email);
            if (usuario == null)
                return Unauthorized("Email não cadastrado.");

            // Generate token using the _jwToken dependency
            var token = _jwToken.GenerateToken(usuario.Id, usuario.Email);

            return new UserToken { Token = token };
        }
    }
}
