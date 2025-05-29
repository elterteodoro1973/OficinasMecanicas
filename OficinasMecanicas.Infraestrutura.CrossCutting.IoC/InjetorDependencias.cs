using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using OficinasMecanicas.Aplicacao.Interfaces;
using OficinasMecanicas.Aplicacao.Parsers;
using OficinasMecanicas.Aplicacao.Servicos;
using OficinasMecanicas.Dados.Repositorios;
using OficinasMecanicas.Dados.Servicos;
using OficinasMecanicas.Dominio.Interfaces;
using OficinasMecanicas.Dominio.Interfaces.Repositorios;
using OficinasMecanicas.Dominio.Interfaces.Servicos;
using OficinasMecanicas.Dominio.Notificacoes;
using System.Text;

namespace OficinasMecanicas.Infraestrutura.CrossCutting.IoC
{
    public static class InjetorDependencias
    {
        public static void ConfigurarContextosEFCore(this IServiceCollection services, string conexao)
        {
           services.AddDbContext<Dados.Contexto.DbContexto>(o => o.UseSqlServer(conexao).EnableSensitiveDataLogging(), ServiceLifetime.Scoped);
        }
        public static void ConfigurarAutoMapper(this IServiceCollection services)
        {
           services.AddAutoMapper(typeof(MapeamentoEntidadeParaDTO), typeof(MapeamentoDTOParaEntidade));
           services.AddAutoMapper(typeof(MapeamentoDTOParaEntidade), typeof(MapeamentoEntidadeParaDTO));
        }

        public static void ConfigurarServicosERepositorios(this IServiceCollection services)
        {
            ////Repositorios             
            services.AddScoped<IAgendamentoVisitaRepositorio, AgendamentoVisitaRepositorio>();
            services.AddScoped<IOficinaMecanicaRepositorio, OficinaMecanicaRepositorio>();
            services.AddScoped<IResetarSenhaRepositorio, ResetarSenhaRepositorio>();
            services.AddScoped<IServicosPrestadosRepositorio, ServicosPrestadosRepositorio>();
            services.AddScoped<IUsuarioRepositorio, UsuarioRepositorio>();
            
            ////Servicos de Dominio    
            services.AddScoped<IAgendamentoVisitaServico,AgendamentoVisitaServico>();
            services.AddScoped<IOficinaMecanicaServico,OficinaMecanicaServico>();            
            services.AddScoped<IResetarSenhaServico,ResetarSenhaServico>();
            services.AddScoped<IServicosPrestadosServico, ServicosPrestadosServico>();
            services.AddScoped<IUsuariosServico,UsuarioServico>();
            
            ////Servicos de Aplicacao
            services.AddScoped<IAgendaVisitaAppServico,AgendaVisitaAppServico>();
            services.AddScoped<IOficinaAppServico,OficinaAppServico>();
            services.AddScoped<IUsuarioAppServico,UsuarioAppServico>();            
            
            //Outros Servicos
            services.AddScoped<INotificador,Notificador>();            
            services.AddSingleton<IHttpContextAccessor,HttpContextAccessor>();
            services.AddScoped<IEmailServico,EmailServico>();            
        }

        public static void ConfigurarMensagensMVC(this IServiceCollection services)
        {            
            services.AddControllersWithViews().AddMvcOptions(o =>
            {
                o.ModelBindingMessageProvider.SetAttemptedValueIsInvalidAccessor((x, y) => "O valor preenchido é inválido para este campo.");
                o.ModelBindingMessageProvider.SetMissingBindRequiredValueAccessor(x => "Este campo precisa ser preenchido.");
                o.ModelBindingMessageProvider.SetMissingKeyOrValueAccessor(() => "Este campo precisa ser preenchido.");
                o.ModelBindingMessageProvider.SetMissingRequestBodyRequiredValueAccessor(() => "É necessário que o body na requisição não esteja vazio.");
                o.ModelBindingMessageProvider.SetNonPropertyAttemptedValueIsInvalidAccessor(x => "O valor preenchido é inválido para este campo.");
                o.ModelBindingMessageProvider.SetNonPropertyUnknownValueIsInvalidAccessor(() => "O valor preenchido é inválido para este campo.");
                o.ModelBindingMessageProvider.SetNonPropertyValueMustBeANumberAccessor(() => "O campo deve ser numérico");
                o.ModelBindingMessageProvider.SetUnknownValueIsInvalidAccessor(x => "O valor preenchido é inválido para este campo.");
                o.ModelBindingMessageProvider.SetValueIsInvalidAccessor(x => "O valor preenchido é inválido para este campo.");
                o.ModelBindingMessageProvider.SetValueMustBeANumberAccessor(x => "O campo deve ser numérico.");
                o.ModelBindingMessageProvider.SetValueMustNotBeNullAccessor(x => "Este campo precisa ser preenchido.");
                o.Filters.Add(
                    new ResponseCacheAttribute
                    {
                        NoStore = true,
                        Location = ResponseCacheLocation.None
                    });
                o.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
            });           
        }

        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtSection = configuration.GetSection("jwt");
            var symmetricSecurityKey = configuration["jwt:secretKey"];

            services.AddAuthentication(options => {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    ValidIssuer = configuration["jwt:issuer"],
                    ValidAudience = configuration["jwt:audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(symmetricSecurityKey)),
                    ClockSkew = TimeSpan.Zero,
                };
            });

            return services;
        }

    }
}
