namespace OficinasMecanicas.Aplicacao.DTO.Oficinas
{
    public class OficinasTelaInicialDTO
    {
        public Guid Id { get; set; }
        public string Nome { get; set; } = null!;
        public string Endereco { get; set; } = null!;
        public string Servicos { get; set; } = null!;

    }
}
