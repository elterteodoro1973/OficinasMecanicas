using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OficinasMecanicas.Dominio.Interfaces.Servicos;
using OficinasMecanicas.Dominio.Interfaces;
using OficinasMecanicas.Aplicacao.Interfaces;
using OficinasMecanicas.Web.ViewModels.Usuarios;
using OficinasMecanicas.Web.ViewModels.Oficinas;

namespace OficinasMecanicas.Web.Controllers
{
    public class OficinaMecanicasController : Controller
    {
        private readonly INotificador _notificador;
        private readonly ILogger<OficinaMecanicasController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;        
        private readonly IOficinaAppServico _oficinaAppServico;

        public OficinaMecanicasController(ILogger<OficinaMecanicasController> logger,            
            IMapper mapper, INotificador notificador,
            IWebHostEnvironment env, IConfiguration configuration, IOficinaAppServico oficinaAppServico)
        {
            _logger = logger;
            _notificador = notificador;
            _mapper = mapper;
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _oficinaAppServico = oficinaAppServico;
        }

        public async Task<IActionResult> Index(string? filtro, string? sort)
        {
            if (HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                var oficinas = await _oficinaAppServico.ListarOficionasTelaInicial(filtro);
                var model = _mapper.Map<List<OficinasMecanicasViewModel>>(oficinas.AsEnumerable());
                return PartialView("Grid", model.AsEnumerable());
            }

            return View();
        }

        public async Task<IActionResult> Adicionar()
        {
            ViewBag.MensagemErro = String.Empty;
            return View(new CadastrarEditarOficinaViewModel() { Id = Guid.NewGuid() });
            //return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Adicionar([Bind("Nome, Email,Senha")] CadastrarEditarOficinaViewModel model)
        {
            //ViewBag.MensagemErro = String.Empty;

            //if (!ModelState.IsValid)
            //    return View(model);

            //var usuario = _mapper.Map<CadastrarOficinaMecanicaDTO>(model);

            //var respostaObjeto = await _usuarioAppServico.PostarRequisicao<CadastrarOficinaMecanicaDTO>(usuario, "api/auth/register");

            //if (!respostaObjeto.sucesso)
            //{
            //    ViewBag.MensagemErro = respostaObjeto.mensagem;
            //    return View(model);
            //}

            //if (!OperacaoValida())
            //    return View(model);

            //TempData["Sucesso"] = "Usuário cadastrado com sucesso !";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Editar(Guid id)
        {
            //var usuario = await _usuarioAppServico.BuscarOficinaMecanicaParaEditarPorId(id);

            //if (usuario == null)
            //    return BadRequest();

            //var model = _mapper.Map<CadastrarEditarOficinaMecanicaViewModel>(usuario);
            //return View(model);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(Guid id, [Bind("Id, Nome,  Email")] CadastrarEditarOficinaViewModel model)
        {
            //if (id != model.Id)
            //    ModelState.AddModelError("", "Usuário Inválido !");

            //if (!ModelState.IsValid)
            //    return View(model);

            //var usuario = _mapper.Map<EditarOficinaMecanicaDTO>(model);

            //await _usuarioAppServico.Atualizar(usuario);

            //if (!OperacaoValida())
            //    return View(model);

            TempData["Sucesso"] = "Usuário atualizado com sucesso !";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Excluir(Guid id)
        {
            //var usuario = await _usuarioAppServico.BuscarOficinaMecanicaParaEditarPorId(id);

            //if (usuario == null)
            //    return BadRequest("Usuário não encontrado !");

            //await _usuarioAppServico.Excluir(id);

            //if (!OperacaoValida())
            //{
            //    var lista = new List<string>();
            //    foreach (var item in _notificador.ObterNotificacoes())
            //    {
            //        ModelState.AddModelError(string.Empty, item.Mensagem);
            //        lista.Add(item.Mensagem);
            //    }
            //    return BadRequest(lista);
            //}

            return Ok("Usuário Excluido com sucesso !");
        }
    }
}
