namespace OficinasMecanicas.Aplicacao.Model
{
    public class Resposta<T>
    {
        public bool sucesso { get; set; } = false;
        public string mensagem { get; set; }
        public T? dados { get; set; }
        
    }
}
