using OficinasMecanicas.Aplicacao.DTO;
using OficinasMecanicas.Aplicacao.DTO.Usuarios;
using OficinasMecanicas.Aplicacao.Model;
using OficinasMecanicas.Dominio.Entidades;
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
        
        Task Logout();
        Task RegistrarLogin(Resposta<UserToken> resposta);
        Task<Resposta<UserToken>> PostarRequisicao<T1>(T1 model, string endPoint);
        Task <EditarUsuarioDTO?> Adicionar(string caminho, CadastrarUsuarioDTO dto);
        Task Atualizar(EditarUsuarioDTO dto);

        Task Excluir(Guid id);

        Task<IList<UsuariosTelaInicialDTO>> ListarUsuariosTelaInicial(string? filtro);

        Task<IList<UsuariosTelaInicialDTO>> BuscarTodos();
        Task<LoginUsuarioDTO?> BuscarPorId(Guid usuarioId);
        Task<LoginUsuarioDTO?> BuscarPorEmail(string email);
        Task<LoginUsuarioDTO?> BuscarPorUsername(string username);
        Task CadastrarNovaSenha( CadastrarNovaSenhaDTO dto);        
        Task<EditarUsuarioDTO?> BuscarUsuarioParaEditarPorId(Guid id);
        Task<UsuariosTelaInicialDTO?> BuscarUsuarioTelaCadastrarNovaSenha(Guid idUsuario);        
        
        Task ResetarSenha(string caminho, string email);
        Task TrocarUsuarioLogado(Guid usuarioId);
        Task<UsuariosTelaInicialDTO?> BuscarUsuarioPorId(Guid idUsuario);  
        Task ValidarTokenCadastrarNovaSenha(string token);
        Task<bool> SenhaValidaLogin(string email, string senha);
        Task<bool> NomePrincipalJaCadastrado(string nome, Guid? id);
        Task<bool> EmailPrincipalJaCadastrado(string email, Guid? id);
    }
}
