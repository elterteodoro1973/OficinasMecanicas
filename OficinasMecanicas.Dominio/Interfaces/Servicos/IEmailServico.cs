using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OficinasMecanicas.Dominio.Interfaces.Servicos
{
    public interface IEmailServico
    {
        Task Enviar(string destinatario, string assunto, string email, IList<string>? listaEmailCopias = null);
        Task <String> GetCredenciasPrimeiroAcesso(String basePath, String nome, String link);
        Task <String> GetTextoResetSenha(String basePath, String nome, String link);
    }
}
