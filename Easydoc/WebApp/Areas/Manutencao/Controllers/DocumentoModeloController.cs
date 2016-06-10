using MK.Easydoc.Core.Infrastructure;
using MK.Easydoc.Core.Repositories;
using MK.Easydoc.Core.Services;
using MK.Easydoc.WebApp.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MK.Easydoc.Core.Entities;
using Newtonsoft.Json.Linq;

namespace MK.Easydoc.WebApp.Areas.Manutencao.Controllers
{
    public class DocumentoModeloController : BaseController
    {
        //
        // GET: /Manutencao/DocumentoModelo/
        public ActionResult Index(int idServico=0)
        {
            int _idservico = idServico==0 ? IdServico_Atual : idServico;
            
            ListaClienteServico(IdCliente_Atual.ToString(), _idservico);
            ViewBag.idCliente = -1; //IdCliente_Atual;
            ViewBag.idServico = -1; //IdServico_Atual;
            //var lista = new DocumentoModeloRepository().Listar(_idservico);
            return View("Index",null);
        }

        public ActionResult Lista(int idServico = 0, int idCliente=0)
        {
            int _idservico = idServico == 0 ? IdServico_Atual : idServico;
            int _idcliente = idCliente == 0 ? IdCliente_Atual : idCliente;

            ListaClienteServico(_idcliente.ToString(), _idservico);
            ViewBag.idCliente = _idcliente;
            ViewBag.idServico = _idservico;
            var lista = new DocumentoModeloRepository().Listar(_idservico);
            return View("Index",lista);
        }
        public ActionResult Novo(string msg="")
        {
            
            ListaClienteServico(IdCliente_Atual.ToString(), IdServico_Atual);
            ViewBag.idCliente = IdCliente_Atual;
            ViewBag.idServico = IdServico_Atual;

            if (msg != "")
                TempData["Error"] = msg;

            return View();
        }

        public ActionResult Edit(int id, int idcliente, int idservico)
        {

            ListaClienteServico(idcliente.ToString(), idservico);
            ViewBag.idCliente = idcliente;
            ViewBag.idServico = idservico;
            //if (msg != "")
              //  TempData["Error"] = msg;
            var lst = new DocumentoModeloRepository().ListarDocumentoCampoModelo(idservico, id);

            return View(lst);
        }

        [HttpPost]
        public ActionResult Incluir(FormCollection frm)
        {
            
            var Retorno = new Retorno();
            try
            {
                var p = new DocumentoModelo();
                
                    p.TipoAcao = 1;
                    p.Descricao = frm["txtnomedoc"].ToString();
                    p.Rotulo = frm["txtrotulo"].ToString();
                    p.idServico = int.Parse(frm["SelServico"].ToString());
                    p.Tipificalote = int.Parse(frm["retTipificar"].ToString());
                    p.Multi_Pagina = int.Parse(frm["retMultipagina"].ToString());
                    p.ScriptSQLTipificar = frm["txtscriptsqltipificar"].ToString();
                    p.ScriptSQLValidar = frm["txtscriptsqlvalidar"].ToString();
                    p.ScriptSQLConsulta = frm["txtscriptsqlconsulta"].ToString();
                    p.ScriptSQLModulo = frm["txtscriptsqlmodulo"].ToString();
                    p.DocumentoModeloPai = frm["txtdocmodelopai"].ToString()==""? 0:int.Parse(frm["txtdocmodelopai"].ToString());
                    p.ArquivoDados = int.Parse(frm["retArqDados"].ToString());
                    p.idCampoModelo = frm["idCampoModelo"].ToString();
                

                var docModelo = new DocumentoModeloRepository();
                Retorno = docModelo.Incluir(p);
                if (Retorno.CodigoRetorno < 0)
                {
                    throw new Exception(Retorno.Mensagem);
                }
                else
                {
                    if (frm["listaCampos"].ToString() != "")
                    {
                        //var campos_selecionados = p.idCampoModelo.Split(',');
                        var items = frm["listaCampos"].ToString(); // Get the JSON string
                        JArray o = JArray.Parse(items); // It is an array so parse into a JArray
                        int n = 1;
                        foreach (var itm in o)
                        {
                            int _IdCampo = int.Parse(itm.SelectToken("idcampo").ToString());   //o.SelectToken("[0].cod").ToString(); // Get the name value of the 1st object in the array
                            int _Digita = int.Parse(itm.SelectToken("Digita").ToString()); //o.SelectToken("[0].valor").ToString();
                            int _FiltroConsulta = int.Parse(itm.SelectToken("FiltroConsulta").ToString());

                            int _Obrigatorio = int.Parse(itm.SelectToken("Obrigatorio").ToString());
                            int _Reconhece = int.Parse(itm.SelectToken("Reconhece").ToString());
                            string _Validacao = itm.SelectToken("Validacao").ToString();

                            var cpm = new DocumentoCampoModelo();
                            cpm.idDocumentoModelo = Retorno.idDocumentoModelo;
                            cpm.idCampoModelo = _IdCampo;
                            cpm.Digita = _Digita;
                            cpm.FiltroConsulta = _FiltroConsulta;
                            cpm.Requerido = _Obrigatorio;
                            cpm.Reconhece = _Reconhece;
                            cpm.ProcSqlValidacao = _Validacao;
                            cpm.IdDocumentoModeloPai = p.DocumentoModeloPai;
                            cpm.Tabulacao = n;
                            new DocumentoModeloRepository().IncluirCampos(cpm);
                            n++;
                        }

                        //foreach (var campo in campos_selecionados)
                        //{
                        //    var cpm = new DocumentoCampoModelo();
                        //    cpm.idDocumentoModelo = Retorno.idDocumentoModelo;
                        //    cpm.idCampoModelo = int.Parse(campo.ToString());
                        //    cpm.IdDocumentoModeloPai = p.DocumentoModeloPai;
                        //    cpm.Tabulacao = n;
                        //    new DocumentoModeloRepository().IncluirCampos(cpm);
                        //    n++;
                        //}
                    }
                }
                TempData["Msg"] = Retorno.Mensagem;
                return RedirectToAction("Index");

            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("Novo", new {msg=ex.Message});
            }
        }


        public ActionResult ListarCampos(int idCliente, int idServico)
        {
            try
            {
                var ListaCampos = new DocumentoRepository().ListarCamposDocumento(idServico);
                ListaClienteServico(idCliente.ToString());
                ViewBag.idCliente = idCliente;
                ViewBag.idServico = idServico;
                ViewBag.ListaCampos = ListaCampos;
                if (ListaCampos.Count == 0) TempData["Error"] = "Não existem campos cadastrados para esse serviço. Por favor, cadastre os campos antes de cadastrar os documentos.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            } 
            return View("Novo");
        }

        public ActionResult ListaServico(string idCliente)
        {
            ListaClienteServico(idCliente);
            ViewBag.idCliente = idCliente;
            ViewBag.idServico = -1;
            return View("Novo");
        }
        public ActionResult ListaServicoIndex(string idCliente)
        {
            ListaClienteServico(idCliente);
            ViewBag.idCliente = idCliente;
            ViewBag.idServico = IdServico_Atual;
            //var lista = new DocumentoModeloRepository().Listar(IdServico_Atual);
            return View("Index",null);
        }
        void ListaClienteServico(string idCliente, int idServico = 0)
        {
            var listaCliente = new ClienteRepository().ListaClientePorUsuario(Session["NomeUsuario"].ToString());
            //ViewBag.Teste = listaCliente;
            var listacli = new SelectList(
                            listaCliente.ToList(),
                            "ID",
                            "Descricao"
                        );
            ViewBag.ListaCliente = listacli;
            ViewBag.CodCliente = idCliente;
            ViewBag.idCliente = idCliente;
            ViewBag.idServico = idServico;
            var listaServico = new ClienteRepository().ListaServicoPorCliente(Session["NomeUsuario"].ToString(), idCliente);

            var lista = new SelectList(
                            listaServico.ToList(),
                            "ID",
                            "Descricao"
                        );
            ViewBag.ListaServico = lista;
        }
    }
}
