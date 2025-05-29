using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using OficinasMecanicas.Aplicacao.DTO.Oficinas;
using OficinasMecanicas.Aplicacao.DTO.Usuarios;
using OficinasMecanicas.Aplicacao.Interfaces;
using OficinasMecanicas.Dominio.Entidades;
using OficinasMecanicas.Dominio.Interfaces;
using OficinasMecanicas.Dominio.Interfaces.Servicos;
using OficinasMecanicas.Dominio.Notificacoes;

namespace OficinasMecanicas.Aplicacao.Servicos
{
    public  class OficinaAppServico : IOficinaAppServico
    {
        private readonly IOficinaMecanicaServico _oficinaMecanicaServico;        
        private readonly INotificador _notificador;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContext;
        public OficinaAppServico(IHttpContextAccessor httpContext, IMapper mapper, INotificador notificador, 
                                  IOficinaMecanicaServico oficinaMecanicaServico, IConfiguration configuration)
        {
            _httpContext = httpContext;            
            _mapper = mapper;
            _notificador = notificador;
            _oficinaMecanicaServico = oficinaMecanicaServico;
            _configuration = configuration;
        }

        
        public async Task<IList<OficinasTelaInicialDTO>> ListarOficionasTelaInicial(string? filtro)
        {
            var dtos = _mapper.Map<IList<OficinasTelaInicialDTO>>(await _oficinaMecanicaServico.BuscarTodos());

            if (!string.IsNullOrEmpty(filtro) && !string.IsNullOrWhiteSpace(filtro))
            {
                dtos = dtos.Where(c => c.Nome.ToUpper().Contains(filtro.ToUpper()) || c.Endereco.ToUpper().Contains(filtro.ToUpper()) || c.Servicos.ToUpper().Contains(filtro.ToUpper())).ToList();
            }

            return dtos;
        }

        public async Task<EditarOficinaDTO> Adicionar(CadastrarOficinaDTO dto)
        {
            var oficina = _mapper.Map<OficinaMecanica>(dto);
            var novoUsuario = await _oficinaMecanicaServico.Adicionar(oficina);
            return _mapper.Map<EditarOficinaDTO>(oficina);
        }        

        public async Task<EditarOficinaDTO?> Atualizar(Guid id,  CadastrarOficinaDTO? dto)
        {            
            if (dto == null || id == null )
            {
                _notificador.Adicionar(new Notificacao("Oficina inválida !"));
                return null;
            }
            
            var oficinaEnvio = await _oficinaMecanicaServico.BuscarPorId(id);
            if (oficinaEnvio == null )
            {
                _notificador.Adicionar(new Notificacao("Oficina não encontrada !"));
                return null;
            }

            var oficina = _mapper.Map<OficinaMecanica>(dto);
            oficina.Id = id;
            await _oficinaMecanicaServico.Atualizar(id,oficina);
            return _mapper.Map<EditarOficinaDTO>(oficina);
        }


        public async Task<bool> Excluir(Guid id)
        {
            await _oficinaMecanicaServico.Excluir(id);
            return !_notificador.TemNotificacao();
        }

        public async Task<EditarOficinaDTO?> BuscarPorId(Guid id)
        {
            var dtos = _mapper.Map<EditarOficinaDTO>(await _oficinaMecanicaServico.BuscarPorId(id));
            return dtos;
        }

        public async Task<EditarOficinaDTO?> BuscarPorNome(string nome)
        {
            var dtos = _mapper.Map<EditarOficinaDTO>(await _oficinaMecanicaServico.BuscarPorNome(nome));
            return dtos;
        }

        public async Task<IEnumerable<OficinasTelaInicialDTO>> BuscarTodos()
        {
            var dtos = _mapper.Map<IList<OficinasTelaInicialDTO>>(await _oficinaMecanicaServico.BuscarTodos());         

            return dtos;
        }

        public async Task<bool> NomePrincipalJaCadastrado(string nome, Guid? id)
        {
            return await _oficinaMecanicaServico.NomePrincipalJaCadastrado(nome, id);
        }
    }
}
