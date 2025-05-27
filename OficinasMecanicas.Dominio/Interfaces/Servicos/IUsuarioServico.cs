using OficinasMecanicas.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OficinasMecanicas.Dominio.Interfaces.Servicos
{
    public interface IUsuarioServico
    {
        Task Login(string caminho, string email, string senha);
        Task Adicionar(string caminho, Usuarios usuario);
        Task Editar(Usuarios usuario);
        Task Excluir(Guid usuarioId);

        Task<Usuarios?> BuscarPorId(Guid usuarioId);
        Task<Usuarios?>  BuscarPorEmail(string email);
        Task<Usuarios?> BuscarPorUsername(string Username);

        Task CadastrarSenha(string token, string email, string senha, string novaSenha);
        Task TrocarUsuarioLogado(Guid usuarioId); 
        void Logout();
        Task ResetarSenha(string caminho, string email);
        Task ValidarTokenResetarSenha(string token);

    }
}
