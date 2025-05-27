using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using OficinasMecanicas.Aplicacao.DTO.Usuarios;
using OficinasMecanicas.Aplicacao.Interfaces;
using OficinasMecanicas.Dominio.Entidades;
using OficinasMecanicas.Dominio.Interfaces;
using OficinasMecanicas.Dominio.Interfaces.Repositorios;
using OficinasMecanicas.Dominio.Interfaces.Servicos;
using OficinasMecanicas.Dominio.Notificacoes;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace OficinasMecanicas.Aplicacao.Servicos
{
    public class UsuarioAppServico : IUsuarioAppServico
    {
        private readonly IUsuarioServico _usuarioServico;
        private readonly IUsuarioRepositorio _usuarioRepositorio;        
        private readonly INotificador _notificador;
        private readonly IMapper _mapper;
       
        
        public UsuarioAppServico(IMapper mapper,INotificador notificador,IUsuarioRepositorio usuarioRepositorio,IUsuarioServico usuarioServico)
        {
            _usuarioRepositorio = usuarioRepositorio;
            _mapper = mapper;
            _notificador = notificador;
            _usuarioServico = usuarioServico;
        }

        public async Task Cadastrar(string caminho, CadastrarEditarUsuarioDTO dto)
        {
            var usuario = _mapper.Map<Usuarios>(dto);           

            await _usuarioServico.Adicionar(caminho, usuario);
        }

        public async Task<IList<UsuariosTelaInicialDTO>> ListarUsuariosTelaInicial(string? filtro)
        { 
            var dtos = _mapper.Map<IList<UsuariosTelaInicialDTO>>(await _usuarioRepositorio.BuscarTodos());

            if (!string.IsNullOrEmpty(filtro) && !string.IsNullOrWhiteSpace(filtro))
            {
                dtos = dtos.Where(c => c.Nome.ToUpper().Contains(filtro.ToUpper()) || c.Email.ToUpper().Contains(filtro.ToUpper()) ).ToList();
            }

            return dtos;
        } 

        public async Task Login(string caminho, string email, string senha)
        => await _usuarioServico.Login(caminho, email, senha);

        public async Task Logout()
        {
            _usuarioServico.Logout();
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
        public async Task<CadastrarEditarUsuarioDTO?> BuscarUsuarioParaEditarPorId(Guid id)
        {
            var usuario = await _usuarioRepositorio.BuscarUsuarioPorId(id);
            if (usuario == null)
                return null;

            return _mapper.Map<CadastrarEditarUsuarioDTO>(usuario);
        }

        public async Task Editar(CadastrarEditarUsuarioDTO? dto)
        {
            if (dto == null || !dto.Id.HasValue)
            {
                _notificador.Adicionar(new Notificacao("Usuario inválido !"));
                return;
            }            

            var usuario = _mapper.Map<Usuarios>(dto);
            
           
            await _usuarioServico.Editar(usuario);
        }
        public async Task Excluir(Guid id)
        => await _usuarioServico.Excluir(id);

        public async Task TrocarUsuarioLogado(Guid usuarioId)
        {
            await _usuarioServico.TrocarUsuarioLogado(usuarioId);
        }

        public async Task<UsuariosTelaInicialDTO?> BuscarUsuarioPorId(Guid idUsuario)
        => _mapper.Map<UsuariosTelaInicialDTO?>(await _usuarioRepositorio.BuscarUsuarioPorId(idUsuario));
         
        private string BuscarNomeCampoLog(string nomeCampo)
        {
            string nomeNormalizado = "";
            switch (nomeCampo)
            {
                case nameof(Usuarios.Username):
                    nomeNormalizado = "Nome";
                    break;                          

               
                default:
                    break;
            }
            if (string.IsNullOrEmpty(nomeNormalizado) && nomeCampo.Contains("Email"))
                nomeNormalizado = "E-mail";

            return nomeNormalizado;
        }

        public async Task ResetarSenha(string caminho, string email)
        {
            await _usuarioServico.ResetarSenha(caminho, email);
        }

        public async Task ValidarTokenCadastrarNovaSenha(string token)
        => await _usuarioServico.ValidarTokenResetarSenha(token);

        
    }
}
