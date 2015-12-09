using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MK.Easydoc.WebApp.Controllers;
using MK.Easydoc.Core.Repositories;

namespace MK.Easydoc.WebApp.Areas.Seguranca.Controllers
{
    public class UsuarioController : BaseController
    {
        // GET: /Seguranca/Usuario/
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ListaUsuarios(FormCollection frm)
        {
            int _tipo = int.Parse(frm["selTipo"].ToString());
            int _condicao = int.Parse(frm["selCondicao"].ToString());
            string _txtpesquisa = frm["txtpesquisa"].ToString();

            //int idcliente = new ClienteRepository().GetCliente()

            var usu = new UsuarioRepository().GetUsuarioCadastro(_tipo, _condicao, ClienteAtual.ID, _txtpesquisa);
            ViewBag.ListaUsuarios = usu;
            return View("ManutencaoUsuario");
        }

        // GET: /Seguranca/ManutencaoUsuario/
        public ActionResult ManutencaoUsuario()
        {
            return View();
        }

        //
        // GET: /ControleAcesso/Usuario/
        public ActionResult MinhaConta()
        {
            ViewBag.FeedbackTrocaSenha = String.Empty;
            ViewBag.FeedbackOffline = "<span class='label label-important'>Não utiliza o sistema!</span>";

            return View();
        }

        //
        // GET: /ControleAcesso/Usuario/
        public ActionResult TrocarSenha()
        {
            ViewBag.FeedbackTrocaSenha = string.Empty;
            return View();
        }

        //
        // POST: /ControleAcesso/Usuario/
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AlterarSenha()
        {
            bool _senhaAlterada = true;

            try
            {
                _senhaAlterada = true;
            }
            catch (Exception) { _senhaAlterada = false; }
            finally
            {
                if (_senhaAlterada == true)
                    ViewBag.FeedbackTrocaSenha = "<div class='alert alert-success'> Senha alterada com sucesso! </div>";
                else
                    ViewBag.FeedbackTrocaSenha = "<div class='alert alert-error'> Ocorreu um erro durante a troca de senha! </div>";
            }

            return View("TrocarSenha");
        }
    }
}
