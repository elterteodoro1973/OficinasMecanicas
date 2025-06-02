using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using OficinasMecanicas.Aplicacao.DTO.Usuarios;
using OficinasMecanicas.Aplicacao.Interfaces;
using OficinasMecanicas.Aplicacao.Model;
using OficinasMecanicas.Dominio.Entidades;
using OficinasMecanicas.Dominio.Interfaces;
using OficinasMecanicas.Dominio.Interfaces.Repositorios;
using OficinasMecanicas.Dominio.Interfaces.Servicos;
using OficinasMecanicas.Dominio.Notificacoes;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;

namespace OficinasMecanicas.Aplicacao.Servicos
{
    public class UsuarioAppServico : IUsuarioAppServico
    {
        private readonly IUsuariosServico _usuarioServico;
        private readonly IUsuarioRepositorio _usuarioRepositorio;        
        private readonly INotificador _notificador;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContext;
        
        public UsuarioAppServico(IHttpContextAccessor httpContext,IMapper mapper,INotificador notificador,
            IUsuarioRepositorio usuarioRepositorio,IUsuariosServico usuarioServico, IConfiguration configuration)
        {
            _httpContext = httpContext;
            _usuarioRepositorio = usuarioRepositorio;
            _mapper = mapper;
            _notificador = notificador;
            _usuarioServico = usuarioServico;
            _configuration = configuration;            
        }

        public async Task<EditarUsuarioDTO> Adicionar(string caminho, CadastrarUsuarioDTO dto)
        {
            var usuario = _mapper.Map<Usuarios>(dto);
            usuario.PasswordHash = _usuarioServico.GetSha256Hash(dto.Senha);
            var  novoUsuario =  await _usuarioServico.Adicionar(caminho, usuario);
            return _mapper.Map<EditarUsuarioDTO>(novoUsuario);
        }

        public async Task<IList<UsuariosTelaInicialDTO>> ListarUsuariosTelaInicial(string? filtro)
        {
            var dtos =  await GetWebApi("api/user");
            var listaDados = dtos.dados;

            if (!string.IsNullOrEmpty(filtro) && !string.IsNullOrWhiteSpace(filtro))
            {
                listaDados = listaDados.Where(c => c.Nome.ToUpper().Contains(filtro.ToUpper()) || c.Email.ToUpper().Contains(filtro.ToUpper())).ToList();
            }

            return listaDados;
        }

        public async Task<IList<Usuarios>> BuscarTodos()
        {
            return  await _usuarioRepositorio.BuscarTodos();
        }

        public async Task RegistrarLogin(Resposta<UserToken> resposta)
        {
            // Extract the inner UserToken object from the Resposta object
            var userToken = resposta.dados;
            if (userToken == null)
            {
                throw new ArgumentNullException(nameof(userToken), "UserToken não pode ser nulo.");
            }

            var token = userToken.Token;
            

            if (_httpContext.HttpContext.User?.Identity != null && _httpContext.HttpContext.User.Identity.IsAuthenticated)
            {
                await _httpContext.HttpContext.SignOutAsync();
                _httpContext.HttpContext.Session.Clear();
            }

            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            identity.AddClaim(new Claim(ClaimTypes.Role, "Usuario"));
            identity.AddClaim(new Claim(ClaimTypes.Name, userToken.Username));
            identity.AddClaim(new Claim(ClaimTypes.Sid, userToken.Id.ToString()));
            identity.AddClaim(new Claim("UsuarioId", userToken.Id.ToString()));
            identity.AddClaim(new Claim("NomeUsuario", userToken.Username));
            identity.AddClaim(new Claim("EmailUsuario", userToken.Email));
            identity.AddClaim(new Claim("TokenUsuario", userToken.Token));

            _httpContext.HttpContext.Session.SetString("Token", token);

            var claimPrincipal = new ClaimsPrincipal(identity);
            await _httpContext.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimPrincipal, new AuthenticationProperties
            {
                IsPersistent = true,
                AllowRefresh = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddMonths(1)
            });

            
        }
        
        public async Task Logout()
        {
            if (_httpContext.HttpContext.User.Identity != null && _httpContext.HttpContext.User.Identity.IsAuthenticated)
            {
                _httpContext.HttpContext.SignOutAsync();
                _httpContext.HttpContext.Session.Clear();
            }
        }

        public async Task<LoginUsuarioDTO?> BuscarPorId(Guid usuarioId)
        {
            var dtos = _mapper.Map<LoginUsuarioDTO>(await _usuarioRepositorio.BuscarUsuarioPorId(usuarioId)); 
            return dtos;
        }
        public async Task<LoginUsuarioDTO?> BuscarPorEmail(string email)
        {
            var dtos = _mapper.Map<LoginUsuarioDTO>(await _usuarioRepositorio.BuscarPorEmail(email));
            return dtos;
        }
        public async Task<LoginUsuarioDTO?> BuscarPorUsername(string username)
        {
            var dtos = _mapper.Map<LoginUsuarioDTO>(await _usuarioRepositorio.BuscarPorUsername(username));
            return dtos;
        }

        public async Task<UsuariosTelaInicialDTO?> BuscarUsuarioTelaCadastrarNovaSenha(Guid idUsuario)
        {
            var usuario = await _usuarioRepositorio.BuscarUsuarioPorId(idUsuario);
            if (usuario == null)
                return null;

            return _mapper.Map<UsuariosTelaInicialDTO>(usuario);
        }

        public async Task CadastrarNovaSenha(CadastrarNovaSenhaDTO dto)
        {
            await _usuarioServico.CadastrarSenha(dto.Token, dto.Email, dto.Senha, dto.ConfirmarSenha);
        }
        public async Task<EditarUsuarioDTO?> BuscarUsuarioParaEditarPorId(Guid id)
        {
            var usuario = await _usuarioRepositorio.BuscarUsuarioPorId(id);
            if (usuario == null)
                return null;

            return _mapper.Map<EditarUsuarioDTO>(usuario);
        }

        public async Task Atualizar(EditarUsuarioDTO? dto)
        {
            if (dto == null || !dto.Id.HasValue)
            {
                _notificador.Adicionar(new Notificacao("Usuario inválido !"));
                return;
            }            

            var usuario = _mapper.Map<Usuarios>(dto);            
           
            await _usuarioServico.Atualizar(usuario);
        }
        public async Task Excluir(Guid id)
        => await _usuarioServico.Excluir(id);

        public async Task TrocarUsuarioLogado(Guid usuarioId)
        {
            await _usuarioServico.TrocarUsuarioLogado(usuarioId);
        }
        public async Task<UsuariosTelaInicialDTO?> BuscarUsuarioPorId(Guid idUsuario)
        => _mapper.Map<UsuariosTelaInicialDTO?>(await _usuarioRepositorio.BuscarUsuarioPorId(idUsuario));
         
        public async Task ResetarSenha(string caminho, string email)
        {
            await _usuarioServico.ResetarSenha(caminho, email);
        }

        public async Task ValidarTokenCadastrarNovaSenha(string token)
        => await _usuarioServico.ValidarTokenResetarSenha(token);

        public async Task<bool> SenhaValidaLogin(string email, string senha)
        => await _usuarioServico.SenhaValidaLogin(email, senha);

        public async Task<bool> NomePrincipalJaCadastrado(string nome, Guid? id)
        {
            return await _usuarioServico.NomePrincipalJaCadastrado(nome, id); ;
        }

        public async Task<bool> EmailPrincipalJaCadastrado(string email, Guid? id)
        {
            return await _usuarioServico.EmailPrincipalJaCadastrado(email, id); ;
        }

        #region HttpClient
        private void ConfiguraClien(HttpClient client)
        {
            var enderecoBase = _configuration["baseURL:link"];
            var tokenClaim = !_httpContext.HttpContext.User.Identity.IsAuthenticated ? "" :
                               _httpContext.HttpContext.User?.Claims.First(c => c.Type == "TokenUsuario").Value.ToString();

            client.BaseAddress = new Uri(enderecoBase);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("Mozilla", "5.0"));
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + tokenClaim);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenClaim);
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

        public async Task<Resposta<UserToken>> PostWebApi<T1>(T1 model, string endPoint)
        {
            var enderecoBase = _configuration["baseURL:link"];
            if (string.IsNullOrEmpty(enderecoBase))
                return RetornoWebErro<UserToken>("Erro ao processar a resposta da API,o endereço base esta vazio.");

            try
            {
                using (var clienteAPI = new HttpClient())
                {
                    ConfiguraClien(clienteAPI);
                    var respostaPostAPI = await clienteAPI.PostAsync(endPoint, ConteudoJson(model));
                    var respostaconteudo = await respostaPostAPI.Content.ReadAsStringAsync();
                    var respostaObjeto = JsonConvert.DeserializeObject<Resposta<UserToken>>(respostaconteudo);

                    if (respostaObjeto == null || !respostaObjeto.sucesso)
                        return RetornoWebErro<UserToken>(respostaObjeto != null ? respostaObjeto.mensagem : "Erro ao processar a resposta da API, objeto de retorno nulo.");

                    return respostaObjeto;
                }
            }
            catch (Exception ex)
            {
               return RetornoWebErro<UserToken>("Erro de comunicação ao processar a requisição:" + ex.Message);
            }
        }

        public async Task<Resposta<IList<UsuariosTelaInicialDTO>>> GetWebApi(string endPoint)
        {
            try
            {
                using (var clienteAPI = new HttpClient())
                {
                    ConfiguraClien(clienteAPI);

                    var respostaPostAPI = await clienteAPI.GetAsync(endPoint);
                    var respostaconteudo = await respostaPostAPI.Content.ReadAsStringAsync();
                    var respostaObjeto = JsonConvert.DeserializeObject<Resposta<IList<UsuariosTelaInicialDTO>>>(respostaconteudo);

                    if (respostaObjeto == null || !respostaObjeto.sucesso) 
                        return RetornoWebErro<IList<UsuariosTelaInicialDTO>>(respostaObjeto != null ? respostaObjeto.mensagem : "Erro ao processar a resposta da API, objeto de retorno nulo.");
                                        
                    return respostaObjeto;
                }
            }
            catch (Exception ex)
            {
                return RetornoWebErro<IList<UsuariosTelaInicialDTO>>("Erro de comunicação ao processar a requisição:" + ex.Message);                
            }
        }
        #endregion
    }
}
