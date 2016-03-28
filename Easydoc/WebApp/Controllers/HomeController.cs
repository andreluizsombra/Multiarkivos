using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MK.Easydoc.Core.Entities;
using MK.Easydoc.Core.Repositories;
using MK.Easydoc.Core.Infrastructure.Framework;
using TecFort.Framework.Generico;

namespace MK.Easydoc.WebApp.Controllers
{
    public class HomeController : BaseController
    {
        private IDictionary<string, object> _qryparams;
        public ActionResult Index()
        {
            //AjaxCallTrocarServicoAtual(0);
            //TODO: 10/03/2016
            if (new UsuarioRepository().GetUsuario(Session["NomeUsuario"].ToString()).TrocarSenha == 1)
            {
                return RedirectToAction("TrocarSenha");
            }

            if (ServicoAtual == null)
            {
                //Servico _serv = UsuarioAtual.Clientes.Where(c => c.Servicos.Where(s => s.Default = true).FirstOrDefault().Default = true).FirstOrDefault().Servicos.Where(s => s.Default = true).FirstOrDefault();
                //AjaxCallTrocarServicoAtual(_serv.ID);
                int _idservico = int.Parse(Session["IdServico"].ToString());
                AjaxCallTrocarServicoAtual(_idservico);
                Session["UsuarioAtual_ID"] = UsuarioAtual.ID;
                var listaCliente = new ClienteRepository().ListaClientePorUsuario(Session["NomeUsuario"].ToString());
                
                ViewBag.Teste = listaCliente;
                ViewBag.ListaCliente = new SelectList(
                        listaCliente.ToList(),
                        "ID",
                        "Descricao"
                    );
            }
            
            return View();
        }

        public ViewResult TrocarSenha(){
            return View("TrocaSenha");
        }

        [HttpPost]
        public ActionResult TrocarSenha(FormCollection frm)
        {
            try
            {
                var _cripto = new Criptografia();
                var _utils = new Util();

                string senha_atual = _cripto.Executar(frm["txt_senha_atual"].Trim().ToString(), _utils.ChaveCripto, Criptografia.TipoNivel.Baixo, Criptografia.TipoAcao.Encriptar, Criptografia.TipoCripto.Números);
                string senha_nova =  _cripto.Executar(frm["txt_senha_nova"].Trim().ToString(), _utils.ChaveCripto, Criptografia.TipoNivel.Baixo, Criptografia.TipoAcao.Encriptar, Criptografia.TipoCripto.Números);

                var ret = new UsuarioRepository().AtualizarSenha(UsuarioAtual.ID, senha_atual, senha_nova);
                if (ret.CodigoRetorno >= 0)
                {
                    TempData["Msg"] = ret.Mensagem.ToString();
                    return RedirectToRoute("Principal");
                    //return View("TrocaSenha");
                }
                else
                {
                    throw new Exception(ret.Mensagem.ToString());
                }
            }
            catch (Exception ex)
            {
                //TempData["Error"] = ex.Message;
                TempData["Msg"] = ex.Message;
                //ViewBag.MsgError = ex.Message;
                return View("TrocaSenha");
            }
        }

        [HttpPost]
        public JsonResult RetornaPendencias(int idServico)
        {
            var serv = new ServicoRepository();
            bool flag = serv.GetControleAtencao(idServico);
            var resultado = new
                    {
                        rest = flag.ToString()
                    };
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AjaxVerificatroca(string idUsuario)
        {
           var ret = new UsuarioRepository();
           int valor = ret.VerificaTroca(int.Parse(idUsuario));
           return Json(valor, JsonRequestBehavior.AllowGet);
        }

        //TODO: AndreSombra 22/02/2016
        [HttpPost]
        public JsonResult VisualizarModulo(int _idModulo)
        {
            this._qryparams = new Dictionary<string, object>();
            this._qryparams.Clear();
            this._qryparams["idUsuario"] = UsuarioAtual.ID;
            this._qryparams["idServico"] = IdServico_Atual; //ServicoAtual.ID;
            this._qryparams["idModulo"] = _idModulo;

            var modulo = new PerfilRepository();
            var resultado = modulo.ListarModulo(this._qryparams);

            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        //TODO: AndreSombra 23/02/2016
        [HttpPost]
        public JsonResult VisualizarModuloPrincipal()
        {
            this._qryparams = new Dictionary<string, object>();
            this._qryparams.Clear();
            this._qryparams["idUsuario"] = UsuarioAtual.ID;
            this._qryparams["idServico"] = IdServico_Atual; //ServicoAtual.ID;

            var modulo = new PerfilRepository();
            var resultado = modulo.ListarModuloPrincipal(this._qryparams);

            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        //TODO: AndreSombra 17/11/2015
        [HttpPost]
        public JsonResult Dashboard_Doc_Modulo(int _periodoInicial, int _periodoFinal)
        {
            this._qryparams = new Dictionary<string, object>();
            this._qryparams.Clear();
            this._qryparams["Usuario_ID"] = UsuarioAtual.ID;
            this._qryparams["Servico_ID"] = ServicoAtual.ID;
            this._qryparams["periodoInicial"] = _periodoInicial;
            this._qryparams["periodoFinal"] = _periodoFinal;

            var serv = new ServicoRepository();
            var resultado = serv.ExibirDashboard_Doc_Modulo(this._qryparams);

            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        //TODO: AndreSombra 18/11/2015
        [HttpPost]
        public JsonResult Dashboard_Doc_Captura(int _periodoInicial, int _periodoFinal)
        {
            this._qryparams = new Dictionary<string, object>();
            this._qryparams.Clear();
            this._qryparams["Usuario_ID"] = UsuarioAtual.ID;
            this._qryparams["Servico_ID"] = ServicoAtual.ID;
            this._qryparams["periodoInicial"] = _periodoInicial;
            this._qryparams["periodoFinal"] = _periodoFinal;

            var serv = new ServicoRepository();
            var resultado = serv.ExibirDashboard_Captura(this._qryparams);

            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        //TODO: AndreSombra 19/11/2015
        [HttpPost]
        public JsonResult Dashboard_Doc_Pendencias(int _periodoInicial, int _periodoFinal)
        {
            this._qryparams = new Dictionary<string, object>();
            this._qryparams.Clear();
            this._qryparams["Usuario_ID"] = UsuarioAtual.ID;
            this._qryparams["Servico_ID"] = ServicoAtual.ID;
            this._qryparams["periodoInicial"] = _periodoInicial;
            this._qryparams["periodoFinal"] = _periodoFinal;

            var serv = new ServicoRepository();
            var resultado = serv.ExibirDashboard_Pendencias(this._qryparams);

            return Json(resultado, JsonRequestBehavior.AllowGet);
        }	

        public ActionResult Dashboard()
        {
            RegistrarLOGSimples(9, 25, UsuarioAtual.NomeUsuario);
            // LOG: Entrou no modulo ESTATISTICA
            ViewBag.Message = "";
            //return View("Dashboard");
            return View("~/Views/Shared/_MorrisDonut.cshtml");
        }
        public ActionResult Pendencias()
        {
            ViewBag.Message = "";
            return View("~/Views/Shared/_Pendencias.cshtml");
        }
        public ActionResult About()
        {
            ViewBag.Message = "";
            ViewBag.UsuarioAtual = UsuarioAtual;
            return View();
        }

        public ActionResult Teste()
        {
            ViewBag.Message = "TESTE";
            return View();
        }

        public ActionResult Inicio()
        {
            ViewBag.Message = "";
            Session["UsuarioAtual_ID"] = UsuarioAtual.ID;
            //ViewBag.UsuarioAtual = UsuarioAtual;
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "";

            return View();
        }
    }
}
