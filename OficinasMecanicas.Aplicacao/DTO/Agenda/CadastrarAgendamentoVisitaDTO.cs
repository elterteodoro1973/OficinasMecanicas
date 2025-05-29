using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OficinasMecanicas.Aplicacao.DTO.Agenda
{
    public class CadastrarAgendamentoVisitaDTO
    { 
        public Guid IdUsuario { get; set; } 
        public Guid IdOficina { get; set; }
        public DateTime DataHora { get; set; } 
        public string Descricao { get; set; } = null!;
    }
}
