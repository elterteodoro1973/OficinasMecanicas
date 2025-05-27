using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OficinasMecanicas.Aplicacao.Interfaces;
using OficinasMecanicas.Dominio.Interfaces.Repositorios;
using System.Threading.Tasks;


namespace WebServicoAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
               "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
           };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IUsuarioRepositorio _usuarioRepositorio;
        private readonly IUsuarioAppServico _usuarioAppServico;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IUsuarioRepositorio usuarioRepositorio, IUsuarioAppServico usuarioAppServico)
        {
            _logger = logger;
            _usuarioRepositorio = usuarioRepositorio;
            _usuarioAppServico = usuarioAppServico;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public async Task<IEnumerable<WeatherForecast>> Get()
        {
            var usuarios = await _usuarioAppServico.BuscarPorEmail("elters.teodoro@gmail.com");

            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
