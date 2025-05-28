using OficinasMecanicas.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OficinasMecanicas.Dominio.Interfaces.Servicos
{
    public interface IUsuariosServico
    {        
        Task<Usuarios?> Adicionar(string caminho, Usuarios usuario);
        Task Atualizar(Usuarios usuario);
        Task Excluir(Guid id);
        Task<Usuarios?> BuscarPorId(Guid id);
        Task<Usuarios?> BuscarPorEmail(string email);
        Task<Usuarios?> BuscarPorUsername(string Username);
        Task CadastrarSenha(string token, string email, string senha, string novaSenha);
        Task TrocarUsuarioLogado(Guid id);
        Task ResetarSenha(string caminho, string email);
        Task ValidarTokenResetarSenha(string token);
        Task<bool> SenhaValidaLogin(string email, string senha);
        Task<bool> NomePrincipalJaCadastrado(string nome, Guid? id);
        Task<bool> EmailPrincipalJaCadastrado(string email, Guid? id);
        string GetSha256Hash(string input);
    }
}
