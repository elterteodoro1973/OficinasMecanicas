using AutoMapper;
using OficinasMecanicas.Aplicacao.DTO.Agenda;
using OficinasMecanicas.Aplicacao.DTO.Oficinas;
using OficinasMecanicas.Aplicacao.DTO.Usuarios;
using OficinasMecanicas.Web.ViewModels.Agenda;
using OficinasMecanicas.Web.ViewModels.Oficinas;
using OficinasMecanicas.Web.ViewModels.Usuarios;

namespace OficinasMecanicas.Web.Configuracoes.AutoMapper
{
    public class MapearDTOParaViewModel : Profile
    {
        public MapearDTOParaViewModel()
        {
            // Mapear usuarios
            CreateMap<UsuariosTelaInicialDTO, UsuariosViewModel>();
            CreateMap<CadastrarUsuarioDTO, CadastrarEditarUsuarioViewModel>();
            CreateMap<EditarUsuarioDTO, CadastrarEditarUsuarioViewModel>();
            CreateMap<CadastrarNovaSenhaDTO, CadastrarNovaSenhaViewModel>()
               .ForMember(c => c.Email, m => m.MapFrom(c => c.Email))
               .ForMember(c => c.Token, m => m.MapFrom(c => c.Token))
               .ForMember(c => c.Senha, m => m.MapFrom(c => c.Senha))
               .ForMember(c => c.ConfirmarSenha, m => m.MapFrom(c => c.ConfirmarSenha));

            // Mapear Oficinas
            CreateMap<OficinasTelaInicialDTO, OficinasMecanicasViewModel>();
            CreateMap<CadastrarOficinaDTO, CadastrarEditarOficinaViewModel>();
            CreateMap<EditarOficinaDTO, CadastrarEditarOficinaViewModel>();

            // Mapear agendamento
            CreateMap<AgendamentosVisitasTelaInicialDTO, AgendamentoVisitaViewModel>();
            CreateMap<CadastrarAgendamentoVisitaDTO, CadastrarEditarAgendamentoVisitaViewModel>();
            CreateMap<EditarAgendamentoVisitaDTO, CadastrarEditarAgendamentoVisitaViewModel>();

        }   
    }
}
