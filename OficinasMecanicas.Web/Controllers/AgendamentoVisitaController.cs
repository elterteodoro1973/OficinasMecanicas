﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OficinasMecanicas.Aplicacao.DTO.Agenda;
using OficinasMecanicas.Aplicacao.Interfaces;
using OficinasMecanicas.Dominio.Interfaces.Servicos;
using OficinasMecanicas.Dominio.Interfaces;
using OficinasMecanicas.Web.ViewModels.Agenda;
using OficinasMecanicas.Aplicacao.Servicos;
using OficinasMecanicas.Web.ViewModels.Usuarios;
using OficinasMecanicas.Web.ViewModels.Oficinas;
using Irony.Parsing;

namespace OficinasMecanicas.Web.Controllers
{
    public class AgendamentoVisitaController : Controller
    {
        private readonly INotificador _notificador;
        private readonly ILogger<AgendamentoVisitaController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IAgendaVisitaAppServico _agendaVisitaAppServico;
        private readonly IUsuarioAppServico _usuarioAppServico;
        private readonly IOficinaAppServico _oficinaAppServico;
        private readonly IServicosPrestadosServico _servicosPrestadosServico;
        private readonly IHttpContextAccessor _httpContext;
        

        public AgendamentoVisitaController(ILogger<AgendamentoVisitaController> logger, IHttpContextAccessor httpContext,
            IMapper mapper, INotificador notificador,
            IWebHostEnvironment env, IConfiguration configuration,
            IAgendaVisitaAppServico agendaVisitaAppServico,
            IOficinaAppServico oficinaAppServico,
            IUsuarioAppServico usuarioAppServico,

            IServicosPrestadosServico servicosPrestadosServico)
        {
            _logger = logger;
            _notificador = notificador;
            _mapper = mapper;
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _usuarioAppServico = usuarioAppServico;
            _oficinaAppServico = oficinaAppServico;
            _agendaVisitaAppServico = agendaVisitaAppServico;
            _httpContext = httpContext;
            _servicosPrestadosServico = servicosPrestadosServico;
        }

        private async Task SetViewBagOficinas()
        {
            var listaOficinas =  _oficinaAppServico.GetWebApi("api/repairshops").Result;
            ViewBag.listaOficinas = listaOficinas.dados;
        }

        public async Task<IActionResult> Index(string? filtro, string? sort)
        {
            if (HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                var agendas = await _agendaVisitaAppServico.ListarAgendamentoVisitasTelaInicial(filtro);                
                return PartialView("Grid", agendas.AsEnumerable());
            }

            return View();
        }

        public async Task<IActionResult> Adicionar()
        {
            SetViewBagOficinas();

            var usuario = _httpContext.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == "UsuarioId");

            var registro = new CadastrarEditarAgendamentoVisitaViewModel() { Id = Guid.NewGuid() , DataHora = DateTime.Now.AddDays(1).Date };

            if (usuario != null && !string.IsNullOrEmpty(usuario.Value))
            {
                registro.IdUsuario  = usuario.Value != null ? Guid.Parse(usuario.Value) : Guid.Empty;
            }

            ViewBag.MensagemErro = string.Empty;
            return await Task.FromResult(View(registro));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Adicionar(CadastrarEditarAgendamentoVisitaViewModel model)
        {
            SetViewBagOficinas();
            ViewBag.MensagemErro = String.Empty;
            
            var agendamentoDB = _mapper.Map<CadastrarAgendamentoVisitaDTO>(model);
            var respostaObjeto = await _agendaVisitaAppServico.PostWebApi<CadastrarAgendamentoVisitaDTO>(agendamentoDB, "api/bookings");
            if (!respostaObjeto.sucesso)
            {
                ViewBag.MensagemErro = respostaObjeto.mensagem;
                return View(model);
            }

            TempData["Sucesso"] = "Agendamento adicionado com sucesso!";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Editar(Guid id)
        {
            SetViewBagOficinas();
            var dtos = await _agendaVisitaAppServico.GetWebApiById(id, $"api/bookings");
            
            if (dtos.dados == null)
            {
                ViewBag.MensagemErro = dtos.mensagem;
                return View(new CadastrarEditarAgendamentoVisitaViewModel() { Id = id });
            }

            var model = _mapper.Map<CadastrarEditarAgendamentoVisitaViewModel>(dtos.dados);
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(Guid id, CadastrarEditarAgendamentoVisitaViewModel model)
        {
            SetViewBagOficinas();
            if (id != model.Id)
                ModelState.AddModelError("", "Agendamento inválido!");
            
            var agendamento = _mapper.Map<CadastrarAgendamentoVisitaDTO>(model);
            

            var respostaObjeto = await _agendaVisitaAppServico.PutWebApi(id, agendamento, $"api/bookings");
            if (respostaObjeto.sucesso)
                TempData["Sucesso"] = "AgendamentoVisita atualizada com sucesso!";
            
            return RedirectToAction("Index", "AgendamentoVisita");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Excluir(Guid id)
        {
            SetViewBagOficinas(); 

            var agendamento = await _agendaVisitaAppServico.GetWebApiById(id, $"api/bookings");
            if (agendamento == null)               
               return BadRequest("Agendamento não encontrado!");

            var respostaObjeto = await _agendaVisitaAppServico.DeleteWebApi(id, $"api/bookings");

            TempData["Sucesso"] = "AgendamentoVisita excluída com sucesso!";
            return Ok("AgendamentoVisita excluída com sucesso !");
        }
    }
}
