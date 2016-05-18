using Microsoft.Practices.EnterpriseLibrary.Data;
using Newtonsoft.Json;
using System;
using System.Xml;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using TecFort.Framework.Generico;
using MK.Easydoc.Core.Entities;
using MK.Easydoc.Core.Infrastructure;
using MK.Easydoc.Core.Repositories.Interfaces;
using System.Net;
using System.Net.Mail;
using System.Web;
using ICSharpCode.SharpZipLib.Zip;
using System.IO;
using System.IO.Compression;

namespace MK.Easydoc.Core.Repositories
{
    public class Email
    {
        public string Remetente { get; set; }
        public string Usuario { get; set; }
        public string Senha { get; set; }
        public string Servidor_Email { get; set; }
        public int Porta { get; set; }

        public Email() { }
        public Email(int idServico)
        {
            ConfirgurarEmail(idServico);
        }

        public void CriarArquivoZip(string ArquivoOrigem, string ArquivoDestino, string diretorio)
        {
            var x = new FastZip();
            x.CreateZip(ArquivoDestino, diretorio,true, "");
            using (ZipFile zip = new ZipFile(@"c:\Teste"))
            {
                zip.Add(ArquivoOrigem);
            }
        }

        public string EnviarEmailPara(string _Subject, string _Conteudo, string _EmailDestinatario, string remetente, string arqanexo = "")
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
                if (arqanexo != "") objEmail.Attachments.Add(new Attachment(anexo));

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

        public string EnviarEmail(string _Subject, string _Conteudo, string _EmailDestinatario, string arqanexo = "")
        {
            string ret = "";
            try
            {

                //crio objeto responsável pela mensagem de email
                MailMessage objEmail = new MailMessage();

                //rementente do email
                objEmail.From = new MailAddress("email-sistema@multiarkivos.com.br", this.Remetente);

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
                //string arqzip = HttpContext.Current.Server.MapPath("~/ImageStorage/Documento_" + this.Remetente.ToString().Trim() + ".zip");

                //CriarArquivoZip(anexo, arqzip,HttpContext.Current.Server.MapPath("~/ImageStorage"));

                if (arqanexo != "") objEmail.Attachments.Add(new Attachment(anexo));
                //if (arqanexo != "") objEmail.Attachments.Add(new Attachment(arqzip));

                //cria o objeto responsável pelo envio do email
                SmtpClient objSmtp = new SmtpClient();

                //endereço do servidor SMTP(para mais detalhes leia abaixo do código)
                objSmtp.Host = this.Servidor_Email;//"mail.multiarkivos.com.br";
                objSmtp.Port = this.Porta; //587;

                //para envio de email autenticado, coloque login e senha de seu servidor de email
                //para detalhes leia abaixo do código
                //objSmtp.Credentials = new NetworkCredential("email-sistema@multiarkivos.com.br", "email-sistema@20016*");
                objSmtp.Credentials = new NetworkCredential(this.Usuario, this.Senha);

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

        private void ConfirgurarEmail(int idServico)
        {
            DocumentoModelo _modelo = new DocumentoModelo();
            Documento _documento = new Documento();
            List<Documento> _documentos = new List<Documento>();

            try
            {
                DbCommand _cmd;
                Database _db = DbConn.CreateDB();
                _cmd = _db.GetStoredProcCommand(String.Format("Get_Configuracao_Email"));

                _db.AddInParameter(_cmd, "@idServico", DbType.Int32, idServico);

                using (IDataReader _dr = _db.ExecuteReader(_cmd))
                {
                    while (_dr.Read())
                    {
                        this.Remetente = _dr["Remetente"].ToString();
                        this.Usuario = _dr["Usuario"].ToString();
                        this.Senha = _dr["Senha"].ToString();
                        this.Servidor_Email = _dr["Servidor_Email"].ToString();
                        this.Porta = int.Parse(_dr["Porta"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro COnfigurarEmail, " + ex.Message);
            }

        }
    }
}
