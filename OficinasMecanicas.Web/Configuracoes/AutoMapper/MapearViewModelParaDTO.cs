using AutoMapper;
using OficinasMecanicas.Aplicacao.DTO.Agenda;
using OficinasMecanicas.Aplicacao.DTO.Oficinas;
using OficinasMecanicas.Aplicacao.DTO.Usuarios;
using OficinasMecanicas.Web.ViewModels.Agenda;
using OficinasMecanicas.Web.ViewModels.Usuarios;

namespace OficinasMecanicas.Web.Configuracoes.AutoMapper
{
    public class MapearViewModelParaDTO : Profile
    {
        public MapearViewModelParaDTO()
        {
            // Mapear Usuarios
            CreateMap<CadastrarEditarUsuarioViewModel, CadastrarUsuarioDTO>();
            CreateMap<CadastrarEditarUsuarioViewModel, EditarUsuarioDTO>();
            CreateMap<CadastrarNovaSenhaViewModel, CadastrarNovaSenhaDTO>()
               .ForMember(c => c.Email, m => m.MapFrom(c => c.Email))
               .ForMember(c => c.Token, m => m.MapFrom(c => c.Token))
               .ForMember(c => c.Senha, m => m.MapFrom(c => c.Senha))
               .ForMember(c => c.ConfirmarSenha, m => m.MapFrom(c => c.ConfirmarSenha));

            // Mapear Oficinas
            CreateMap<CadastrarEditarOficinaViewModel,CadastrarOficinaDTO>();
            CreateMap<CadastrarEditarOficinaViewModel,EditarOficinaDTO>();

            // Mapear agendamento
            CreateMap<CadastrarEditarAgendamentoVisitaViewModel, CadastrarAgendamentoVisitaDTO>();
            CreateMap<CadastrarEditarAgendamentoVisitaViewModel, EditarAgendamentoVisitaDTO>();
        }
    }
}
