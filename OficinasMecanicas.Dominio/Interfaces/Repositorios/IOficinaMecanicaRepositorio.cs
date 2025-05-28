using OficinasMecanicas.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OficinasMecanicas.Dominio.Interfaces.Repositorios
{
    public interface IOficinaMecanicaRepositorio
    { 
        Task<OficinaMecanica> Adicionar(OficinaMecanica oficina, CancellationToken cancellationToken = default);
        Task<OficinaMecanica> Atualizar(OficinaMecanica oficina, CancellationToken cancellationToken = default);
        Task<OficinaMecanica> Excluir(Guid id, CancellationToken cancellationToken = default);

        Task<IEnumerable<OficinaMecanica>> BuscarTodos();
        Task<OficinaMecanica?> BuscarPorId(Guid id);
        Task<OficinaMecanica?> BuscarPorNome(string nome);
        Task<bool> NomePrincipalJaCadastrado(string nome, Guid? id);
    }
}
