using OficinasMecanicas.Aplicacao.DTO.Oficinas;
using OficinasMecanicas.Aplicacao.DTO.Usuarios;
using OficinasMecanicas.Aplicacao.Model;
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
        Task<EditarOficinaDTO?> Atualizar(Guid id, CadastrarOficinaDTO? dto);
        Task<bool> Excluir(Guid id);
        Task<IList<OficinasTelaInicialDTO>> ListarOficionasTelaInicial(string? filtro);
        Task<IEnumerable<OficinasTelaInicialDTO>> BuscarTodos();        
        Task<EditarOficinaDTO?> BuscarPorId(Guid id);
        Task<EditarOficinaDTO?> BuscarPorNome(string nome);
        Task<bool> NomePrincipalJaCadastrado(string nome, Guid? id);
        
        Task<Resposta<IList<OficinasTelaInicialDTO>>> GetWebApi(string endPoint);
        Task<Resposta<EditarOficinaDTO>> PostWebApi<T1>(T1 model, string endPoint);
        Task<Resposta<EditarOficinaDTO>> GetWebApiById(Guid id, string endPoint);
        Task<Resposta<EditarOficinaDTO>> PutWebApi(Guid id, CadastrarOficinaDTO model, string endPoint);
        Task<Resposta<EditarOficinaDTO>> DeleteWebApi(Guid id, string endPoint);

    }
}
