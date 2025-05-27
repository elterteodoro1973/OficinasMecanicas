using OficinasMecanicas.Dominio.Entidades;
using OficinasMecanicas.Dominio.Interfaces.Servicos;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OficinasMecanicas.Dominio.Interfaces.Repositorios
{
    public interface IUsuarioRepositorio 
    {
        Task<Usuarios> Adicionar(Usuarios usuario, CancellationToken cancellationToken = default);
        Task<Usuarios> Atualizar(Usuarios usuario, CancellationToken cancellationToken = default);
        Task<Usuarios> Excluir(Guid id, CancellationToken cancellationToken = default);
        Task<IList<Usuarios>> BuscarTodos();
        Task<Usuarios?> BuscarUsuarioPorId(Guid id);
        Task<Usuarios?> BuscarPorEmail(string email);
        Task<Usuarios?> BuscarPorUsername(string username);
        Task<bool> EmailValidoLogin(string email);
        Task<bool> UsuarioNaoPossuiSenhaCadastrada(string email);
        Task<bool> SenhaValidaLogin(string email, string senha);
        Task<bool> NomePrincipalJaCadastrado(string nome, Guid? id);        
        Task<bool> EmailPrincipalJaCadastrado(string email, Guid? id);
        Task<bool> IdUsuarioValido(Guid id);
        
       
    }
}
