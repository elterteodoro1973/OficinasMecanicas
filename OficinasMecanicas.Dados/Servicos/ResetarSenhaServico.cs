using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using OficinasMecanicas.Dominio.DTO;
using OficinasMecanicas.Dominio.Entidades;
using OficinasMecanicas.Dominio.Interfaces;
using OficinasMecanicas.Dominio.Interfaces.Repositorios;
using OficinasMecanicas.Dominio.Interfaces.Servicos;
using OficinasMecanicas.Dominio.Notificacoes;

namespace OficinasMecanicas.Dados.Servicos
{
    public class ResetarSenhaServico : BaseServico<ResetarSenha>, IResetarSenhaServico
    {
        private readonly IHttpContextAccessor _httpContext;
        private readonly IResetarSenhaRepositorio _resetarSenhaRepositorio;
        private INotificador _notificador;
        public  ResetarSenhaServico(IHttpContextAccessor httpContext,
                IResetarSenhaRepositorio resetarSenhaRepositorio,
                INotificador notificador) : base(notificador)
        {
            _httpContext = httpContext;
            _resetarSenhaRepositorio = resetarSenhaRepositorio;
            _notificador = notificador;           
        }

        public async Task Adicionar(ResetarSenha resetarSenha)
        { 
            try
            {
                await _resetarSenhaRepositorio.Adicionar(resetarSenha);                
            }
            catch (Exception ex)
            {               
                _notificador.Adicionar(new Notificacao("Erro ao adicionar o Usuario !" + ex.Message));
            }
        }

        public async Task Atualizar(ResetarSenha resetarSenha)
        {            
            try
            {
                await _resetarSenhaRepositorio.Atualizar(resetarSenha);                
            }
            catch (Exception ex)
            {               
                _notificador.Adicionar(new Notificacao("Erro ao excluir o resete de senha !"));
            }
        }

        public async Task Excluir(Guid resetarSenhaId)
        {
            var resetarSenhaDB = await _resetarSenhaRepositorio.BuscarResetarSenhaPorId(resetarSenhaId);
            try
            {
                await _resetarSenhaRepositorio.Excluir(resetarSenhaId);                
            }
            catch (Exception ex)            {
                
                _notificador.Adicionar(new Notificacao("Erro ao excluir o resete de senha !"));
            }
        }

        public async Task<ResetarSenha?> BuscarResetarSenhaPorToken(string token)
        {
            var resetarSenhaDB = await _resetarSenhaRepositorio.BuscarResetarSenhaPorToken(token);
            return resetarSenhaDB;
        }
    }
}