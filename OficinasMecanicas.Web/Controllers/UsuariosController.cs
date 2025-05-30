using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OficinasMecanicas.Aplicacao.DTO.Usuarios;
using OficinasMecanicas.Aplicacao.Interfaces;
using OficinasMecanicas.Dominio.Interfaces;
using OficinasMecanicas.Dominio.Interfaces.Servicos;
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
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper; 
        private readonly IWebHostEnvironment _env;
        
        public UsuariosController( ILogger<UsuariosController> logger, IUsuarioAppServico usuarioAppServico, IResetarSenhaServico resetarSenhaServico,
            IMapper mapper, INotificador notificador,         
            IWebHostEnvironment env, IConfiguration configuration) : base(notificador)
        {
            
            _logger = logger;
            _usuarioAppServico = usuarioAppServico;
            _resetarSenhaServico = resetarSenhaServico;
            _notificador = notificador;
            _mapper = mapper;
            _env = env;
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
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
            ViewBag.MensagemErro = string.Empty;
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                await _usuarioAppServico.Logout();
                HttpContext.Session.Clear();
            }

            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            ViewBag.MensagemErro = string.Empty;

            if (!ModelState.IsValid)
                return View(model);

            if (!OperacaoValida())
                return View(model);
            
            var respostaObjeto = await _usuarioAppServico.PostWebApi<LoginViewModel>(model,"api/auth/login");

            if (!respostaObjeto.sucesso)
            {
                ViewBag.MensagemErro = respostaObjeto.mensagem;
                return View(model);
            }
            await _usuarioAppServico.RegistrarLogin(respostaObjeto);

            return RedirectToAction("Index", "Home");
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

            //await _usuarioAppServico.Login(_env.WebRootPath, model.Email, model.Senha);

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Adicionar()
        {
            ViewBag.MensagemErro = String.Empty;
            return View(new CadastrarEditarUsuarioViewModel() {Id = Guid.NewGuid() });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Adicionar([Bind("Nome, Email,Senha")] CadastrarEditarUsuarioViewModel model)
        {
            ViewBag.MensagemErro = String.Empty;
            if (!ModelState.IsValid)
                return View(model);

            var usuario = _mapper.Map<CadastrarUsuarioDTO>(model);
            var respostaObjeto = await _usuarioAppServico.PostWebApi<CadastrarUsuarioDTO>(usuario, "api/auth/register");
            if (!respostaObjeto.sucesso)
            {
                ViewBag.MensagemErro = respostaObjeto.mensagem;
                return View(model);
            }

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

            var usuario = _mapper.Map<EditarUsuarioDTO>(model);

            await _usuarioAppServico.Atualizar(usuario);

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
        
        
    }
}
