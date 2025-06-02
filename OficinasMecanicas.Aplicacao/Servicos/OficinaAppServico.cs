using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using OficinasMecanicas.Aplicacao.DTO.Oficinas;
using OficinasMecanicas.Aplicacao.DTO.Usuarios;
using OficinasMecanicas.Aplicacao.Interfaces;
using OficinasMecanicas.Aplicacao.Model;
using OficinasMecanicas.Dominio.Entidades;
using OficinasMecanicas.Dominio.Interfaces;
using OficinasMecanicas.Dominio.Interfaces.Servicos;
using OficinasMecanicas.Dominio.Notificacoes;
using System.Net.Http.Headers;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace OficinasMecanicas.Aplicacao.Servicos
{
    public  class OficinaAppServico : IOficinaAppServico
    {
        private readonly IOficinaMecanicaServico _oficinaMecanicaServico;        
        private readonly INotificador _notificador;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContext;
        public OficinaAppServico(IHttpContextAccessor httpContext, IMapper mapper, INotificador notificador, 
                                  IOficinaMecanicaServico oficinaMecanicaServico, IConfiguration configuration)
        {
            _httpContext = httpContext;            
            _mapper = mapper;
            _notificador = notificador;
            _oficinaMecanicaServico = oficinaMecanicaServico;
            _configuration = configuration;
        }
        
        public async Task<IList<OficinasTelaInicialDTO>> ListarOficionasTelaInicial(string? filtro)
        {            
            var lista = await GetWebApi("api/repairshops");
            var dtos = lista.dados.ToList() ;

            if (!string.IsNullOrEmpty(filtro) && !string.IsNullOrWhiteSpace(filtro))
            {
                dtos = dtos.Where(c => c.Nome.ToUpper().Contains(filtro.ToUpper()) || c.Endereco.ToUpper().Contains(filtro.ToUpper()) || c.Servicos.ToUpper().Contains(filtro.ToUpper())).ToList();
            }

            return dtos;
        }

        public async Task<EditarOficinaDTO> Adicionar(CadastrarOficinaDTO dto)
        {
            var oficina = _mapper.Map<OficinaMecanica>(dto);
            var novoUsuario = await _oficinaMecanicaServico.Adicionar(oficina);
            return _mapper.Map<EditarOficinaDTO>(oficina);
        }        

        public async Task<EditarOficinaDTO?> Atualizar(Guid id,  CadastrarOficinaDTO? dto)
        {            
            if (dto == null || id == null )
            {
                _notificador.Adicionar(new Notificacao("Oficina inválida !"));
                return null;
            }
            
            var oficinaEnvio = await _oficinaMecanicaServico.BuscarPorId(id);
            if (oficinaEnvio == null )
            {
                _notificador.Adicionar(new Notificacao("Oficina não encontrada !"));
                return null;
            }

            var oficina = _mapper.Map<OficinaMecanica>(dto);
            oficina.Id = id;
            await _oficinaMecanicaServico.Atualizar(id,oficina);
            return _mapper.Map<EditarOficinaDTO>(oficina);
        }


        public async Task<bool> Excluir(Guid id)
        {
            await _oficinaMecanicaServico.Excluir(id);
            return !_notificador.TemNotificacao();
        }

        public async Task<EditarOficinaDTO?> BuscarPorId(Guid id)
        {
            var dtos = _mapper.Map<EditarOficinaDTO>(await _oficinaMecanicaServico.BuscarPorId(id));
            return dtos;
        }

        
        public async Task<EditarOficinaDTO?> BuscarPorNome(string nome)
        {
            var dtos = _mapper.Map<EditarOficinaDTO>(await _oficinaMecanicaServico.BuscarPorNome(nome));
            return dtos;
        }

        public async Task<IEnumerable<OficinasTelaInicialDTO>> BuscarTodos()
        {
            var dtos = _mapper.Map<IList<OficinasTelaInicialDTO>>(await _oficinaMecanicaServico.BuscarTodos());         

            return dtos;
        }

        public async Task<bool> NomePrincipalJaCadastrado(string nome, Guid? id)
        {
            return await _oficinaMecanicaServico.NomePrincipalJaCadastrado(nome, id);
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
        public async Task<Resposta<IList<OficinasTelaInicialDTO>>> GetWebApi(string endPoint)
        {
            try
            {
                //busca a lista de oficinas via API
                using (var clienteAPI = new HttpClient())
                {
                    ConfiguraClien(clienteAPI);
                    var respostaPostAPI = await clienteAPI.GetAsync(endPoint);
                    var respostaconteudo = await respostaPostAPI.Content.ReadAsStringAsync();
                    var respostaObjeto = JsonConvert.DeserializeObject<Resposta<IList<OficinasTelaInicialDTO>>>(respostaconteudo);

                    if (respostaObjeto == null || !respostaObjeto.sucesso)
                        return RetornoWebErro<IList<OficinasTelaInicialDTO>>(respostaObjeto != null ? respostaObjeto.mensagem : "Erro ao processar a resposta da API.");                    
                    return respostaObjeto;
                }
            }
            catch (Exception ex)
            {                
               return RetornoWebErro<IList<OficinasTelaInicialDTO>>("Erro de comunicação ao processar a requisição=>" + ex.Message);
            }
        }

        public async Task<Resposta<EditarOficinaDTO>> GetWebApiById(Guid id, string endPoint)
        {
            try
            {
                //busca o objeto OficinaMecanica via API pelo id
                using (var clienteAPI = new HttpClient())
                {
                    ConfiguraClien(clienteAPI);
                    var respostaPostAPI = await clienteAPI.GetAsync($@"{endPoint}/{id}");
                    var respostaconteudo = await respostaPostAPI.Content.ReadAsStringAsync();
                    var respostaObjeto = JsonConvert.DeserializeObject<Resposta<EditarOficinaDTO>>(respostaconteudo);

                    if (respostaObjeto == null || !respostaObjeto.sucesso)                                           
                        return RetornoWebErro<EditarOficinaDTO>(respostaObjeto != null ? respostaObjeto.mensagem : "Erro ao processar a resposta da API.");
                    return respostaObjeto;
                }
            }
            catch (Exception ex)
            {
                return RetornoWebErro<EditarOficinaDTO>("Erro de comunicação ao processar a requisição=>" + ex.Message);                
            }
        }

        public async Task<Resposta<EditarOficinaDTO>> PostWebApi<T1>(T1 model, string endPoint)
        {
            
            try
            {  //Grava o objeto OficinaMecanica via API
                using (var clienteAPI = new HttpClient())
                {
                    ConfiguraClien(clienteAPI);       
                    var respostaPostAPI = await clienteAPI.PostAsync(endPoint, ConteudoJson(model));
                    var respostaconteudo = await respostaPostAPI.Content.ReadAsStringAsync();
                    var respostaObjeto = JsonConvert.DeserializeObject<Resposta<EditarOficinaDTO>>(respostaconteudo);
                    if (respostaObjeto == null || !respostaObjeto.sucesso)                                            
                        return RetornoWebErro<EditarOficinaDTO>(respostaObjeto != null ? respostaObjeto.mensagem : "Erro ao gravar os dados da oficina via API.");

                    return respostaObjeto;
                }
            }
            catch (Exception ex)
            {               
                return RetornoWebErro<EditarOficinaDTO>("Erro de comunicação ao processar a requisição=>" + ex.Message);
            }
        }

        public async Task<Resposta<EditarOficinaDTO>> PutWebApi(Guid id,CadastrarOficinaDTO model,string endPoint)
        {
            try
            {
                //Atualiza o objeto OficinaMecanica via API pelo id
                using (var clienteAPI = new HttpClient())
                {
                    ConfiguraClien(clienteAPI);
                    var respostaPostAPI = await clienteAPI.PutAsync($@"{endPoint}/{id}", ConteudoJson(model));
                    var respostaconteudo = await respostaPostAPI.Content.ReadAsStringAsync();
                    var respostaObjeto = JsonConvert.DeserializeObject<Resposta<EditarOficinaDTO>>(respostaconteudo);

                    if (respostaObjeto == null || !respostaObjeto.sucesso)
                        return RetornoWebErro<EditarOficinaDTO>(respostaObjeto != null ? respostaObjeto.mensagem : "Erro ao atualizar o dados da oficina via API.");
                    return respostaObjeto;
                }
            }
            catch (Exception ex)
            {
                return RetornoWebErro<EditarOficinaDTO>("Erro de comunicação ao processar a requisição=>" + ex.Message);
            }
        }

        public async Task<Resposta<EditarOficinaDTO>> DeleteWebApi(Guid id, string endPoint)
        {
            try
            {
                //Deleta o objeto OficinaMecanica via API pelo id
                using (var clienteAPI = new HttpClient())
                {
                    ConfiguraClien(clienteAPI);
                    var respostaPostAPI = await clienteAPI.DeleteAsync($@"{endPoint}/{id}");
                    var respostaconteudo = await respostaPostAPI.Content.ReadAsStringAsync();
                    var respostaObjeto = JsonConvert.DeserializeObject<Resposta<EditarOficinaDTO>>(respostaconteudo);

                    if (respostaObjeto == null || !respostaObjeto.sucesso)                    
                        return RetornoWebErro<EditarOficinaDTO>(respostaObjeto != null ? respostaObjeto.mensagem : "Erro ao deletar a oficina via API.");
                    return respostaObjeto;
                }
            }
            catch (Exception ex)
            {
                return RetornoWebErro<EditarOficinaDTO>("Erro de comunicação ao processar a requisição:" + ex.Message);
            }
        }
        #endregion

    }
}
