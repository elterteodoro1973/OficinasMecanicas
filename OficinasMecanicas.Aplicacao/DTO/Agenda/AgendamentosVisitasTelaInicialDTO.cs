namespace OficinasMecanicas.Aplicacao.DTO.Agenda
{
    public class AgendamentosVisitasTelaInicialDTO
    {
        public Guid? Id { get; set; }
        public Guid IdUsuario { get; set; }
        public string NomeUsuario { get; set; }
        public Guid IdOficina { get; set; }
        public string NomeOficina { get; set; }
        public DateTime DataHora { get; set; }
        public string Descricao { get; set; } = null!;

    }
}
