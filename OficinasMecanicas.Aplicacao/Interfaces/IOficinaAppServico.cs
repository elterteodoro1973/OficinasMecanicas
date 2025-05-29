using OficinasMecanicas.Aplicacao.DTO.Oficinas;
using OficinasMecanicas.Aplicacao.DTO.Usuarios;
using OficinasMecanicas.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OficinasMecanicas.Aplicacao.Interfaces
{
    public interface IOficinaAppServico
    {
        Task<EditarOficinaDTO?> Adicionar(CadastrarOficinaDTO dto);
        Task<EditarOficinaDTO?> Atualizar(EditarOficinaDTO? dto);
        Task<bool> Excluir(Guid id);

        Task<IList<OficinasTelaInicialDTO>> ListarOficionasTelaInicial(string? filtro);
        Task<IEnumerable<OficinasTelaInicialDTO>> BuscarTodos();
        
        Task<EditarOficinaDTO?> BuscarPorId(Guid id);
        Task<EditarOficinaDTO?> BuscarPorNome(string nome);
        Task<bool> NomePrincipalJaCadastrado(string nome, Guid? id);
    }
}
