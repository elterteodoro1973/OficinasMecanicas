using Microsoft.AspNetCore.Http;
using OficinasMecanicas.Dominio.Interfaces.Repositorios;
using OficinasMecanicas.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OficinasMecanicas.Dominio.Entidades;
using OficinasMecanicas.Dominio.Interfaces.Servicos;

namespace OficinasMecanicas.Dados.Servicos
{
    public class ServicosPrestadosServico : BaseServico<ServicosPrestados>, IServicosPrestadosServico
    {        
        private readonly IServicosPrestadosRepositorio _servicosPrestadosRepositorio;
        private readonly INotificador _notificador;

        public ServicosPrestadosServico(IHttpContextAccessor httpContext,
            IServicosPrestadosRepositorio servicosPrestadosRepositorio,
            INotificador notificador) : base(notificador)
        {            
            _servicosPrestadosRepositorio = servicosPrestadosRepositorio;
            _notificador = notificador;
        }

        public async Task<IEnumerable<ServicosPrestados>> BuscarTodos()
        {
            return await _servicosPrestadosRepositorio.BuscarTodos();
        }
    }
}
