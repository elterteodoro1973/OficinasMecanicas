using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OficinasMecanicas.Aplicacao.DTO.Oficinas;
using OficinasMecanicas.Aplicacao.Interfaces;
using OficinasMecanicas.Dominio.Interfaces;
using OficinasMecanicas.Dominio.Interfaces.Servicos;
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
        private readonly IServicosPrestadosServico _servicosPrestadosServico;

        public OficinaMecanicasController(ILogger<OficinaMecanicasController> logger,
            IMapper mapper, INotificador notificador,
            IWebHostEnvironment env, IConfiguration configuration, IOficinaAppServico oficinaAppServico, IServicosPrestadosServico servicosPrestadosServico)
        {
            _logger = logger;
            _notificador = notificador;
            _mapper = mapper;
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _oficinaAppServico = oficinaAppServico;
            _servicosPrestadosServico = servicosPrestadosServico;
        }

        private async Task SetViewBagServicosPrestados()
        {
            var lista = _servicosPrestadosServico.BuscarTodos().Result;
            ViewBag.listaServico = lista.ToList();
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
            SetViewBagServicosPrestados();
            ViewBag.MensagemErro = string.Empty;
            return await Task.FromResult(View(new CadastrarEditarOficinaViewModel() { Id = Guid.NewGuid() }));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Adicionar(CadastrarEditarOficinaViewModel model)
        {
            SetViewBagServicosPrestados();
            ViewBag.MensagemErro = String.Empty;
            if (!ModelState.IsValid)
                return View(model);

            var oficinaDB = _mapper.Map<CadastrarOficinaDTO>(model);
            var respostaObjeto = await _oficinaAppServico.PostWebApi<CadastrarOficinaDTO>(oficinaDB, "api/repairshops");
            if (!respostaObjeto.sucesso)
            {
                ViewBag.MensagemErro = respostaObjeto.mensagem;
                return View(model);
            }            
            
            TempData["Sucesso"] = "Oficina adicionada com sucesso!";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Editar(Guid id)
        {
            SetViewBagServicosPrestados();
            var dtos = await _oficinaAppServico.GetWebApiById(id, $"api/repairshops");
            ViewBag.MensagemErro = string.Empty;
            if (dtos.dados == null)
            {
                ViewBag.MensagemErro = dtos.mensagem;
                return View(new CadastrarEditarOficinaViewModel() { Id = id } );
            }
            var model = _mapper.Map<CadastrarEditarOficinaViewModel>(dtos.dados );
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(Guid id, CadastrarEditarOficinaViewModel model)
        {
            SetViewBagServicosPrestados();
            ViewBag.MensagemErro = string.Empty;
            if (id != model.Id)
                ModelState.AddModelError("", "Oficina inválida!");

            if (!ModelState.IsValid)
                return View(model);
            
            var oficina = _mapper.Map<CadastrarOficinaDTO>(model);
            //await _oficinaAppServico.Atualizar(id, oficina);

            var respostaObjeto = await _oficinaAppServico.PutWebApi(id, oficina, $"api/repairshops");
            if (respostaObjeto.sucesso) 
              TempData["Sucesso"] = "Oficina atualizada com sucesso!";

            return RedirectToAction(nameof(Index));
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Excluir(Guid id)
        {
            SetViewBagServicosPrestados();

            var oficina = await _oficinaAppServico.GetWebApiById(id, $"api/repairshops");
            if (oficina == null)
                return BadRequest("Oficina não encontrada!");

            var respostaObjeto = await _oficinaAppServico.DeleteWebApi(id, $"api/repairshops");

            TempData["Sucesso"] = "Oficina excluída com sucesso !";
            return Ok("Oficina excluída com sucesso !");
        }
    }
}
