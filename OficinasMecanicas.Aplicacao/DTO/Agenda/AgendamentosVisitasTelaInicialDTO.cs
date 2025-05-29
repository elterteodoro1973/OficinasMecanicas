namespace OficinasMecanicas.Aplicacao.DTO.Agenda
{
    public class AgendamentosVisitasTelaInicialDTO
    {
        public Guid? Id { get; set; }
        public Guid IdUsuario { get; set; }
        public Guid IdOficina { get; set; }
        public DateTime DataHora { get; set; }
        public string Descricao { get; set; } = null!;

    }
}
