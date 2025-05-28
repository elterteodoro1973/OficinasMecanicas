using AutoMapper;
using OficinasMecanicas.Aplicacao.DTO.Oficinas;
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

            CreateMap<CadastrarUsuarioDTO, Usuarios>()               
               .ForMember(c => c.Username, m => m.MapFrom(c => c.Nome))               
               .ForMember(c => c.Email, m => m.MapFrom(c => c.Email));

            CreateMap<EditarUsuarioDTO, Usuarios>()
               .ForMember(c => c.Id, m => m.MapFrom(c => c.Id))
               .ForMember(c => c.Username, m => m.MapFrom(c => c.Nome))
               .ForMember(c => c.Email, m => m.MapFrom(c => c.Email));


            CreateMap<OficinasTelaInicialDTO, OficinaMecanica>()
                    .ForMember(c => c.Id, m => m.MapFrom(c => c.Id))
                    .ForMember(c => c.Nome, m => m.MapFrom(c => c.Nome))
                    .ForMember(c => c.Endereco, m => m.MapFrom(c => c.Endereco))
                    .ForMember(c => c.Servicos, m => m.MapFrom(c => c.Servicos));           

            CreateMap<CadastrarOficinaDTO, OficinaMecanica>()
               .ForMember(c => c.Nome, m => m.MapFrom(c => c.Nome))
               .ForMember(c => c.Endereco, m => m.MapFrom(c => c.Endereco))
               .ForMember(c => c.Servicos, m => m.MapFrom(c => c.Servicos));

            CreateMap<EditarOficinaDTO, OficinaMecanica>()
               .ForMember(c => c.Id, m => m.MapFrom(c => c.Id))
               .ForMember(c => c.Nome, m => m.MapFrom(c => c.Nome))
               .ForMember(c => c.Endereco, m => m.MapFrom(c => c.Endereco))
               .ForMember(c => c.Servicos, m => m.MapFrom(c => c.Servicos));
        }
    }
}
