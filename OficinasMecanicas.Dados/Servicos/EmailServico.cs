using Microsoft.Extensions.Configuration;
using OficinasMecanicas.Dominio.Interfaces.Servicos;
using System.Net;
using System.Net.Mail;

namespace OficinasMecanicas.Dados.Servicos
{
    public class EmailServico : IEmailServico
    {

        private readonly IConfiguration _configuration;

        public EmailServico(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task Enviar(string destinatario, string assunto, string email, IList<string>? listaEmailCopias = null)
        {
            using (var smtpClient = new SmtpClient(_configuration["EmailConfiguracao:EnderecoSMTP"] , 587))
            {
                smtpClient.Credentials = new NetworkCredential(_configuration["EmailConfiguracao:Email"] , _configuration["EmailConfiguracao:Senha"].Trim());
                smtpClient.EnableSsl = true;

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_configuration["EmailConfiguracao:Email"]),  
                    Subject = assunto,
                    Body = email,
                    IsBodyHtml = true
                };

                mailMessage.To.Add(destinatario);

                if (listaEmailCopias != null)
                {
                    foreach (var item in listaEmailCopias)
                    {
                        mailMessage.Bcc.Add(item);
                    }
                }

                await smtpClient.SendMailAsync(mailMessage);
            }
        }

        public async Task<string> GetCredenciasPrimeiroAcesso(string basePath, string nome, string link)
        {
            string arq = $@"{basePath}\assets\html\GetCredenciasAcesso.html";
            string[] Linhas1 = System.IO.File.ReadAllLines(arq);

            string retorno = "";
            foreach (string linha in Linhas1)
            {
                retorno += linha;
            }

            retorno = retorno.Replace("$Nome", nome);
            retorno = retorno.Replace("$Link", link);

            return await Task.FromResult(retorno);
        }

        public async Task<string> GetTextoResetSenha(string basePath, string nome, string link)
        {
            string arq = $@"{basePath}\assets\html\GetTextoResetSenha.html";
            string[] Linhas1 = System.IO.File.ReadAllLines(arq);

            string retorno = "";
            foreach (string linha in Linhas1)
            {
                retorno += linha;
            }

            // Fix: Replace the incorrect usage of 'await' with a direct string manipulation
            retorno = retorno.Replace("$Nome", nome);
            retorno = retorno.Replace("$Link", link);

            return await Task.FromResult(retorno);
        }
    }
}
