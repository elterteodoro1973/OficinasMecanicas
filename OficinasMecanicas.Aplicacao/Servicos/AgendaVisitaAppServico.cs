using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using OficinasMecanicas.Aplicacao.DTO.Agenda;
using OficinasMecanicas.Aplicacao.DTO.Oficinas;
using OficinasMecanicas.Aplicacao.Interfaces;
using OficinasMecanicas.Dominio.Entidades;
using OficinasMecanicas.Dominio.Interfaces.Servicos;
using OficinasMecanicas.Dominio.Interfaces;
using OficinasMecanicas.Dominio.Notificacoes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using Newtonsoft.Json;
using OficinasMecanicas.Aplicacao.Model;
using System.Net.Http.Headers;
using System.Net.Http;

namespace OficinasMecanicas.Aplicacao.Servicos
{
    public class AgendaVisitaAppServico : IAgendaVisitaAppServico
    {
        private readonly IAgendamentoVisitaServico _agendamentoVisitaServico;
        private readonly INotificador _notificador;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContext;
        public AgendaVisitaAppServico(IHttpContextAccessor httpContext, IMapper mapper, INotificador notificador,
                                  IAgendamentoVisitaServico agendamentoVisitaServico, IConfiguration configuration)
        {
            _httpContext = httpContext;
            _mapper = mapper;
            _notificador = notificador;
            _agendamentoVisitaServico = agendamentoVisitaServico;
            _configuration = configuration;
        }
        public async Task<IList<AgendamentosVisitasTelaInicialDTO>> ListarAgendamentoVisitasTelaInicial(string? filtro)
        {
            var lista = await GetWebApi("api/bookings");
            var dtos = lista.dados.ToList();

            if (!string.IsNullOrEmpty(filtro) && !string.IsNullOrWhiteSpace(filtro))
            {
                dtos = dtos.Where(c => c.Descricao.ToUpper().Contains(filtro.ToUpper())).ToList();
            }

            var usuario = _httpContext.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == "UsuarioId");

            if (usuario != null && !string.IsNullOrEmpty(usuario.Value))
            {
                dtos = dtos.Where(c => c.IdUsuario.ToString() == usuario.Value).ToList();
            }

            return dtos;
        }
        public async Task<EditarAgendamentoVisitaDTO> Adicionar(CadastrarAgendamentoVisitaDTO dto)
        {
            var agendamentoVisita = _mapper.Map<AgendamentoVisita>(dto);
            var novoUsuario = await _agendamentoVisitaServico.Adicionar(agendamentoVisita);
            return _mapper.Map<EditarAgendamentoVisitaDTO>(agendamentoVisita);
        }
        public async Task<EditarAgendamentoVisitaDTO?> Atualizar(Guid id, CadastrarAgendamentoVisitaDTO? dto)
        {
            if (dto == null || id == null)
            {
                _notificador.Adicionar(new Notificacao("Oficiona inválida !"));
                return null;
            }
            var oficina = _mapper.Map<AgendamentoVisita>(dto);
            oficina.Id = id;
            await _agendamentoVisitaServico.Atualizar(id,oficina);
            return _mapper.Map<EditarAgendamentoVisitaDTO>(oficina);
        }
        public async Task<bool> Excluir(Guid id)
        {
            await _agendamentoVisitaServico.Excluir(id);
            return !_notificador.TemNotificacao();
        }        
        public async Task<EditarAgendamentoVisitaDTO?> BuscarPorId(Guid id)
        {
            var dtos = _mapper.Map<EditarAgendamentoVisitaDTO>(await _agendamentoVisitaServico.BuscarPorId(id));
            return dtos;
        }
        public async Task<IEnumerable<AgendamentosVisitasTelaInicialDTO>> BuscarTodos()
        {  
            var listaAgenda = await _agendamentoVisitaServico.BuscarTodos();
            var agendaResult = listaAgenda.Select(a => new AgendamentosVisitasTelaInicialDTO
            {
                Id = a.Id,
                IdUsuario = a.IdUsuario.Value,
                IdOficina = a.IdOficina.Value,
                NomeUsuario = a.IdUsuarioNavigation?.Username ?? "Usuário não encontrado",
                NomeOficina = a.IdOficinaNavigation?.Nome ?? "Oficina não encontrada",
                DataHora = a.DataHora.Value,
                Descricao = a.Descricao

            }).ToList();

            return agendaResult;
        }

        public async Task<IEnumerable<AgendamentosVisitasTelaInicialDTO>?> BuscarPorDatas(DateTime dtInicio, DateTime dtfinal)
        {
            var dtos = _mapper.Map<IEnumerable<AgendamentosVisitasTelaInicialDTO>>(await _agendamentoVisitaServico.BuscarPorDatas(dtInicio, dtfinal));
            return dtos;
        }
        public async Task<IEnumerable<AgendamentosVisitasTelaInicialDTO>?> BuscarPorDescricao(string descricao)
        {
            var dtos = _mapper.Map<IEnumerable<AgendamentosVisitasTelaInicialDTO>>(await _agendamentoVisitaServico.BuscarPorDescricao(descricao));
            return dtos;
        }

        #region HttpClient
        private void ConfiguraClien(HttpClient client)
        {
            // Configura o cliente HTTP com a URL base e os cabeçalhos necessários
            var enderecoBase = _configuration["baseURL:link"];
            var tokenClaim = _httpContext.HttpContext.User.Claims.First(c => c.Type == "TokenUsuario");

            client.BaseAddress = new Uri(enderecoBase);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("Mozilla", "5.0"));
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + tokenClaim.Value);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenClaim.Value);
        }
        private Resposta<T> RetornoWebErro<T>(string mensagem)
        {
            // Retorna um objeto de resposta com erro
            return new Resposta<T>
            {
                sucesso = false,
                mensagem = mensagem,
                dados = default
            };
        }
        private StringContent ConteudoJson<T>(T model)
        {
            // Corrige o método para retornar o objeto correto
            var conteudoJSON = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
            conteudoJSON.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return conteudoJSON;
        }

        public async Task<Resposta<IList<AgendamentosVisitasTelaInicialDTO>>> GetWebApi(string endPoint)
        {
            try
            {
                //busca a lista de oficinas via API
                using (var clienteAPI = new HttpClient())
                {
                    ConfiguraClien(clienteAPI);
                    var respostaPostAPI = await clienteAPI.GetAsync(endPoint);
                    var respostaconteudo = await respostaPostAPI.Content.ReadAsStringAsync();
                    var respostaObjeto = JsonConvert.DeserializeObject<Resposta<IList<AgendamentosVisitasTelaInicialDTO>>>(respostaconteudo);

                    if (respostaObjeto == null || !respostaObjeto.sucesso)
                        return RetornoWebErro<IList<AgendamentosVisitasTelaInicialDTO>>(respostaObjeto != null ? respostaObjeto.mensagem : "Erro ao processar a resposta da API.");
                    return respostaObjeto;
                }
            }
            catch (Exception ex)
            {
                return RetornoWebErro<IList<AgendamentosVisitasTelaInicialDTO>>("Erro de comunicação ao processar a requisição=>" + ex.Message);
            }
        }

        public async Task<Resposta<EditarAgendamentoVisitaDTO>> GetWebApiById(Guid id, string endPoint)
        {
            try
            {
                //busca o objeto OficinaMecanica via API pelo id
                using (var clienteAPI = new HttpClient())
                {
                    ConfiguraClien(clienteAPI);
                    var respostaPostAPI = await clienteAPI.GetAsync($@"{endPoint}/{id}");
                    var respostaconteudo = await respostaPostAPI.Content.ReadAsStringAsync();
                    var respostaObjeto = JsonConvert.DeserializeObject<Resposta<EditarAgendamentoVisitaDTO>>(respostaconteudo);

                    if (respostaObjeto == null || !respostaObjeto.sucesso)
                        return RetornoWebErro<EditarAgendamentoVisitaDTO>(respostaObjeto != null ? respostaObjeto.mensagem : "Erro ao processar a resposta da API.");
                    return respostaObjeto;
                }
            }
            catch (Exception ex)
            {
                return RetornoWebErro<EditarAgendamentoVisitaDTO>("Erro de comunicação ao processar a requisição=>" + ex.Message);
            }
        }

        public async Task<Resposta<EditarAgendamentoVisitaDTO>> PostWebApi<T1>(T1 model, string endPoint)
        {
            try
            {  //Grava o objeto OficinaMecanica via API
                using (var clienteAPI = new HttpClient())
                {
                    ConfiguraClien(clienteAPI);
                    var respostaPostAPI = await clienteAPI.PostAsync(endPoint, ConteudoJson(model));
                    var respostaconteudo = await respostaPostAPI.Content.ReadAsStringAsync();
                    var respostaObjeto = JsonConvert.DeserializeObject<Resposta<EditarAgendamentoVisitaDTO>>(respostaconteudo);

                    if (respostaObjeto == null || !respostaObjeto.sucesso)
                        return RetornoWebErro<EditarAgendamentoVisitaDTO>(respostaObjeto != null ? respostaObjeto.mensagem : "Erro ao gravar os dados da oficina via API.");
                    return respostaObjeto;
                }
            }
            catch (Exception ex)
            {
                return RetornoWebErro<EditarAgendamentoVisitaDTO>("Erro de comunicação ao processar a requisição=>" + ex.Message);
            }
        }

        public async Task<Resposta<EditarAgendamentoVisitaDTO>> PutWebApi(Guid id, CadastrarAgendamentoVisitaDTO model, string endPoint)
        {
            try
            {
                // Atualiza o objeto OficinaMecanica via API pelo id
                using (var clienteAPI = new HttpClient())
                {
                    ConfiguraClien(clienteAPI);
                    var respostaPostAPI = await clienteAPI.PutAsync($@"{endPoint}/{id}", ConteudoJson(model));
                    var respostaconteudo = await respostaPostAPI.Content.ReadAsStringAsync();
                    var respostaObjeto = JsonConvert.DeserializeObject<Resposta<EditarAgendamentoVisitaDTO>>(respostaconteudo);

                    if (respostaObjeto == null || !respostaObjeto.sucesso)
                        return RetornoWebErro<EditarAgendamentoVisitaDTO>(respostaObjeto != null ? respostaObjeto.mensagem : "Erro ao atualizar os dados da oficina via API.");
                    return respostaObjeto;
                }
            }
            catch (Exception ex)
            {
                return RetornoWebErro<EditarAgendamentoVisitaDTO>("Erro de comunicação ao processar a requisição=>" + ex.Message);
            }
        }

        public async Task<Resposta<EditarAgendamentoVisitaDTO>> DeleteWebApi(Guid id, string endPoint)
        {
            try
            {
                //Deleta o objeto OficinaMecanica via API pelo id
                using (var clienteAPI = new HttpClient())
                {
                    ConfiguraClien(clienteAPI);
                    var respostaPostAPI = await clienteAPI.DeleteAsync($@"{endPoint}/{id}");
                    var respostaconteudo = await respostaPostAPI.Content.ReadAsStringAsync();
                    var respostaObjeto = JsonConvert.DeserializeObject<Resposta<EditarAgendamentoVisitaDTO>>(respostaconteudo);

                    if (respostaObjeto == null || !respostaObjeto.sucesso)
                        return RetornoWebErro<EditarAgendamentoVisitaDTO>(respostaObjeto != null ? respostaObjeto.mensagem : "Erro ao deletar a oficina via API.");
                    return respostaObjeto;
                }
            }
            catch (Exception ex)
            {
                return RetornoWebErro<EditarAgendamentoVisitaDTO>("Erro de comunicação ao processar a requisição=>" + ex.Message);
            }
        }
       
        #endregion
    }
}
