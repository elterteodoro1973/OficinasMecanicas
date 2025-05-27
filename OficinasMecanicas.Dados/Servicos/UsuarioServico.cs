using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using OficinasMecanicas.Dominio.DTO;
using OficinasMecanicas.Dominio.Entidades;
using OficinasMecanicas.Dominio.Entidades.Validacoes.Usuarios;
using OficinasMecanicas.Dominio.Interfaces;
using OficinasMecanicas.Dominio.Interfaces.Repositorios;
using OficinasMecanicas.Dominio.Interfaces.Servicos;
using OficinasMecanicas.Dominio.Notificacoes;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace OficinasMecanicas.Dados.Servicos
{
    public class UsuarioServico : BaseServico<Usuarios>, IUsuarioServico
    {
        private readonly IHttpContextAccessor _httpContext;
        private readonly IUsuarioRepositorio _usuarioRepositorio;
        private readonly IResetarSenhaRepositorio _resetarSenhaRepositorio;
        private INotificador _notificador;       
             
        private readonly EmailConfiguracao _emailConfiguracao;
        private readonly IEmailServico _emailServico;
       

        public UsuarioServico(IHttpContextAccessor httpContext, 
            IUsuarioRepositorio usuarioRepositorio,
            IResetarSenhaRepositorio resetarSenhaRepositorio,
            INotificador notificador,
            IEmailServico emailServico,            
            IOptions<EmailConfiguracao> emailConfiguracao  ) : base(notificador)
        {
            _httpContext = httpContext;
            _usuarioRepositorio = usuarioRepositorio;
            _resetarSenhaRepositorio = resetarSenhaRepositorio;
            _notificador = notificador;
            _emailConfiguracao = emailConfiguracao.Value;
            _emailServico = emailServico;
        }

        public async Task Adicionar(string caminho, Usuarios usuario)
        {
            await _ValidarInclusao(usuario);

            if (_notificador.TemNotificacao()) return; 
            try
            {
                usuario.Id = Guid.NewGuid();
                await _usuarioRepositorio.Adicionar(usuario);

                var token = Guid.NewGuid();
                ResetarSenha resetarSenhaDB = new ResetarSenha()
                {
                    Id = Guid.NewGuid(),
                    Token = GerarHash512(token.ToString()).Replace("/", "b").Replace("=", "a").Replace("+", "C"),
                    UsuarioId = usuario.Id,
                    DataSolicitacao = DateTime.Now,
                    DataExpiracao = DateTime.Now.AddHours(24),
                    Excluido = false
                };
                await _resetarSenhaRepositorio.Adicionar(resetarSenhaDB);

                var urlResetSenha = string.Concat(_httpContext.HttpContext.Request.Scheme, "://", _httpContext.HttpContext.Request.Host.Value, $"/Usuarios/CadastrarNovaSenha?token={resetarSenhaDB.Token}");
                string textoEmail = _emailServico.GetCredenciasPrimeiroAcesso(caminho, usuario.Username, urlResetSenha);
                await _emailServico.Enviar(usuario.Email, "Cadastrar Senha!", textoEmail);

                
            }
            catch (Exception ex)
            {                
                _notificador.Adicionar(new Notificacao("Erro ao adicionar o Usuario !" + ex.Message));
            }
        }

        public async Task Editar(Usuarios usuario)
        {
            await _ValidarEdicao(usuario);

            if (_notificador.TemNotificacao()) return;

            var usuarioDB = await _usuarioRepositorio.BuscarUsuarioPorId(usuario.Id);
            
            try
            {
                usuarioDB.Username = usuario.Username;                
                usuarioDB.Email = usuario.Email;
                await _usuarioRepositorio.Atualizar(usuarioDB);               
            }
            catch (Exception ex)
            {                
                _notificador.Adicionar(new Notificacao("Erro ao ediar o Usuario !" + ex.Message));
            }
        }

        public async Task Excluir(Guid usuarioId)
        {
            await _ValidarExclusao(usuarioId);

            if (_notificador.TemNotificacao()) return;

            var usuarioDB = await _usuarioRepositorio.BuscarUsuarioPorId(usuarioId);
            if (usuarioDB == null)
            {
                _notificador.Adicionar(new Notificacao("Usuário não encontrado !"));
               return;
            }


            try
            {
                await _usuarioRepositorio.Excluir(usuarioId);
            }
            catch (Exception ex)
            {                
                _notificador.Adicionar(new Notificacao("Erro ao excluir o usuário !"));
            }
        }

        public async Task Login(string caminho,  string email, string senha)
        {
            await _ValidarLogin(email, senha);

            if (_notificador.TemNotificacao()) return;

            var usuarioDB = await _usuarioRepositorio.BuscarPorEmail(email);

            if (!await _usuarioRepositorio.SenhaValidaLogin(email, GetSha256Hash(senha)))
            {
                _notificador.Adicionar(new Notificacao("Senha inválida !"));
                return;
            }
            else
            {
                
                await Login(usuarioDB);
            }
        }
        private async Task Login(Usuarios usuarioDB)
        {
            if (_httpContext.HttpContext.User.Identity != null && _httpContext.HttpContext.User.Identity.IsAuthenticated)
            {
                await _httpContext.HttpContext.SignOutAsync();
                _httpContext.HttpContext.Session.Clear();
            }

            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            identity.AddClaim(new Claim("UsuarioId", usuarioDB.Id.ToString()));
            identity.AddClaim(new Claim("NomeUsuario", usuarioDB.Username));


            

            var claimPrincipal = new ClaimsPrincipal(identity);
            await _httpContext.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimPrincipal, new AuthenticationProperties
            {
                IsPersistent = false,
                AllowRefresh = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddMonths(1)
            });
        }



        public async Task<Usuarios?> BuscarPorId(Guid usuarioId)
        {
           return await _usuarioRepositorio.BuscarUsuarioPorId(usuarioId);
        }
        public async Task<Usuarios?> BuscarPorEmail(string email)
        {
            return  await _usuarioRepositorio.BuscarPorEmail(email);
        }
        
        public async Task<Usuarios?> BuscarPorUsername(string username)
        {
            return await _usuarioRepositorio.BuscarPorUsername(username);
        }


        private async Task _ValidarInclusao(Usuarios usuario)
        {
            if (!ExecutarValidacao<CadastrarEditarUsuarioValidacao, Usuarios>(new CadastrarEditarUsuarioValidacao(), usuario)) return;

            if (_notificador.TemNotificacao()) return;
                        
            if (await _usuarioRepositorio.NomePrincipalJaCadastrado(usuario.Username, null))
                _notificador.Adicionar(new Notificacao("Nome do usuário já cadastrado !"));           

            if (await _usuarioRepositorio.EmailPrincipalJaCadastrado(usuario.Email, null))
                _notificador.Adicionar(new Notificacao("E-mail do usuário já cadastrado !"));

        }

        private async Task _ValidarEdicao(Usuarios usuario)
        {
            if (!ExecutarValidacao<CadastrarEditarUsuarioValidacao, Usuarios>(new CadastrarEditarUsuarioValidacao(true), usuario)) return;
              
            if (_notificador.TemNotificacao()) return;

            if (await _usuarioRepositorio.NomePrincipalJaCadastrado(usuario.Username,usuario.Id))
                _notificador.Adicionar(new Notificacao("Nome do usuário já cadastrado !"));
            

            if (await _usuarioRepositorio.EmailPrincipalJaCadastrado(usuario.Email,usuario.Id))
                _notificador.Adicionar(new Notificacao("E-mail principal já cadastrado !"));
        }

        private async Task _ValidarExclusao(Guid usuarioId)
        {
            if (usuarioId == Guid.Empty)
            {
                _notificador.Adicionar(new Notificacao("Identificador de Usuário obrigatório !"));
            }
            
        }

        private string GetSha256Hash(string input)
        {
            var byteValue = Encoding.UTF8.GetBytes(input);
            var byteHash = SHA256.HashData(byteValue);
            return Convert.ToBase64String(byteHash);
        }

        public void Logout()
        {
            if (_httpContext.HttpContext.User.Identity != null && _httpContext.HttpContext.User.Identity.IsAuthenticated)
            {
                _httpContext.HttpContext.SignOutAsync();
                _httpContext.HttpContext.Session.Clear();
            }
        }

        private async Task _ValidarLogin(string email, string senha)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrWhiteSpace(email))
                _notificador.Adicionar(new Notificacao("E-mail é obrigatório !"));

            if (string.IsNullOrEmpty(senha) || string.IsNullOrWhiteSpace(senha))
                _notificador.Adicionar(new Notificacao("Senha é obrigatório !"));

            if (_notificador.TemNotificacao()) return;

            if (!await _usuarioRepositorio.EmailValidoLogin(email))
            {
                _notificador.Adicionar(new Notificacao("E-mail é inválido !"));
                return;
            }

            var usuario = await _usuarioRepositorio.BuscarPorEmail(email);

            

        }

        public async Task CadastrarSenha(string token, string email, string senha, string confirmarSenha)
        {
            await _ValidarCadastroNovaSenha(token, email, senha, confirmarSenha);

            if (_notificador.TemNotificacao()) return;

            var usuarioDB = await _usuarioRepositorio.BuscarPorEmail( email);
            var resetarSenhaDB = await _resetarSenhaRepositorio.BuscarResetarSenhaPorToken(token);            

            try
            {                
                usuarioDB.PasswordHash = GetSha256Hash(senha);                
                await _usuarioRepositorio.Atualizar(usuarioDB);
                

                resetarSenhaDB.Efetivado = true;
                await _resetarSenhaRepositorio.Atualizar(resetarSenhaDB);                
            }
            catch (Exception ex)
            {
                
                _notificador.Adicionar(new Notificacao("Erro ao cadastrar a nova senha do usuário !" + ex.Message));
            }

            await Login(usuarioDB);
        }

        private async Task _ValidarCadastroNovaSenha(string token, string email, string senha, string confirmarSenha)
        {
            await ValidarTokenResetarSenha(token);

            if (string.IsNullOrEmpty(senha) || string.IsNullOrWhiteSpace(senha))
                _notificador.Adicionar(new Notificacao("Senha é obrigatório !"));
            else if (senha.Length > 30)
                _notificador.Adicionar(new Notificacao("Senha possui tamanho máximo de 50 caracteres !"));

            if (_notificador.TemNotificacao()) return;

            if (string.IsNullOrEmpty(confirmarSenha) || string.IsNullOrWhiteSpace(confirmarSenha))
                _notificador.Adicionar(new Notificacao("Confirmar senha é obrigatório !"));
            else if (confirmarSenha.Length > 30)
                _notificador.Adicionar(new Notificacao("Confirmar senha possui tamanho máximo de 50 caracteres !"));

            if (_notificador.TemNotificacao()) return;

            if (senha != confirmarSenha)
                _notificador.Adicionar(new Notificacao("Senhas diferentes !"));
        }

        public async Task TrocarUsuarioLogado(Guid usuarioId)
        {
            await _ValidarTrocaUsuarioLogado(usuarioId);

            if (_notificador.TemNotificacao()) return;

            var usuarioDB = await _usuarioRepositorio.BuscarUsuarioPorId(usuarioId);

            if (_httpContext.HttpContext.User.Identity != null && _httpContext.HttpContext.User.Identity.IsAuthenticated)
            {
                await _httpContext.HttpContext.SignOutAsync();
                _httpContext.HttpContext.Session.Clear();
            }

            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            identity.AddClaim(new Claim("UsuarioId", usuarioDB.Id.ToString()));
            identity.AddClaim(new Claim("NomeUsuario", usuarioDB.Username));


            var claimPrincipal = new ClaimsPrincipal(identity);
            await _httpContext.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimPrincipal, new AuthenticationProperties
            {
                IsPersistent = false,
                AllowRefresh = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddMonths(1)
            });

        }

        private async Task _ValidarTrocaUsuarioLogado(Guid usuarioId)
        {
            if (usuarioId == Guid.Empty)
                _notificador.Adicionar(new Notificacao("Identificador de usuário é obrigatório !"));           

            if (_notificador.TemNotificacao()) return;

            if (!await _usuarioRepositorio.IdUsuarioValido(usuarioId))
                _notificador.Adicionar(new Notificacao("Identificador de usuário inválido !"));            

            if (_notificador.TemNotificacao()) return;
        }

        public async Task ResetarSenha(string caminho, string email)
        {
            await _ValidarReseteSenha(email);

            if (_notificador.TemNotificacao()) return;

            var usuarioDB = await _usuarioRepositorio.BuscarPorEmail(email);            
            
            try
            {
                var token = Guid.NewGuid();

                ResetarSenha resetarSenhaDB = new ResetarSenha()
                {
                    Id = Guid.NewGuid(),
                    Token = GerarHash512(token.ToString()).Replace("/", "b").Replace("=", "a").Replace("+", "C"),
                    UsuarioId = usuarioDB.Id,
                    DataSolicitacao = DateTime.Now,
                    DataExpiracao = DateTime.Now.AddHours(4),
                    Excluido = false
                };
                await _resetarSenhaRepositorio.Adicionar(resetarSenhaDB);

                var urlResetSenha = string.Concat(_httpContext.HttpContext.Request.Scheme, "://", _httpContext.HttpContext.Request.Host.Value, $"/Usuarios/CadastrarNovaSenha?token={resetarSenhaDB.Token}");
                string textoEmail = _emailServico.GetTextoResetSenha(caminho, usuarioDB.Username, urlResetSenha);
                await _emailServico.Enviar(email, "Resetar Senha!", textoEmail);               
            }
            catch (Exception ex)
            {                
                
                _notificador.Adicionar(new Notificacao("Erro ao resetar a senha do usuário: " + ex.Message));
            }
            return;
        }        
        
        public static string GerarHash512(string chave)
        {
            byte[] saltBytes = null;

            int minSaltSize = 4;
            int maxSaltSize = 8;

            Random random = new Random();
            int saltSize = random.Next(minSaltSize, maxSaltSize);

            saltBytes = new byte[saltSize];
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            rng.GetNonZeroBytes(saltBytes);

            byte[] plainTextBytes = Encoding.UTF8.GetBytes(chave);
            byte[] plainTextWithSaltBytes = new byte[plainTextBytes.Length + saltBytes.Length];

            for (int i = 0; i < plainTextBytes.Length; i++)
                plainTextWithSaltBytes[i] = plainTextBytes[i];

            for (int i = 0; i < saltBytes.Length; i++)
                plainTextWithSaltBytes[plainTextBytes.Length + i] = saltBytes[i];

            HashAlgorithm hash = new SHA512Managed();
            byte[] hashBytes = hash.ComputeHash(plainTextWithSaltBytes);
            byte[] hashWithSaltBytes = new byte[hashBytes.Length + saltBytes.Length];

            for (int i = 0; i < hashBytes.Length; i++)
                hashWithSaltBytes[i] = hashBytes[i];

            for (int i = 0; i < saltBytes.Length; i++)
                hashWithSaltBytes[hashBytes.Length + i] = saltBytes[i];

            string hashValue = Convert.ToBase64String(hashWithSaltBytes);

            return hashValue;
        }

        private async Task _ValidarReseteSenha(string email)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrWhiteSpace(email))
                _notificador.Adicionar(new Notificacao("E-mail é obrigatório !"));
            else if (!await _usuarioRepositorio.EmailValidoLogin(email))
                _notificador.Adicionar(new Notificacao("E-mail inválido !"));
        }        

        public async Task ValidarTokenResetarSenha(string token)
        {
            if (token == string.Empty)
            {
                _notificador.Adicionar(new Notificacao("Token é obrigatório !"));
                return;
            }

            var resetarSenha = await _resetarSenhaRepositorio.BuscarResetarSenhaPorToken(token);

            if (resetarSenha == null)
            {
                _notificador.Adicionar(new Notificacao("Token inválido!"));
                return;
            }
            else if (resetarSenha.DataExpiracao < DateTime.Now)
            {
                _notificador.Adicionar(new Notificacao("Link de Acesso vencido !"));
                return;
            }
            else if (resetarSenha.Efetivado.Value )
            {
                _notificador.Adicionar(new Notificacao("Token já validado!"));
                return;
            }
        }

        
    }
}
