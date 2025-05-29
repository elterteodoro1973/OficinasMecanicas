

using OficinasMecanicas.Dominio.Entidades;

namespace OficinasMecanicas.Dominio.Interfaces.Servicos
{
    public  interface IOficinaMecanicaServico
    {   
        Task<OficinaMecanica?> Adicionar(OficinaMecanica oficina);
        Task Atualizar(Guid id,OficinaMecanica oficina);
        Task Excluir(Guid id);
        Task<IEnumerable<OficinaMecanica>> BuscarTodos();
        Task<OficinaMecanica?> BuscarPorId(Guid id);
        Task<OficinaMecanica?> BuscarPorNome(string nome);
        Task<bool> NomePrincipalJaCadastrado(string nome, Guid? id);
    }
}
