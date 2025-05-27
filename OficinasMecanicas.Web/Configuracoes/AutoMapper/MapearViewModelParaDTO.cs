using AutoMapper;
using OficinasMecanicas.Aplicacao.DTO;
using OficinasMecanicas.Aplicacao.DTO.Perfis;
using OficinasMecanicas.Aplicacao.DTO.Usuarios;
using OficinasMecanicas.Web.ViewModels.Perfis;
using OficinasMecanicas.Web.ViewModels.Usuarios;

namespace OficinasMecanicas.Web.Configuracoes.AutoMapper
{
    public class MapearViewModelParaDTO : Profile
    {
        public MapearViewModelParaDTO()
        {
            CreateMap<CadastrarEditarUsuarioViewModel, CadastrarEditarUsuarioDTO>();
            CreateMap<CadastrarNovaSenhaViewModel, CadastrarNovaSenhaDTO>()
               .ForMember(c => c.Email, m => m.MapFrom(c => c.Email))
               .ForMember(c => c.Token, m => m.MapFrom(c => c.Token))
               .ForMember(c => c.Senha, m => m.MapFrom(c => c.Senha))
               .ForMember(c => c.ConfirmarSenha, m => m.MapFrom(c => c.ConfirmarSenha));

            CreateMap<PerfilViewModel, PerfilDTO>()
                .ForMember(c => c.Id, m =>
                {
                    m.PreCondition(c => c.Id.HasValue);
                    m.MapFrom(c => c.Id);
                }).
                ForMember(c => c.Descricao, m => m.MapFrom(c => c.Descricao))
                .ForMember(c => c.Nome, m => m.MapFrom(c => c.Nome))
                .ForMember(c => c.Administrador, m => m.MapFrom(c => c.Administrador))
                .ForMember(c => c.Claims, m => m.Ignore());
            
            CreateMap<CadastrarPerfilUsuarioViewModel, CadastrarPerfilUsuarioDTO>()
                .ForMember(c => c.PerfilId, m => m.MapFrom(c => c.PerfilId))
                .ForMember(c => c.UsuarioId, m => m.MapFrom(c => c.UsuarioId)); 

            CreateMap<LogPerfilViewModel, LogTransacoesDTO>()
               .ForMember(c => c.Data, m => m.MapFrom(c => c.Data))
               .ForMember(c => c.EntidadeId, m => m.MapFrom(c => c.EntidadeId))
               .ForMember(c => c.UsuarioId, m => m.MapFrom(c => c.UsuarioId))
               .ForMember(c => c.Dados, m => m.MapFrom(c => c.Dados));
        }
    }
}
