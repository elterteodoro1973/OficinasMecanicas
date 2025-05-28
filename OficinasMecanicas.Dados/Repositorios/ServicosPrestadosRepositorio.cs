using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OficinasMecanicas.Dados.Contexto;
using OficinasMecanicas.Dominio.Entidades;
using OficinasMecanicas.Dominio.Interfaces.Repositorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OficinasMecanicas.Dados.Repositorios
{
    
    public class ServicosPrestadosRepositorio : IServicosPrestadosRepositorio
    {
        private readonly DbContexto _contexto;
        readonly ILogger<UsuarioRepositorio> _logger;
        public ServicosPrestadosRepositorio(DbContexto contexto, ILogger<UsuarioRepositorio> _logger)
        {
            _contexto = contexto;
            _logger = _logger;
        }

        public async Task<IEnumerable<ServicosPrestados>> BuscarTodos()
        {
            return await _contexto.ServicosPrestados.AsNoTracking().ToListAsync();
        }
    }
}
