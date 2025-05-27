﻿namespace OficinasMecanicas.Dominio.DTO
{
    public class EmailConfiguracao
    {
        public string Email { get; set; } = null!;
        public string Senha { get; set; } = null!;
        public string EnderecoSMTP { get; set; } = null!;
        public string BaseURL { get; set; } = null!;
        public int Porta { get; set; }
        public bool UsarSSL { get; set; }
    }
}
