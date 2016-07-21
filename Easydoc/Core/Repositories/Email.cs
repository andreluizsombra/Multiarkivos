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
using System.IO.Compression;
using ICSharpCode.SharpZipLib.Zip;
using System.ComponentModel;


namespace MK.Easydoc.Core.Repositories
{
    public class Email
    {
        public string Remetente { get; set; }
        public string Usuario { get; set; }
        public string Senha { get; set; }
        public string Servidor_Email { get; set; }
        public int Porta { get; set; }
        public string DiretorioTemp { get; set; }
        public string ArquivoZip { get; set; }
        public bool Download { get; set; }
        public Email() { }
        public Email(int idServico)
        {
            ConfirgurarEmail(idServico);
        }
        public static string RetornoNomeArquivo(string path){
            string patharquivo = "";
            if (path.Contains("https"))
                patharquivo = path;
            else
                patharquivo = HttpContext.Current.Server.MapPath(path);

            return System.IO.Path.GetFileName(patharquivo);
        }
        public string CriarArquivoZip(string ArquivoOrigem, string ArquivoDestino="", int qtdArqOrigem=0)
        {
            
            string _diretorio = Path.GetDirectoryName(ArquivoOrigem);
            string _nome_arq_origem = System.IO.Path.GetFileName(ArquivoOrigem);
            FastZip fastZip = new FastZip();
            bool recurse = true;  // Include all files by recursing through the directory structure
            string filter = null; // Dont filter any files at all

            string fileName = "";
            string destFile = "";
            string _diretorio_temp = _diretorio + "\\temp";
            this.DiretorioTemp = _diretorio_temp;

            if (System.IO.Directory.Exists(_diretorio_temp))
            {
                string[] files = System.IO.Directory.GetFiles(_diretorio_temp);

                // Copy the files and overwrite destination files if they already exist.
                foreach (string s in files)
                {
                    // Use static Path methods to extract only the file name from the path.
                    fileName = System.IO.Path.GetFileName(s);
                    destFile = System.IO.Path.Combine(_diretorio_temp, fileName);
                    System.IO.File.Delete(destFile);
                }
               // System.IO.Directory.Delete(_diretorio_temp);
            }
            else
            {
                Directory.CreateDirectory(_diretorio_temp);
            }
            
            if (System.IO.Directory.Exists(_diretorio_temp))
            {
                string[] files = System.IO.Directory.GetFiles(_diretorio);

                if (qtdArqOrigem == 1)
                {
                    fileName = _nome_arq_origem;
                    destFile = System.IO.Path.Combine(_diretorio_temp, fileName);
                    System.IO.File.Copy(ArquivoOrigem, destFile, true);
                }
                else
                {
                    foreach (string s in files)
                    {
                        // Use static Path methods to extract only the file name from the path.
                        fileName = System.IO.Path.GetFileName(s);
                        destFile = System.IO.Path.Combine(_diretorio_temp, fileName);
                        System.IO.File.Copy(s, destFile, true);
                    }
                }
            }

            //System.IO.Directory.Delete(_diretorio_temp);
            //fastZip.CreateZip("fileName.zip", @"C:\SourceDirectory", recurse, filter);
            
            string arqzip = HttpContext.Current.Server.MapPath("~/ImageStorage/Documentos.zip");
            if (System.IO.File.Exists(arqzip)) { System.IO.File.Delete(arqzip); }
            //string arqzip = HttpContext.Current.Server.MapPath(_diretorio_temp+@"\Documentos.zip");
            this.ArquivoZip = arqzip;
            fastZip.CreateZip(arqzip, _diretorio_temp, recurse, filter);
            
            //var zip = new FastZip();
            //zip.CreateZip()
            return arqzip;
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

                string _nome_arquivo = "";
                //Anexar arquivo
                string anexo = "";
                if (int.Parse(HttpContext.Current.Session["Nuvem"].ToString()) == 1)
                {
                    WebClient webClient = new WebClient();
                    //webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(Completo);
                    //webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressoFeito);
                    _nome_arquivo = RetornoNomeArquivo(arqanexo);
                    if(!System.IO.Directory.Exists(HttpContext.Current.Server.MapPath("~/StoragePrivate/TempNuvem")))
                    {
                        Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/StoragePrivate/TempNuvem"));
                    }
                    
                    //webClient.DownloadFileAsync(new Uri(arqanexo), HttpContext.Current.Server.MapPath("~/StoragePrivate/TempNuvem/"+_nome_arquivo));
                    webClient.DownloadFile(arqanexo, HttpContext.Current.Server.MapPath("~/StoragePrivate/TempNuvem/" + _nome_arquivo));
                    anexo = HttpContext.Current.Server.MapPath("~/StoragePrivate/TempNuvem/" + _nome_arquivo);
                }
                else
                {
                    anexo = HttpContext.Current.Server.MapPath("~" + arqanexo);
                }

                //string arqzip = HttpContext.Current.Server.MapPath("~/ImageStorage/Documento_" + this.Remetente.ToString().Trim() + ".zip");
                //CriarArquivoZip(anexo, HttpContext.Current.Server.MapPath("~/ImageStorage"));
                
                //19/05/2016
                anexo = CriarArquivoZip(anexo,"",1);

                if (anexo != "") objEmail.Attachments.Add(new Attachment(anexo));
                //if (arqanexo != "") objEmail.Attachments.Add(new Attachment(arqzip));

                //cria o objeto responsável pelo envio do email
                SmtpClient objSmtp = new SmtpClient();

                if (this.Servidor_Email == null) throw new Exception("Servidor de email não esta configurado.");
                if (this.Porta == 0) throw new Exception("Porta do servidor de email não esta configurada.");
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
        private void Completo(object sender, AsyncCompletedEventArgs e)
        {
            Download = true;
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
