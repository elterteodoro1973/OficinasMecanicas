using AutoMapper;
using OficinasMecanicas.Aplicacao.DTO.Agenda;
using OficinasMecanicas.Aplicacao.DTO.Oficinas;
using OficinasMecanicas.Aplicacao.DTO.Usuarios;
using OficinasMecanicas.Dominio.Entidades;

namespace OficinasMecanicas.Aplicacao.Parsers
{
    public class MapeamentoEntidadeParaDTO : Profile
    {
        public MapeamentoEntidadeParaDTO()
        {
            // Mapear usuarios
            CreateMap<Usuarios, UsuariosTelaInicialDTO>()
                .ForMember(c => c.Id, m => m.MapFrom(c => c.Id))                
                .ForMember(c => c.Nome, m => m.MapFrom(c => c.Username))
                .ForMember(c => c.Email, m => m.MapFrom(c => c.Email));

            CreateMap<Usuarios, LoginUsuarioDTO>()
                .ForMember(c => c.Id, m => m.MapFrom(c => c.Id))
                .ForMember(c => c.Username, m => m.MapFrom(c => c.Username))
                .ForMember(c => c.Email, m => m.MapFrom(c => c.Email));

            CreateMap<Usuarios, CadastrarUsuarioDTO>()                
                .ForMember(c => c.Nome, m => m.MapFrom(c => c.Username))
                .ForMember(c => c.Email, m => m.MapFrom(c => c.Email));

            CreateMap<Usuarios, EditarUsuarioDTO>()
                .ForMember(c => c.Id, m => m.MapFrom(c => c.Id))
                .ForMember(c => c.Nome, m => m.MapFrom(c => c.Username))
                .ForMember(c => c.Email, m => m.MapFrom(c => c.Email));

            // Mapear Oficinas
            CreateMap<OficinaMecanica, OficinasTelaInicialDTO>()
                .ForMember(c => c.Id, m => m.MapFrom(c => c.Id))
               .ForMember(c => c.Nome, m => m.MapFrom(c => c.Nome))
               .ForMember(c => c.Endereco, m => m.MapFrom(c => c.Endereco))
               .ForMember(c => c.Servicos, m => m.MapFrom(c => c.Servicos));

            CreateMap<OficinaMecanica, CadastrarOficinaDTO>()
               .ForMember(c => c.Nome, m => m.MapFrom(c => c.Nome))
               .ForMember(c => c.Endereco, m => m.MapFrom(c => c.Endereco))
               .ForMember(c => c.Servicos, m => m.MapFrom(c => c.Servicos));

            CreateMap<OficinaMecanica, EditarOficinaDTO>()
                .ForMember(c => c.Id, m => m.MapFrom(c => c.Id))
                .ForMember(c => c.Nome, m => m.MapFrom(c => c.Nome))
               .ForMember(c => c.Endereco, m => m.MapFrom(c => c.Endereco))
               .ForMember(c => c.Servicos, m => m.MapFrom(c => c.Servicos));

            // Mapear Agendamento
            CreateMap<AgendamentoVisita, CadastrarAgendamentoVisitaDTO>()
               .ForMember(c => c.IdUsuario, m => m.MapFrom(c => c.IdUsuario))
               .ForMember(c => c.IdOficina, m => m.MapFrom(c => c.IdOficina))
               .ForMember(c => c.DataHora, m => m.MapFrom(c => c.DataHora))
               .ForMember(c => c.Descricao, m => m.MapFrom(c => c.Descricao));

            CreateMap<AgendamentoVisita, EditarAgendamentoVisitaDTO>()
                .ForMember(c => c.Id, m => m.MapFrom(c => c.Id))
               .ForMember(c => c.IdUsuario, m => m.MapFrom(c => c.IdUsuario))
               .ForMember(c => c.IdOficina, m => m.MapFrom(c => c.IdOficina))
               .ForMember(c => c.DataHora, m => m.MapFrom(c => c.DataHora))
               .ForMember(c => c.Descricao, m => m.MapFrom(c => c.Descricao));
        }
    }
}
