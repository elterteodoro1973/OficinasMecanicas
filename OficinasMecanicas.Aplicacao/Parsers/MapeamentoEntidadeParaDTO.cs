using AutoMapper;
using OficinasMecanicas.Aplicacao.DTO.Usuarios;
using OficinasMecanicas.Dominio.Entidades;

namespace OficinasMecanicas.Aplicacao.Parsers
{
    public class MapeamentoEntidadeParaDTO : Profile
    {
        public MapeamentoEntidadeParaDTO()
        {
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


        }
    }
}
