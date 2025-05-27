using OficinasMecanicas.Dominio.Entidades;

namespace OficinasMecanicas.Dominio.Interfaces.Repositorios
{
    public interface IResetarSenhaRepositorio 
    {

        Task<ResetarSenha> Adicionar(ResetarSenha resetarSenha, CancellationToken cancellationToken = default);
        Task<ResetarSenha> Atualizar(ResetarSenha resetarSenha, CancellationToken cancellationToken = default);
        Task<ResetarSenha> Excluir(Guid id, CancellationToken cancellationToken = default);
        Task<ResetarSenha?> BuscarResetarSenhaPorId(Guid id);
        Task<ResetarSenha?> BuscarResetarSenhaPorToken(string token);
        Task<IList<ResetarSenha>> ListarResetarSenha();
    }
}
