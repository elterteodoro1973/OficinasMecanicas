using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OficinasMecanicas.Dados.Contexto;
using OficinasMecanicas.Dominio.Entidades;
using OficinasMecanicas.Dominio.Interfaces.Repositorios;
using System.Threading;

namespace OficinasMecanicas.Dados.Repositorios
{

    public class OficinaMecanicaRepositorio : IOficinaMecanicaRepositorio
    {

        private readonly DbContexto _contexto;
        readonly ILogger<UsuarioRepositorio> _logger;
        public OficinaMecanicaRepositorio(DbContexto contexto, ILogger<UsuarioRepositorio> _logger)
        {
            _contexto = contexto;
            _logger = _logger;
        }

        public async Task<OficinaMecanica> Adicionar(OficinaMecanica oficina, CancellationToken cancellationToken = default)
        {
            await _contexto.OficinaMecanica.AddAsync(oficina, cancellationToken);
            await _contexto.SaveChangesAsync(cancellationToken);
            return oficina;
        }

        public async Task<OficinaMecanica> Atualizar(OficinaMecanica oficina, CancellationToken cancellationToken = default)
        {
            _contexto.OficinaMecanica.Update(oficina);
            await _contexto.SaveChangesAsync(cancellationToken);
            return oficina;
        }

        public async Task<OficinaMecanica> Excluir(Guid id, CancellationToken cancellationToken = default)
        {
            var oficina = await _contexto.OficinaMecanica.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
            if (oficina == null)
            {
                throw new InvalidOperationException("Oficina não encontrada.");
            }
            _contexto.OficinaMecanica.Remove(oficina);
            await _contexto.SaveChangesAsync(cancellationToken);

            return oficina;
        }

        public async Task<OficinaMecanica?> BuscarPorId(Guid id)
        => await _contexto.OficinaMecanica.Where(c => c.Id == id).AsNoTracking().FirstOrDefaultAsync();

        public async Task<OficinaMecanica?> BuscarPorNome(string nome)
        => await _contexto.OficinaMecanica.Where(c => c.Nome.ToUpper() == nome.ToUpper()).AsNoTracking().FirstOrDefaultAsync();

        public async Task<IEnumerable<OficinaMecanica>> BuscarTodos()
         => await _contexto.OficinaMecanica.AsNoTracking().ToListAsync();

        public async Task<bool> NomePrincipalJaCadastrado(string nome, Guid? id)
        {
            return await _contexto.Usuarios.Where(c => c.Email == nome.ToUpper() && (id.HasValue ? c.Id != id : true)).AnyAsync();
        }
    }
}
