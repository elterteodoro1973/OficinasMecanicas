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
            var lista = await RequisitarDados("api/repairshops");
            var dtos = lista.dados.ToList() ;

            if (!string.IsNullOrEmpty(filtro) && !string.IsNullOrWhiteSpace(filtro))
            {
                dtos = dtos.Where(c => c.Nome.ToUpper().Contains(filtro.ToUpper()) || c.Endereco.ToUpper().Contains(filtro.ToUpper()) || c.Servicos.ToUpper().Contains(filtro.ToUpper())).ToList();
            }

            return dtos;
        }

        public async Task<Resposta<IList<OficinasTelaInicialDTO>>> RequisitarDados(string endPoint)
        {
            try
            {
                using (var clienteAPI = new HttpClient())
                {
                    ConfiguraClien(clienteAPI);
                    var respostaPostAPI = await clienteAPI.GetAsync(endPoint);
                    var respostaconteudo = await respostaPostAPI.Content.ReadAsStringAsync();
                    var respostaObjeto = JsonConvert.DeserializeObject<Resposta<IList<OficinasTelaInicialDTO>>>(respostaconteudo);

                    if (respostaObjeto == null || respostaObjeto.dados == null)
                    {
                        return new Resposta<IList<OficinasTelaInicialDTO>>
                        {
                            sucesso = false,
                            mensagem = "Erro ao processar a resposta da API.",
                            dados = default
                        };
                    }

                    return respostaObjeto;
                }
            }
            catch (Exception ex)
            {
                return new Resposta<IList<OficinasTelaInicialDTO>>
                {
                    sucesso = false,
                    mensagem = "Erro de comunicação ao processar a requisição:" + ex.Message,
                    dados = default
                };
            }
        }




        private void ConfiguraClien(HttpClient client)
        {
            var enderecoBase = _configuration["baseURL:link"];
            var tokenClaim = _httpContext.HttpContext.User.Claims.First(c => c.Type == "TokenUsuario");

            client.BaseAddress = new Uri(enderecoBase);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("Mozilla", "5.0"));
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + tokenClaim.Value);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenClaim.Value);
        }

        public async Task<Resposta<UserToken>> PostarRequisicao<T1>(T1 model, string endPoint)
        { 
            try
            {
                using (var clienteAPI = new HttpClient())
                {
                    ConfiguraClien(clienteAPI);
                    var conteudoJSON = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
                    conteudoJSON.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                    var respostaPostAPI = await clienteAPI.PostAsync(endPoint, conteudoJSON);
                    var respostaconteudo = await respostaPostAPI.Content.ReadAsStringAsync();
                    var respostaObjeto = JsonConvert.DeserializeObject<Resposta<UserToken>>(respostaconteudo);

                    if (respostaObjeto == null)
                    {
                        return new Resposta<UserToken>
                        {
                            sucesso = false,
                            mensagem = "Erro ao processar a resposta da API.",
                            dados = default
                        };
                    }

                    return respostaObjeto;
                }
            }
            catch (Exception ex)
            {
                return new Resposta<UserToken>
                {
                    sucesso = false,
                    mensagem = "Erro de comunicação ao processar a requisição:" + ex.Message,
                    dados = default
                };
            }
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
    }
}
