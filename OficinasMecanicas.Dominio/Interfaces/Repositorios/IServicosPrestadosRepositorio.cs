using OficinasMecanicas.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OficinasMecanicas.Dominio.Interfaces.Repositorios
{
    public interface IServicosPrestadosRepositorio
    {
        Task<IEnumerable<ServicosPrestados>> BuscarTodos(); 
    }
}
