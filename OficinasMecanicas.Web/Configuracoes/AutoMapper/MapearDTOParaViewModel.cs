﻿using AutoMapper;
using OficinasMecanicas.Aplicacao.DTO;
using OficinasMecanicas.Aplicacao.DTO.Perfis;
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

            CreateMap<LogTransacoesDTO, LogViewModel>()
               .ForMember(c => c.DataFormatada, m =>
               {
                   m.MapFrom(c => c.Data.ToString("dd/MM/yyyy HH:mm:ss"));
               });

            CreateMap<SelectOptionDTO, SelectOptionViewModel>();

            CreateMap<PerfilDTO, PerfilViewModel>();

            CreateMap<PerfilDTO, PerfilViewModel>()
                .ForMember(c => c.Claims, m => m.Ignore());

            CreateMap<LogTransacoesDTO, LogPerfilViewModel>()
             .ForMember(c => c.Data, m => m.MapFrom(c => c.Data))
             .ForMember(c => c.EntidadeId, m => m.MapFrom(c => c.EntidadeId))
             .ForMember(c => c.UsuarioId, m => m.MapFrom(c => c.UsuarioId))
             .ForMember(c => c.Dados, m => m.MapFrom(c => c.Dados));



        }   
    }
}
