using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MK.Easydoc.WebApp.Controllers;
using MK.Easydoc.Core.Entities;
using MK.Easydoc.Core.Repositories;
using MK.Easydoc.Core.Infrastructure;

namespace MK.Easydoc.WebApp.Areas.Manutencao.Controllers
{
    public class ClienteController : BaseController
    {
        public ActionResult Index()
        {
            var Clientes = new ClienteRepository().ListarClientesUsuario(UsuarioAtual.ID);
            ViewBag.ListaClientes = Clientes.ToList();
            return View();
        }
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Incluir(FormCollection frm)
        {
            var Retorno = new Retorno();
            try
            {
                var cli = new Cliente()
                {
                    TipoAcao=1,
                    CPF_CNPJ = Decimal.Parse(frm["cpfcnpj"].ToString()),
                    Descricao = frm["nome"].ToString(),
                    Status = frm["status"]==null ? 0 : int.Parse(frm["status"].ToString()),
                    QtdeUsuario = int.Parse(frm["qtdusu"].ToString()),
                    EmailPrincipal = frm["email"].ToString(),
                    idUsuarioAtual = UsuarioAtual.ID
                };
                var cliRet = new ClienteRepository();
                Retorno = cliRet.Incluir(cli);
                if (Retorno.CodigoRetorno < 0)
                {
                    throw new Exception(Retorno.Mensagem);
                }
                //ViewBag.Msg = Retorno.Mensagem;
                TempData["Msg"] = Retorno.Mensagem;
                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                TempData["Error"] = Retorno.Mensagem;
                throw new Exception(ex.Message); 
            }
        }
    }
}
