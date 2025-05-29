using AutoMapper;
using OficinasMecanicas.Aplicacao.DTO.Agenda;
using OficinasMecanicas.Aplicacao.DTO.Oficinas;
using OficinasMecanicas.Aplicacao.DTO.Usuarios;
using OficinasMecanicas.Dominio.Entidades;

namespace OficinasMecanicas.Aplicacao.Parsers
{
    public class MapeamentoDTOParaEntidade : Profile
    {
        public MapeamentoDTOParaEntidade()
        {
            // Mapear Usuarios
            CreateMap<UsuariosTelaInicialDTO, Usuarios>()
                .ForMember(c => c.Id, m => m.MapFrom(c => c.Id))                
                .ForMember(c => c.Username, m => m.MapFrom(c => c.Nome))
                .ForMember(c => c.Email, m => m.MapFrom(c => c.Email));


            CreateMap<LoginUsuarioDTO,Usuarios>()
                .ForMember(c => c.Id, m => m.MapFrom(c => c.Id))
                .ForMember(c => c.Username, m => m.MapFrom(c => c.Username))
                .ForMember(c => c.Email, m => m.MapFrom(c => c.Email));

            CreateMap<CadastrarUsuarioDTO, Usuarios>()               
               .ForMember(c => c.Username, m => m.MapFrom(c => c.Nome))               
               .ForMember(c => c.Email, m => m.MapFrom(c => c.Email));

            CreateMap<EditarUsuarioDTO, Usuarios>()
               .ForMember(c => c.Id, m => m.MapFrom(c => c.Id))
               .ForMember(c => c.Username, m => m.MapFrom(c => c.Nome))
               .ForMember(c => c.Email, m => m.MapFrom(c => c.Email));

            // Mapear agendamento
            CreateMap<AgendamentosVisitasTelaInicialDTO, AgendamentoVisita>()
                    .ForMember(c => c.Id, m => m.MapFrom(c => c.Id))
                    .ForMember(c => c.IdUsuario, m => m.MapFrom(c => c.IdUsuario))
                    .ForMember(c => c.IdOficina, m => m.MapFrom(c => c.IdOficina))
                    .ForMember(c => c.DataHora, m => m.MapFrom(c => c.DataHora))
                    .ForMember(c => c.Descricao, m => m.MapFrom(c => c.Descricao));           

            CreateMap<CadastrarAgendamentoVisitaDTO, AgendamentoVisita>()               
                    .ForMember(c => c.IdUsuario, m => m.MapFrom(c => c.IdUsuario))
                    .ForMember(c => c.IdOficina, m => m.MapFrom(c => c.IdOficina))
                    .ForMember(c => c.DataHora, m => m.MapFrom(c => c.DataHora))
                    .ForMember(c => c.Descricao, m => m.MapFrom(c => c.Descricao));

            CreateMap<EditarAgendamentoVisitaDTO, AgendamentoVisita>()
                .ForMember(c => c.Id, m => m.MapFrom(c => c.Id))
                .ForMember(c => c.IdUsuario, m => m.MapFrom(c => c.IdUsuario))
                .ForMember(c => c.IdOficina, m => m.MapFrom(c => c.IdOficina))
                .ForMember(c => c.DataHora, m => m.MapFrom(c => c.DataHora))
                .ForMember(c => c.Descricao, m => m.MapFrom(c => c.Descricao));

        }
    }
}
