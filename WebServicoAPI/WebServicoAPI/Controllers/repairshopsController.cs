using Microsoft.AspNetCore.Mvc;
using OficinasMecanicas.Aplicacao.DTO.Oficinas;
using OficinasMecanicas.Aplicacao.Model;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebServicoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class repairshopsController : ControllerBase
    {
        [HttpPost] //Cria nova oficina
        [ProducesResponseType(typeof(Resposta<EditarOficinaDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Resposta<object>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Post([FromBody] CadastrarOficinaDTO  oficina)
        {
            var oficinaDB = new EditarOficinaDTO { Id = Guid.NewGuid(), Nome = "D4", Endereco = "Rua D, 14" };
            return Ok(new Resposta<object>
            {
                sucesso = true,
                mensagem = "Post realizado com sucesso",
                dados = oficinaDB
            });
        }


        [HttpPut("{id}")] //Atualiza dados da oficina
        [ProducesResponseType(typeof(Resposta<EditarOficinaDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Resposta<object>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Put(int id, [FromBody] EditarOficinaDTO oficina)
        {
            var oficinaDB = new EditarOficinaDTO { Id = Guid.NewGuid(), Nome = "E", Endereco = "Rua E, 15" };
            return Ok(new Resposta<object> // Fixed type mismatch here
            {
                sucesso = true,
                mensagem = "Put realizado com sucesso",
                dados = oficinaDB
            });
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(Resposta<EditarOficinaDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Resposta<object>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Delete(int id)//Remove uma oficina
        {
            var oficina = new EditarOficinaDTO { Id = Guid.NewGuid(), Nome = "F6", Endereco = "Rua F, 16" };
            return Ok(new Resposta<object> // Fixed type mismatch here
            {
                sucesso = true,
                mensagem = "Delete realizado com sucesso",
                dados = oficina
            });
        }

        [HttpGet]
        [ProducesResponseType(typeof(Resposta<List<EditarOficinaDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Resposta<object>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Get()//Lista todas as oficinas
        {
            List<EditarOficinaDTO> lista = new List<EditarOficinaDTO>() {
                    new EditarOficinaDTO{ Id= Guid.NewGuid(),Nome = "A1",Endereco= "Rua A, 12" },
                    new EditarOficinaDTO{ Id= Guid.NewGuid(),Nome = "B1",Endereco= "Rua B, 14" },
                };

            return Ok(new Resposta<List<EditarOficinaDTO>>
            {
                sucesso = true,
                mensagem = "Get realizado com sucesso",
                dados = lista
            });
        }



        [HttpGet("{id}")]//Detalhes de uma oficina
        [ProducesResponseType(typeof(Resposta<EditarOficinaDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Resposta<object>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Get(int id)
        {
            var oficina = new EditarOficinaDTO { Id = Guid.NewGuid(), Nome = "C3", Endereco = "Rua C, 13" };
            return Ok(new Resposta<EditarOficinaDTO> 
            {
                sucesso = true,
                mensagem = "Get realizado com sucesso",
                dados = oficina
            });
        }

        

        
    }
}
