using AutoMapper;
using OficinasMecanicas.Aplicacao.DTO;
using OficinasMecanicas.Aplicacao.DTO.Usuarios;
using OficinasMecanicas.Web.ViewModels;
using OficinasMecanicas.Web.ViewModels.Perfis;
using OficinasMecanicas.Web.ViewModels.Usuarios;

namespace OficinasMecanicas.Web.Configuracoes.AutoMapper
{
    public class MapearDTOParaViewModel : Profile
    {
        public MapearDTOParaViewModel()
        {
            CreateMap<UsuariosTelaInicialDTO, UsuariosViewModel>();
            CreateMap<CadastrarEditarUsuarioDTO, CadastrarEditarUsuarioViewModel>();
            CreateMap<CadastrarNovaSenhaDTO, CadastrarNovaSenhaViewModel>()
               .ForMember(c => c.Email, m => m.MapFrom(c => c.Email))
               .ForMember(c => c.Token, m => m.MapFrom(c => c.Token))
               .ForMember(c => c.Senha, m => m.MapFrom(c => c.Senha))
               .ForMember(c => c.ConfirmarSenha, m => m.MapFrom(c => c.ConfirmarSenha));
        }   
    }
}
