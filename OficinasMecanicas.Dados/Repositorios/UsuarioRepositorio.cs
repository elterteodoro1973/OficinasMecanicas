using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OficinasMecanicas.Dados.Contexto;
using OficinasMecanicas.Dominio.Entidades;
using OficinasMecanicas.Dominio.Interfaces.Repositorios;


namespace OficinasMecanicas.Dados.Repositorios
{
    public class UsuarioRepositorio : IUsuarioRepositorio
    {
        private readonly DbContexto _contexto;
        readonly ILogger<UsuarioRepositorio> _logger;
        public UsuarioRepositorio(DbContexto contexto, ILogger<UsuarioRepositorio> _logger) 
        {
            _contexto = contexto;
            _logger = _logger;
        }

        public async Task<Usuarios> Adicionar(Usuarios usuario, CancellationToken cancellationToken = default)
        {
            await _contexto.Usuarios.AddAsync(usuario, cancellationToken);
            await _contexto.SaveChangesAsync(cancellationToken);
            return usuario;
        }
        
        public async Task<Usuarios> Atualizar(Usuarios usuario, CancellationToken cancellationToken = default)
        {
            _contexto.Usuarios.Update(usuario);
            await _contexto.SaveChangesAsync(cancellationToken);
            return usuario;
        }

        public async Task<Usuarios?> Excluir(Guid id, CancellationToken cancellationToken = default)
        {
            var usuario = await _contexto.Usuarios.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
            if (usuario == null)
            {
                throw new InvalidOperationException("Usuário não encontrado.");
            }            
            _contexto.Usuarios.Remove(usuario);            
            await _contexto.SaveChangesAsync(cancellationToken);

            return usuario;
        }

        public async Task<bool> NomePrincipalJaCadastrado(string nome, Guid? id)
        {
            return await _contexto.Usuarios.Where(c => c.Username == nome.ToUpper() && (id.HasValue ? c.Id != id : true)).AnyAsync();
        }
                
        public async Task<bool> EmailPrincipalJaCadastrado(string email, Guid? id)
        {
            return await _contexto.Usuarios.Where(c =>  c.Email == email.ToUpper() && (id.HasValue ? c.Id != id : true)).AnyAsync();
        }
               
        public async Task<bool> IdUsuarioValido(Guid id)
        => await _contexto.Usuarios.Where(c => c.Id == id).AnyAsync();

        public async Task<IList<Usuarios>> BuscarTodos() 
        => await _contexto.Usuarios.AsNoTracking().ToListAsync();

        public async Task<Usuarios?> BuscarUsuarioPorId(Guid id)
        => await _contexto.Usuarios.Where(c => c.Id == id).AsNoTracking().FirstOrDefaultAsync();
        
        public async Task<Usuarios?> BuscarPorEmail(string email)
        => await _contexto.Usuarios.Where(c => c.Email.ToUpper() == email.ToUpper()).AsNoTracking().FirstOrDefaultAsync();

        public async Task<Usuarios?> BuscarPorUsername(string username)
        => await _contexto.Usuarios.Where(c => c.Username.ToUpper() == username.ToUpper()).AsNoTracking().FirstOrDefaultAsync();

        public async Task<bool> EmailValidoLogin(string email)
        => await _contexto.Usuarios.Where(c =>  c.Email == email.ToUpper().Trim()).AnyAsync();

        public async Task<bool> SenhaValidaLogin(string email, string senha)
        => await _contexto.Usuarios.Where(c => c.Email == email.ToUpper().Trim() && c.PasswordHash == senha).AnyAsync();

        public async Task<bool> UsuarioNaoPossuiSenhaCadastrada(string email)
        => await _contexto.Usuarios.Where(c =>  c.Email == email.ToUpper().Trim() ).AnyAsync();        
    }
}
