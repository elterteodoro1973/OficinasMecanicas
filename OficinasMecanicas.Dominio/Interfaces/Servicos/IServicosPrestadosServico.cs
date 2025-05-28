using OficinasMecanicas.Dominio.Entidades;

namespace OficinasMecanicas.Dominio.Interfaces.Servicos
{
    public interface IServicosPrestadosServico
    {
        Task<IEnumerable<ServicosPrestados>> BuscarTodos(); // Ensure Servicos is a class or type defined in the correct namespace
    }
}
