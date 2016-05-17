using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;

namespace MK.Easydoc.Core.Repositories
{
    public class Email
    {
        public string EnviarEmailPara(string _Subject, string _Conteudo, string _EmailDestinatario, string remetente, string arqanexo="")
        {
            string ret = "";
            try
            {
                //crio objeto responsável pela mensagem de email
                MailMessage objEmail = new MailMessage();


                //rementente do email
                objEmail.From = new MailAddress("email-sistema@multiarkivos.com.br", remetente);

                //email para resposta(quando o destinatário receber e clicar em responder, vai para:)
                // objEmail.ReplyTo = new MailAddress("email@seusite.com.br");

                //destinatário(s) do email(s). Obs. pode ser mais de um, pra isso basta repetir a linha
                //abaixo com outro endereço
                objEmail.To.Add(_EmailDestinatario);

                //se quiser enviar uma cópia oculta pra alguém, utilize a linha abaixo:
                //objEmail.Bcc.Add("oculto@provedor.com.br");

                //prioridade do email
                objEmail.Priority = MailPriority.Normal;

                //utilize true pra ativar html no conteúdo do email, ou false, para somente texto
                objEmail.IsBodyHtml = true;

                //Assunto do email
                objEmail.Subject = _Subject;

                //corpo do email a ser enviado
                objEmail.Body = _Conteudo;//"Conteúdo do email. Se ativar html, pode utilizar cores, fontes, etc.";

                //codificação do assunto do email para que os caracteres acentuados serem reconhecidos.
                objEmail.SubjectEncoding = Encoding.GetEncoding("ISO-8859-1");

                //codificação do corpo do emailpara que os caracteres acentuados serem reconhecidos.
                objEmail.BodyEncoding = Encoding.GetEncoding("ISO-8859-1");

                //Anexar arquivo
                string anexo = HttpContext.Current.Server.MapPath("~" + arqanexo);
                if(arqanexo!="") objEmail.Attachments.Add(new Attachment(anexo));

                //cria o objeto responsável pelo envio do email
                SmtpClient objSmtp = new SmtpClient();

                //endereço do servidor SMTP(para mais detalhes leia abaixo do código)
                objSmtp.Host = "mail.multiarkivos.com.br";
                objSmtp.Port = 26; //587;

                //para envio de email autenticado, coloque login e senha de seu servidor de email
                //para detalhes leia abaixo do código
                objSmtp.Credentials = new NetworkCredential("email-sistema@multiarkivos.com.br", "email-sistema@20016*");

                //envia o email
                objSmtp.Send(objEmail);

                ret = "Email enviado com sucesso";
            }
            catch (Exception erx)
            {
                ret = "Erro ao enviar Email, " + erx.Message;
                //throw new ArgumentException("Erro ao enviar Email, " + erx.Message, erx);
            }
            return ret;
        }
    }
}
