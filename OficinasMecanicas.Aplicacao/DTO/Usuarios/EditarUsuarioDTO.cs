using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OficinasMecanicas.Aplicacao.DTO.Usuarios
{
    public class EditarUsuarioDTO
    {
        public Guid? Id { get; set; }
        public string Nome { get; set; } = null!;        
        public string Email { get; set; } = null!;
        public string? Senha { get; set; } = null!;
    }
}
