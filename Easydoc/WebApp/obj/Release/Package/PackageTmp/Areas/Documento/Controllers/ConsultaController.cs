using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using MK.Easydoc.Core.Entities;
using MK.Easydoc.Core.Services.Interfaces;
using MK.Easydoc.WebApp.Controllers;
using MK.Easydoc.WebApp.ViewModels;

namespace MK.Easydoc.WebApp.Areas.Documento.Controllers
{
    public class ConsultaController : BaseController
    {
        #region Private Read-Only Fields
        //private readonly ILoteService _loteService;
        private readonly IDocumentoService _docService;
        #endregion

        public ConsultaController()
        {
            //_loteService = DependencyResolver.Current.GetService<ILoteService>();
            _docService = DependencyResolver.Current.GetService<IDocumentoService>();
        }

        //
        // GET: /Documento/Consulta/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult MontarConsulta()
        {
            List<DocumentoConsulta> _cons = new List<DocumentoConsulta>();
            _cons.AddRange(_docService.ListarConsultasModelo(UsuarioAtual.ID, ServicoAtual.ID));
            ViewBag.Consultas = _cons;
            return View();
        }

        public ActionResult Consultar()
        {
            return View();
        }

        //AjaxCallConsultaDinamica
        [HttpPost]
        public JsonResult AjaxCallSalvarConsultaDinamica(int id_documento_modelo,string nome_consulta, string string_json)
        {
            var _resultado = string.Empty;
            try
            {
                _resultado = this._docService.SalvarConsultaDocumentoModelo(UsuarioAtual.ID, ServicoAtual.ID, id_documento_modelo, nome_consulta,string.Empty,false, string_json);
            }
            catch (Exception ex) { return Json(new RetornoViewModel(false, ex.Message)); }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            //var _j = serializer.DeserializeObject(_resultado);
            return Json(_resultado);
        }

        //AjaxCallConsultaDinamica
        [HttpPost]
        public JsonResult AjaxCallConsultaDinamica(int id_documento_modelo, string campos,string filtros, string proc_name)
        {
            var _resultado = string.Empty;
            try
            {
                if (filtros == "")
                {
                    filtros = "idStatus=1010";
                }
                else
                {
                    filtros += " and idStatus=1010";
                }


                _resultado = this._docService.PesquisarDocumentos(ServicoAtual.ID, id_documento_modelo, campos, filtros);
            }
            catch (Exception ex) { return Json(new RetornoViewModel(false, ex.Message)); }

            //string json = JsonConvert.SerializeObject(_resultado, Formatting.Indented);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            var _j = serializer.DeserializeObject(_resultado);
            //serializer.Deserialize (_resultado,Json);

            //return Json(new RetornoViewModel(true, null, _j));
            return Json(_j);
        }


//AjaxCallSelecionaConsultaDinamica
        [HttpPost]
        public JsonResult AjaxCallSelecionaConsultaDinamica(int id_consulta_modelo)
        {
            var _resultado = string.Empty;
            DocumentoConsulta _consulta = new DocumentoConsulta();
            try
            {
            List<DocumentoConsulta> _cons = new List<DocumentoConsulta>();
            _cons.AddRange(_docService.ListarConsultasModelo(UsuarioAtual.ID, ServicoAtual.ID));
            _resultado = _cons.Where(c => c.ID == id_consulta_modelo).FirstOrDefault().ModeloJSON;//this._docService.PesquisarDocumentos(ServicoAtual.ID, id_documento_modelo, campos, filtros);
            }
            catch (Exception ex) { return Json(new RetornoViewModel(false, ex.Message)); }

            //string json = JsonConvert.SerializeObject(_resultado, Formatting.Indented);
            //JavaScriptSerializer serializer = new JavaScriptSerializer();
            //var _j = serializer.DeserializeObject(_resultado);
            //serializer.Deserialize (_resultado,Json);

            return Json(new RetornoViewModel(true, null, @_resultado));
            //return Json(_j);
        }

        [HttpPost]
        //public JsonResult AjaxCallPesquisarDocumentos(string _search, int nd, int rows,int page, int sidx, string sord)
        public JsonResult AjaxCallPesquisarDocumentos(int id_documento_modelo, string filtros)
        {
            var _resultado = string.Empty;
            try
            {
                _resultado = this._docService.PesquisarDocumentos(ServicoAtual.ID, id_documento_modelo, "Caixa", filtros);
            }
            catch (Exception ex) { return Json(new RetornoViewModel(false, ex.Message)); }

            //string json = JsonConvert.SerializeObject(_resultado, Formatting.Indented);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            var _j = serializer.DeserializeObject(_resultado);
            //serializer.Deserialize (_resultado,Json);
            //return Json(new RetornoViewModel(true, null, _j));
            return Json(_j);

        }

        [HttpPost]
        public JsonResult AjaxCallBuscarTiposDocumento()
        {
            var docs = default(List<MK.Easydoc.Core.Entities.DocumentoModelo>);

            try
            {
                var lista = this._docService.ListarTipos(ServicoAtual.ID);

                docs = (from doc in lista
                        select doc
                        ).ToList();
            }
            catch (Exception ex) { return Json(new RetornoViewModel(false, ex.Message)); }

            return Json(new RetornoViewModel(true, null, docs));
        }

        //AjaxCallGridHeadData
        [HttpPost]
        public JsonResult AjaxCallGridHeadData(int id_documento_modelo)
        {
            string sJson = "{}";
            try
            {
/*                sJson = @"{'
                         
                         colNames': ['ID', 'Matricula', 'Caixa', 'Período','Lote','Arquivo'],
                        'colModel': [
                            { name: 'IdDocumento', index: 'IdDocumento', jsonmap: 'IdDocumento', key: true, align: 'left', width: 40 },
                            { name: 'Matricula', index: 'Matricula', jsonmap: 'Matricula', align: 'center', width: 100 },
                            { name: 'Caixa', index: 'Caixa', jsonmap: 'Caixa', align: 'center',width: 100 },
                            { name: 'Periodo', index: 'Periodo', jsonmap: 'Periodo', align: 'center',width: 100 },
                            { name: 'Lote', index: 'Lote', jsonmap: 'Lote', align: 'left',width: 100, align: 'center'},
                            { name: 'PathArquivo', index: 'PathArquivo', jsonmap: 'PathArquivo', align: 'left', width: 30, sortable: false, formatter: '#FormatterLinkOpenArquivo#'}
                        ]}";
 */
                sJson = @"{'th': ['ID', 'Matricula', 'Caixa', 'Período','Lote','Arquivo'],
                            'tr': [
                                            { name: 'IdDocumento',  index: 'IdDocumento',   jsonmap: 'IdDocumento', align: 'left',      width: 40, key: true},
                                            { name: 'Matricula',    index: 'Matricula',     jsonmap: 'Matricula',   align: 'center',    width: 100 },
                                            { name: 'Caixa',        index: 'Caixa',         jsonmap: 'Caixa',       align: 'center',    width: 100 },
                                            { name: 'Periodo',      index: 'Periodo',       jsonmap: 'Periodo',     align: 'center',    width: 100 },
                                            { name: 'Lote',         index: 'Lote',          jsonmap: 'Lote',        align: 'center',    width: 100 },
                                            { name: 'PathArquivo',  index: 'PathArquivo',   jsonmap: 'PathArquivo', align: 'left',      width: 30, sortable: false, formatter: '#FormatterLinkOpenArquivo#'}
                                        ],
                            'where': 
                            [
                                { sel: 'matricula', op: '=', val: '7880123456' },
                                { sel: 'matricula', op: '=', val: '7880123456', x:'and'},
                            ],
                            'exec':{proc:'proc_consulta_padrao', idoc: 1, par:''}
                        }";
            }
            catch (Exception ex) { return Json(new RetornoViewModel(false, ex.Message)); }
            return Json(new RetornoViewModel(true, null, sJson));
        }

        //AjaxCallBuscarCamposModelo
        [HttpPost]
        public JsonResult AjaxCallBuscarCamposModelo(int id_documento_modelo)
        {
            var _campos = default(List<MK.Easydoc.Core.Entities.CampoModelo>);

            try
            {
                var lista = _docService.ListarCamposModelo(id_documento_modelo).Where(c=>c.FiltroConsulta).ToList<CampoModelo>();

                _campos = (from campo in lista
                        select campo
                        ).ToList();
            }
            catch (Exception ex) { return Json(new RetornoViewModel(false, ex.Message)); }
            return Json(new RetornoViewModel(true, null, _campos));
        }

    }
}
