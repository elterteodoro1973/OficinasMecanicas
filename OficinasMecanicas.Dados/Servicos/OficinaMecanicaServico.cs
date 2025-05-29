using Microsoft.AspNetCore.Http;
using OficinasMecanicas.Dominio.Entidades;
using OficinasMecanicas.Dominio.Entidades.Validacoes.Oficina;
using OficinasMecanicas.Dominio.Interfaces;
using OficinasMecanicas.Dominio.Interfaces.Repositorios;
using OficinasMecanicas.Dominio.Interfaces.Servicos;
using OficinasMecanicas.Dominio.Notificacoes;

namespace OficinasMecanicas.Dados.Servicos
{  
    public class OficinaMecanicaServico : BaseServico<OficinaMecanica>, IOficinaMecanicaServico
    {
        private readonly IHttpContextAccessor _httpContext;
        private readonly IOficinaMecanicaRepositorio _oficinaMecanicaRepositorio;        
        private readonly INotificador _notificador;       
                
        public OficinaMecanicaServico(
            IHttpContextAccessor httpContext,
            IOficinaMecanicaRepositorio oficinaMecanicaRepositorio,            
            INotificador notificador) : base(notificador)
        {
            _httpContext = httpContext;
            _oficinaMecanicaRepositorio = oficinaMecanicaRepositorio;            
            _notificador = notificador;
        }
        public async Task<OficinaMecanica?> Adicionar(OficinaMecanica oficina)
        {
            try
            {
               oficina.Id = Guid.NewGuid();
               await _ValidarInclusao(oficina);
               if (_notificador.TemNotificacao()) return null;               
               await _oficinaMecanicaRepositorio.Adicionar(oficina);
               return oficina;
            }
            catch (Exception ex)
            {
                _notificador.Adicionar(new Notificacao("Erro ao adicionar oficina !" + ex.Message));
                return oficina;
            }
        }

        public async Task Atualizar(Guid id ,OficinaMecanica oficina)
        {            
            await _ValidarEdicao(oficina);

            if (_notificador.TemNotificacao()) return;

            var oficinaDB = await _oficinaMecanicaRepositorio.BuscarPorId(id);

            try
            {
                oficinaDB.Nome = oficina.Nome;
                oficinaDB.Endereco = oficina.Endereco;
                oficinaDB.Servicos = oficina.Servicos;
                await _oficinaMecanicaRepositorio.Atualizar(oficinaDB);
            }
            catch (Exception ex)
            {
                _notificador.Adicionar(new Notificacao("Erro ao editar a oficina !" + ex.Message));
            }
        }

        public async Task Excluir(Guid id)
        {
            await _ValidarExclusao(id);

            if (_notificador.TemNotificacao()) return;

            var oficinaDB = await _oficinaMecanicaRepositorio.BuscarPorId(id);
            if (oficinaDB == null)
            {
                _notificador.Adicionar(new Notificacao("Oficiona não encontrada!"));
                return;
            }

            try
            {
                await _oficinaMecanicaRepositorio.Excluir(id);
            }
            catch (Exception ex)
            {
                _notificador.Adicionar(new Notificacao("Erro ao excluir a oficiona !"));
            }
        }

        private async Task _ValidarInclusao(OficinaMecanica oficina)
        {
            if (!ExecutarValidacao<CadastrarEditarOficinaValidacao, OficinaMecanica>(new CadastrarEditarOficinaValidacao(true), oficina)) return;

            if (_notificador.TemNotificacao()) return;

            if (await _oficinaMecanicaRepositorio.NomePrincipalJaCadastrado(oficina.Nome, null))
                _notificador.Adicionar(new Notificacao("Nome do usuário já cadastrado !"));

        }
        private async Task _ValidarEdicao(OficinaMecanica oficina)
        {
            if (!ExecutarValidacao<CadastrarEditarOficinaValidacao, OficinaMecanica>(new CadastrarEditarOficinaValidacao(true), oficina)) return;

            if (_notificador.TemNotificacao()) return;

            if (await _oficinaMecanicaRepositorio.NomePrincipalJaCadastrado(oficina.Nome, oficina.Id))
                _notificador.Adicionar(new Notificacao("Nome do usuário já cadastrado !"));

        }
        private async Task _ValidarExclusao(Guid id)
        {
            if (id == Guid.Empty)
            {
                _notificador.Adicionar(new Notificacao("Identificador da oficina obrigatório !"));
            }
        }
         
        public async Task<IEnumerable<OficinaMecanica>> BuscarTodos() => await _oficinaMecanicaRepositorio.BuscarTodos();
        public async Task<OficinaMecanica?> BuscarPorId(Guid id) => await _oficinaMecanicaRepositorio.BuscarPorId(id);        
        public async Task<OficinaMecanica?> BuscarPorNome(string nome) => await _oficinaMecanicaRepositorio.BuscarPorNome(nome);
        public async Task<bool> NomePrincipalJaCadastrado(string nome, Guid? id) => await _oficinaMecanicaRepositorio.NomePrincipalJaCadastrado(nome, id);
        
    }

}
