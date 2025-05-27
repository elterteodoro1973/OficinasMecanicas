using AutoMapper;
using OficinasMecanicas.Aplicacao.DTO.Usuarios;
using OficinasMecanicas.Dominio.Entidades;

namespace OficinasMecanicas.Aplicacao.Parsers
{
    public class MapeamentoDTOParaEntidade : Profile
    {
        public MapeamentoDTOParaEntidade()
        {
            CreateMap<UsuariosTelaInicialDTO, Usuarios>()
                .ForMember(c => c.Id, m => m.MapFrom(c => c.Id))                
                .ForMember(c => c.Username, m => m.MapFrom(c => c.Nome))
                .ForMember(c => c.Email, m => m.MapFrom(c => c.Email));


            CreateMap<LoginUsuarioDTO,Usuarios>()
                .ForMember(c => c.Id, m => m.MapFrom(c => c.Id))
                .ForMember(c => c.Username, m => m.MapFrom(c => c.Username))
                .ForMember(c => c.Email, m => m.MapFrom(c => c.Email));

            CreateMap<CadastrarEditarUsuarioDTO,Usuarios>()
               .ForMember(c => c.Id, m => m.MapFrom(c => c.Id))
               .ForMember(c => c.Username, m => m.MapFrom(c => c.Nome))               
               .ForMember(c => c.Email, m => m.MapFrom(c => c.Email));            
        }
    }
}
