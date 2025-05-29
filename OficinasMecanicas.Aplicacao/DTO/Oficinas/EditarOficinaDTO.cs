using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OficinasMecanicas.Aplicacao.DTO.Oficinas
{
    public class EditarOficinaDTO
    {
        public Guid? Id { get; set; }
        public string Nome { get; set; } = null!;
        public string Endereco { get; set; } = null!;
        public string Servicos { get; set; } = null!;
    }
}
