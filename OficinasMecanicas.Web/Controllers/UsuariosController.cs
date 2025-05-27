using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OficinasMecanicas.Aplicacao.DTO.Usuarios;
using OficinasMecanicas.Aplicacao.Interfaces;
using OficinasMecanicas.Dominio.Interfaces;
using OficinasMecanicas.Dominio.Interfaces.Servicos;
using OficinasMecanicas.Web.Configuracoes.Claims;
using OficinasMecanicas.Web.ViewModels.Usuarios;

namespace OficinasMecanicas.Web.Controllers
{
    [Authorize]
    public class UsuariosController : BaseController
    {
        private readonly IUsuarioAppServico _usuarioAppServico;
        private readonly IResetarSenhaServico _resetarSenhaServico;
        private readonly INotificador _notificador;
        private readonly ILogger<UsuariosController> _logger;

        private readonly IMapper _mapper; 
        private readonly IWebHostEnvironment _env;

        public UsuariosController(ILogger<UsuariosController> logger, IUsuarioAppServico usuarioAppServico, IResetarSenhaServico resetarSenhaServico,
            IMapper mapper, INotificador notificador,         
            IWebHostEnvironment env ) : base(notificador)
        {
            _logger = logger;
            _usuarioAppServico = usuarioAppServico;
            _resetarSenhaServico = resetarSenhaServico;
            _notificador = notificador;
            _mapper = mapper;
            _env = env;
        }

        public async Task<IActionResult> Index(string? filtro, string? sort)
        {
            if (HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                var usuarios = await _usuarioAppServico.ListarUsuariosTelaInicial(filtro);
                var model = _mapper.Map<List<UsuariosViewModel>>(usuarios.AsEnumerable());
                return PartialView("Grid", model.AsEnumerable());
            }

            return View();
        }

        [AllowAnonymous]
        public async Task<IActionResult> Login()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                await _usuarioAppServico.Logout();
                HttpContext.Session.Clear();
            }

            return View();
        }

        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                await _usuarioAppServico.Logout();
            }

            return View(nameof(Login));
        }

        [AllowAnonymous]
        [HttpPost]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> Login([Bind("Usuario, Senha")] LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            await _usuarioAppServico.Login(_env.WebRootPath, model.Usuario, model.Senha);

            if (!OperacaoValida())
                return View(model);

            return RedirectToAction("Index", "Home");

        }

        [AllowAnonymous]
        public async Task<IActionResult> CadastrarNovaSenha(string? token)
        {
            if ( token == string.Empty )
                return BadRequest();

            await _usuarioAppServico.ValidarTokenCadastrarNovaSenha(token);

            if (!OperacaoValida())
                return View();

            var resetarSenha = await _resetarSenhaServico.BuscarResetarSenhaPorToken(token);
            var usuario = await _usuarioAppServico.BuscarUsuarioPorId(resetarSenha.UsuarioId);

            var model = new CadastrarNovaSenhaViewModel { Email = usuario.Email, Token = token };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> CadastrarNovaSenha(string? token, [Bind("Email, Token, Senha, ConfirmarSenha")] CadastrarNovaSenhaViewModel model)
        {
            if (token == string.Empty || token != model.Token)
                return BadRequest();

            if (!ModelState.IsValid)
                return View(model);

            await _usuarioAppServico.CadastrarNovaSenha(_mapper.Map<CadastrarNovaSenhaDTO>(model));

            if (!OperacaoValida())
                return View(model);

            TempData["Sucesso"] = "Senha cadastrada com sucesso !";

            await _usuarioAppServico.Login(_env.WebRootPath, model.Email, model.Senha);

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Adicionar()
        {
            
            return View(new CadastrarEditarUsuarioViewModel() {Id = Guid.NewGuid() });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Adicionar([Bind("Nome, Email")] CadastrarEditarUsuarioViewModel model)
        {
            

            if (!ModelState.IsValid)
                return View(model);

            var usuario = _mapper.Map<CadastrarEditarUsuarioDTO>(model);

            await _usuarioAppServico.Cadastrar(_env.WebRootPath, usuario);

            if (!OperacaoValida())
                return View(model);

            TempData["Sucesso"] = "Usuário cadastrado com sucesso !";
            return RedirectToAction(nameof(Index));
        }       

        public async Task<IActionResult> Editar(Guid id)
        {
            

            var usuario = await _usuarioAppServico.BuscarUsuarioParaEditarPorId(id);           

            if (usuario == null)
                return BadRequest();            

            var model = _mapper.Map<CadastrarEditarUsuarioViewModel>(usuario);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(Guid id, [Bind("Id, Nome,  Email")] CadastrarEditarUsuarioViewModel model)
        {
            

            if (id != model.Id)
                ModelState.AddModelError("", "Usuário Inválido !");

            if (!ModelState.IsValid)
                return View(model);

            var usuario = _mapper.Map<CadastrarEditarUsuarioDTO>(model);

            await _usuarioAppServico.Editar(usuario);

            if (!OperacaoValida())
                return View(model);

            TempData["Sucesso"] = "Usuário atualizado com sucesso !";
            return RedirectToAction(nameof(Index));
        }

        

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Excluir(Guid id)
        {
            var usuario = await _usuarioAppServico.BuscarUsuarioParaEditarPorId(id);

            if (usuario == null)
                return BadRequest("Usuário não encontrado !");

            await _usuarioAppServico.Excluir(id);

            if (!OperacaoValida())
            {
                var lista = new List<string>();
                foreach (var item in _notificador.ObterNotificacoes())
                {
                    ModelState.AddModelError(string.Empty, item.Mensagem);
                    lista.Add(item.Mensagem);
                }
                return BadRequest(lista);
            }

            return Ok("Usuário Excluido com sucesso !");
        }
        public async Task<IActionResult> Permissoes(Guid id)
        {
            if (id == Guid.Empty)
                return NotFound();

            var usuario = await _usuarioAppServico.BuscarUsuarioPorId(id);

            if (usuario == null)
                return BadRequest();

            var claims = ClaimsUtils.RecuperarListaTuplasModulosClaims();
            ViewBag.Categorias = claims.GroupBy(c => c.Item1).Select(g => g.First()).Select(c => c.Item1).OrderBy(c => c).ToList();
            ViewBag.ClaimModulos = claims;

             
            
            Guid? perfilId = null;
            //if (usuario.Administrador)
            //{
            //    var perfil = _mapper.Map<PerfilViewModel>(await _perfilAppServico.BuscarPerfilAdministrador());
            //    perfilId = perfil.Id;
            //}
            
            

            var model = new CadastrarPerfilUsuarioViewModel
            {
                NomeUsuario = usuario.Nome,
                Email = usuario.Email,
                UsuarioId = id,
                PerfilId = perfilId,
               

            };

            return View(model);
        }
        
       
        [AllowAnonymous]
        public IActionResult EsqueciSenha()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> EsqueciSenha([Bind("Email")] EsqueciSenhaViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            await _usuarioAppServico.ResetarSenha(_env.WebRootPath, model.Email);

            if (!OperacaoValida())
                return View(model);


            ViewBag.ExibirMensagemSucesso = true;
            return View();
        }

        public async Task<IActionResult> BuscarPermissoesUsuario(Guid usuarioId)
        {
            if (usuarioId == Guid.Empty)
                return BadRequest("Identificador do usuário inválido !");
            
            
            try
            {
                //var dto = await _usuarioAppServico.BuscarPermissoesUsuarioCBHPorId(cbhId, usuarioId);

                //if (dto == null)
                //    return NotFound();

                //var model = new BuscarPerfilEPermissoesUsuarioViewModel
                //{
                //    PerfilId = dto.PerfilId
                //};
                var claims = ClaimsUtils.RecuperarListaTuplasModulosClaims();
                //model.Permissoes = claims.IntersectBy(dto.Permissoes.Select(c => new { c.Type, c.Value }), c => new { c.Item4.Type, c.Item4.Value }).Select(c => c.Item3).ToArray();

                //return Ok(model);
                return Ok(null);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro ao buscar as permissões do usuário !" + ex.Message);
            }
        }

        public async Task<IActionResult> TrocarUsuarioLogado()
        {
            if (!BuscarUsuarioIdLogado().HasValue)
                return BadRequest();

            await _usuarioAppServico.TrocarUsuarioLogado(BuscarUsuarioIdLogado().Value);

            if (OperacaoValida())
                TempData["Sucesso"] = "selecionado com sucesso !";

            return RedirectToAction("Index", "Home");
        }
    }
}
