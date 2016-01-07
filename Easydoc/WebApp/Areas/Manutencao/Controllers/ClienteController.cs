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
            try
            {
                var cli = new Cliente()
                {
                    TipoAcao=1,
                    CPF_CNPJ = Decimal.Parse(frm["cpfcnpj"].ToString()),
                    Descricao = frm["nome"].ToString(),
                    Status = int.Parse(frm["status"].ToString()),
                    QtdeUsuario = int.Parse(frm["qtdusu"].ToString()),
                    EmailPrincipal = frm["email"].ToString(),
                    idUsuarioAtual = UsuarioAtual.ID
                };
                var cliRet = new ClienteRepository();
                var Retorno = cliRet.Incluir(cli);
                if (Retorno.CodigoRetorno < 0)
                {
                    throw new Exception(Retorno.Mensagem);
                }
                ViewBag.Msg = Retorno.Mensagem;
                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                ViewBag.Error = ex.Message;
                throw new Exception(ex.Message); 
            }
        }
    }
}
