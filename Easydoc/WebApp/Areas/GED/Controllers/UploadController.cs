using System;
using System.Linq;
using System.Web.Mvc;
using System.Dynamic;
using System.Collections.Generic;
using MK.Easydoc.Core.Entities;
using MK.Easydoc.Core.Services.Interfaces;
using MK.Easydoc.Core.Infrastructure;
using MK.Easydoc.WebApp.ViewModels;
using System.IO;
using System.Web;
using MK.Easydoc.WebApp.Controllers;
using System.Text.RegularExpressions;
using MK.Easydoc.Core.Helpers;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Web.Helpers;


namespace MK.Easydoc.WebApp.Areas.GED.Controllers
{
    public class UploadController : BaseController
    {

        private const string _LOTE_SESSION_NAME = "__UoloadController__lote_imagens__";

        #region Public Properties

        #endregion Public Properties
        public Lote LoteImagens {
            get
            {
                return (Lote)Session[_LOTE_SESSION_NAME] ?? new Lote();
            }
            private set { Session[_LOTE_SESSION_NAME] = value; }
        }

        #region Private Read-Only Fields

        private readonly ILoteService _loteService;
        private readonly IDocumentoService _docService;
        Lote _lote = new Lote();
        #endregion

        #region Constructors

        public UploadController()
        {
            _lote = new Lote();
            this._loteService = DependencyResolver.Current.GetService<ILoteService>();
            _docService = DependencyResolver.Current.GetService<IDocumentoService>();
        }

        #endregion
        private string RetornaDiretorioUpload() {
            string _diretorio = String.Empty;
            try
            {
                //string _raiz = HttpContext.Server.MapPath("~/Content/Uploads");//@"C:\Temp\img\upload";
                //string _cliente = StringFormatHelper.RemoveSpecialCharacters(ClienteAtual.Descricao.Trim().Replace(@" ", "_")).Replace(@" ", "_");
                //string _servico = StringFormatHelper.RemoveSpecialCharacters(ServicoAtual.Descricao.Trim().Replace(@" ", "_")).Replace(@" ", "_");

                string _cliente = IdCliente_Atual.ToString("D3");
                string _servico = IdServico_Atual.ToString("D4");

                //string _diretorio = string.Format(@"{0}\{1}\{2}\{3}\{4}\{5}\{6}", _raiz, _cliente, _servico, DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, LoteImagens.ID);
                ////string _diretorio = string.Format(@"{0}\{1}\{2}\{3}\{4}", _cliente, _servico, DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                string DataCaptura = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString("D2") + DateTime.Now.Day.ToString("D2");
                string v_cliente = _cliente;
                string v_servico = _servico;
                _diretorio = string.Format(@"{0}\{1}\{2}", v_cliente, v_servico, DataCaptura);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            
           return _diretorio;
        }
        private string CriaDiretorio() {

            string _raiz = HttpContext.Server.MapPath("~/StoragePrivate/");//@"C:\Temp\img\upload";
            string _cliente = StringFormatHelper.RemoveSpecialCharacters(ClienteAtual.Descricao.Trim().Replace(@" ", "_")).Replace(@" ", "_");
            string _servico = StringFormatHelper.RemoveSpecialCharacters(ServicoAtual.Descricao.Trim().Replace(@" ", "_")).Replace(@" ", "_");
            string _diretorio = string.Format(@"{0}\{1}", _raiz, LoteImagens.PathCaptura.Trim());
            //string _diretorio = string.Format(@"{0}\{1}\{2}", _raiz, LoteImagens.PathCaptura.Trim(), LoteImagens.ID);
            //string _diretorio = string.Format(@"{0}\{1}\{2}\{3}\{4}\{5}", _raiz, _cliente, _servico, DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

            if (!Directory.Exists(_diretorio))
            {
                Directory.CreateDirectory(_diretorio);
            }
            return _diretorio;        
        }
        [HttpPost]
        public JsonResult AjaxCallGerarLote()
        {
            try
            {
                if (LoteImagens.ID == 0 || ServicoAtual.ID != LoteImagens.ServicoCaptura.ID)
                {
                    LoteImagens = _loteService.CriarLote(UsuarioAtual.ID, 1, ServicoAtual.ID, RetornaDiretorioUpload());
                }                                                            
            }
            catch (ValidationException ex) { 
                return Json(new RetornoViewModel(false, ex.Message, ex.Errors.Select(e => e.ErrorMessage).ToArray(), RetornoType.ValidationExceptions)); 
            }
            catch (Exception ex) { return Json(new RetornoViewModel(false, ex.Message)); }

            return Json(new RetornoViewModel(true, null, LoteImagens)); 
        }
        public JsonResult AjaxCallEncerrarLote()
        {
            try
            {

                int _ret = 0;
                bool UsaArquivoDados = _loteService.UsaArquivoDados(ServicoAtual.ID);

                if (UsaArquivoDados)
                {
                    //logica com json
                    if (LoteImagens.Log.Dados == null)
                    {
                        return Json(new RetornoViewModel(false, "O arquivo de dados do lote não foi importado ou é inválido. Favor verificar!", null, RetornoType.ValidationExceptions));
                    }
                    if (LoteImagens.ID != 0 && ServicoAtual.ID == LoteImagens.ServicoCaptura.ID && LoteImagens.Itens.Count > 0)
                    {
                        if (LoteImagens.Log.NumeroPaginas == 0 || LoteImagens.QtdeImagem == LoteImagens.Log.NumeroPaginas)
                        {
                            _ret = _loteService.InserirItensLote(LoteImagens);
                            LoteImagens.StatusLote = 1010;
                            _loteService.AtualizarLote(LoteImagens);
                        }
                        else
                        {
                            return Json(new RetornoViewModel(false, string.Format(@"A quantidade de imagens importadas ({0}) não corresponde com a quantidade de imagens informada no aquivo ({1}), certifique-se de que importou todos os documentos do lote, caso contrário digitalize novamente o lote.", LoteImagens.Itens.Count, LoteImagens.Log.NumeroPaginas), null, RetornoType.ValidationExceptions));
                        }
                        int _retidlote = _loteService.VerificaLoteJaExiste(LoteImagens.ID, LoteImagens.Log.NomeLote);
                        if (_retidlote != 0)
                        {
                            return Json(new RetornoViewModel(false, string.Format(@"Lote já importado!!!"), null, RetornoType.ValidationExceptions));
                        }
                        if (!(ServicoAtual.ID == LoteImagens.ServicoCaptura.ID))
                        {
                            return Json(new RetornoViewModel(false, string.Format(@"Lote não e deste Serviço !!!"), null, RetornoType.ValidationExceptions));
                        }
                        
                        string[] _dados = LoteImagens.Log.Dados;
                        if (_dados.Count() > 0)
                        {
                            foreach (string _dado in _dados)
                            {
                                string[] _arr = _dado.Split('=');

                                if (_arr[0].ToString().Trim() == "TipoDocumento")
                                {
                                    if (!string.IsNullOrEmpty(_arr[1].ToString().Trim()))
                                    {
                                        _loteService.TipificarItemDescricao(UsuarioAtual.ID, ServicoAtual.ID, LoteImagens.ID, LoteImagens.ID, _arr[1].ToString().Trim());
                                    }
                                }
                            }
                        }
                        //Se tiver codigo de barras, atualiza o dicumento atravez do lote e do nome original da imagem
                        foreach (Arquivo _arq in LoteImagens.Log.Arquivos)
                        {
                            if (!string.IsNullOrEmpty(_arq.CB))
                            {
                                string _img = string.Empty;
                                _img = LoteImagens.Itens.Where(i => i.NomeOriginal.ToLowerInvariant() == _arq.Nome.ToLowerInvariant()).FirstOrDefault().NomeFinal;
                                if (!string.IsNullOrEmpty(_img))
                                {
                                    _docService.AtualiarDocumentoCB(UsuarioAtual.ID, ServicoAtual.ID, LoteImagens.ID, _arq.Verso, _arq.CB, _img);
                                }
                            }
                        }
                        LoteImagens = new Lote();
                    }
                    else
                    {
                        return Json(new RetornoViewModel(false, "Não foram importados arquivos de imagens para o lote gerado. Favor verificar!.", null));
                    }
                    //--------------------
                }
                else
                {
                    //logica Sem json
                    if (LoteImagens.Log.NumeroPaginas == 0 || LoteImagens.QtdeImagem == LoteImagens.Log.NumeroPaginas)
                    {                        
                        _ret = _loteService.InserirItensLote(LoteImagens);
                        LoteImagens.StatusLote = 1010;
                        _loteService.AtualizarLote(LoteImagens);
                        _loteService.TipificarItemDescricao(UsuarioAtual.ID, ServicoAtual.ID, LoteImagens.ID, LoteImagens.ID, "");
                        LoteImagens = new Lote();
                    }
                    else
                    {
                        return Json(new RetornoViewModel(false, string.Format(@"A quantidade de imagens importadas ({0}) não corresponde com a quantidade de imagens informada no aquivo ({1}), certifique-se de que importou todos os documentos do lote, caso contrário digitalize novamente o lote.", LoteImagens.Itens.Count, LoteImagens.Log.NumeroPaginas), null, RetornoType.ValidationExceptions));
                    }
                    //---------------------------------------------------------------------------------------
                }

            }
            catch (ValidationException ex)
            {
                return Json(new RetornoViewModel(false, ex.Message, ex.Errors.Select(e => e.ErrorMessage).ToArray(), RetornoType.ValidationExceptions));
            }
            catch (Exception ex) { return Json(new RetornoViewModel(false, ex.Message)); }

            return Json(new RetornoViewModel(true, "Lote importado com sucesso!", null));
        }
        public JsonResult AjaxCallCancelarLote()
        {
            try
            {
                _loteService.ApagarLote(LoteImagens.ID);
                LoteImagens = new Lote();
            }
            catch (ValidationException ex)
            {
                return Json(new RetornoViewModel(false, ex.Message, ex.Errors.Select(e => e.ErrorMessage).ToArray(), RetornoType.ValidationExceptions));
            }
            catch (Exception ex) { return Json(new RetornoViewModel(false, ex.Message)); }

            return Json(new RetornoViewModel(true, null, LoteImagens));
        }
        private LogLote ConverteLogLoteJSON(string _pathJSON) {
            JObject jsonObj = new JObject();
            //JObject o1 = new JObject();
            JsonResult _js = new JsonResult();

            //o1 = JObject.Parse(System.IO.File.ReadAllText(_pathJSON));

            // read JSON directly from a file
            using (StreamReader _file = System.IO.File.OpenText(_pathJSON))
            using (JsonTextReader reader = new JsonTextReader(_file))
            {
                jsonObj = (JObject)JToken.ReadFrom(reader);
                _js = Json(jsonObj);
            }

            //JObject rss = JObject.Parse(jsonObj.ToString());

            //string _loglot = (string)rss["Lote"];

            var jsonData = default(string);
            //var lot = default(LogLote);
            LogLote _logLote = new LogLote();
            jsonData = JsonConvert.SerializeObject(jsonObj["Lote"]);

            //lot = new LogLote();
            _logLote = (LogLote)JsonConvert.DeserializeObject<LogLote>(jsonData);

            //string fa = _logLote.NomeLote;


            return _logLote;
        
        }
        [HttpPost]
        public JsonResult SaveFiles(string qqfile)
        {
            //string id = Request["id"];
            var path = CriaDiretorio();//string.Empty;//@"C:\Temp\img";//Server.MapPath("FilesFolderPath");
            var file = string.Empty;
            var file_Extension = string.Empty;
            var file_Size = string.Empty;
            var fine_new_name = string.Empty;



            int arquivosSalvos = 0;
            for (int i = 0; i < Request.Files.Count; i++)
            {
                HttpPostedFileBase arquivo = Request.Files[i];
                if (arquivo.ContentLength > 0)
                {
                    string fileName = Path.GetFileName(arquivo.FileName);
                    arquivosSalvos++;
                }
            }


            //string teste = @"C:\tmp\Teste.txt";
            //var _arq = new FileInfo(teste);
            //string DataCriacao = _arq.CreationTime.ToString("dd/MM/yyyy hh:mm:ss");


            try
            {
                //LoteImagens.PathCaptura = CriaDiretorio();
                
                var stream = Request.InputStream;
                if (String.IsNullOrEmpty(Request["qqfile"]))
                {
                    // IE
                    HttpPostedFileBase postedFile = Request.Files[0];
                    stream = postedFile.InputStream;
                    file = Path.Combine(LoteImagens.PathCaptura, Path.GetFileName(Request.Files[0].FileName));
                    
                }
                else
                {
                    //Webkit, Mozilla
                    file = Path.Combine(path, qqfile);
                }

                var arq = new FileInfo(file);
                var caminho_original = System.IO.Path.GetDirectoryName(qqfile);
                string _DataCriacao = arq.CreationTime.ToString("dd/MM/yyyy hh:mm:ss");

                // Get Extension
                file_Extension = Path.GetExtension(file); 

                var file_new = "";
                
                //fine_new_name = string.Format("U{0}C{1}S{2}o us_{3}{4}{5}{6}{7}", UsuarioAtual.ID, ServicoAtual.Cliente.ID, ServicoAtual.ID, DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Minute, DateTime.Now.Millisecond);
                //file_new = file.Replace(qqfile, string.Format("{0}{1}", fine_new_name, file_Extension));


                // renomeia e verifia se nao existe ja um arquivo com o mesmo nome.
                do
                {
                    //fine_new_name = string.Format("U{0}C{1}S{2}_{3}{4}{5}{6}{7}{8}", UsuarioAtual.ID, ServicoAtual.IdCliente, ServicoAtual.ID, DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Minute, DateTime.Now.Millisecond, file_Extension);
                    fine_new_name = string.Format("{0}{1}{2}_{3}{4}{5}{6}{7}{8}", UsuarioAtual.ID, ServicoAtual.IdCliente, ServicoAtual.ID, DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Minute, DateTime.Now.Millisecond, file_Extension);

                    file_new = file.Replace(qqfile, fine_new_name);
                } while (System.IO.File.Exists(file_new));
                
                // Save File
                var buffer = new byte[stream.Length];
                stream.Read(buffer, 0, buffer.Length);
                System.IO.File.WriteAllBytes(file_new, buffer);

                //27/11/2015
                var v_arq = new FileInfo(file_new);
                DateTime v_DataCriacaoArqCap = DateTime.Parse(v_arq.CreationTime.ToString("dd/MM/yyyy hh:mm:ss.fff"));

                //DataCriacaoArqCap

                if (file_Extension.ToUpper().Trim() != ".JSON")
                {
                    LoteImagens.QtdeImagem++;
                    LoteImagens.Itens.Add(new LoteItem { ID = 0, NomeFinal = fine_new_name, NomeOriginal = qqfile, DataCriacaoArqCap=v_DataCriacaoArqCap, OrigemID = 1, StatusImagem = 1000, SequenciaCaptura = LoteImagens.QtdeImagem, DocumentoModelo = (new MK.Easydoc.Core.Entities.DocumentoModelo { ID = 0 }), UsuarioCaptura = (new Usuario { ID = UsuarioAtual.ID }) });
                }
                else
                {
                    LoteImagens.Log = ConverteLogLoteJSON(@file_new);                    
                }
                //lot = new LogLote();
                //lot = (LogLote)JsonConvert.DeserializeObject<LogLote>(jsonData);

                //var x = _lo.NomeLote;
                //IList<LogLote> _logs = new List<LogLote>{
                //    new LogLote{
                //        o2
                //    }

                //};

                //_js = Json(o2);
                // Get File Size
                FileInfo f = new FileInfo(file_new);
                file_Size = Convert.ToString(f.Length); 

            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, "application/json");
            }
            return Json(new { success = true }, "text/html");
        }

    }
}
