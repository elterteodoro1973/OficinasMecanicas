using OficinasMecanicas.Aplicacao.DTO;
using OficinasMecanicas.Aplicacao.DTO.Usuarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace OficinasMecanicas.Aplicacao.Interfaces
{
    public interface IUsuarioAppServico
    {
        Task<IList<UsuariosTelaInicialDTO>> ListarUsuariosTelaInicial(string? filtro);
        Task Logout();
        Task Login(string caminho, string email, string senha);
        Task Cadastrar(string caminho, CadastrarEditarUsuarioDTO dto);
        Task Editar(CadastrarEditarUsuarioDTO dto);

        Task<LoginUsuarioDTO?> BuscarPorId(Guid usuarioId);
        Task<LoginUsuarioDTO?> BuscarPorEmail(string email);
        Task<LoginUsuarioDTO?> BuscarPorUsername(string username);
        Task CadastrarNovaSenha( CadastrarNovaSenhaDTO dto);        
        Task<CadastrarEditarUsuarioDTO?> BuscarUsuarioParaEditarPorId(Guid id);
        Task<UsuariosTelaInicialDTO?> BuscarUsuarioTelaCadastrarNovaSenha(Guid idUsuario);        
        Task Excluir(Guid id);
        Task ResetarSenha(string caminho, string email);
        Task TrocarUsuarioLogado(Guid usuarioId);
        Task<UsuariosTelaInicialDTO?> BuscarUsuarioPorId(Guid idUsuario);     

        Task ValidarTokenCadastrarNovaSenha(string token);

    }
}
