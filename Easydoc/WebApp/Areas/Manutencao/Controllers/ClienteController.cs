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
            RegistrarLOGSimples(7, 20, UsuarioAtual.NomeUsuario);
            // LOG: Entrou no modulo Manutencao/Cliente

            //Não exbir lista
            //var Clientes = new ClienteRepository().ListarClientesUsuario(UsuarioAtual.ID);
            //ViewBag.ListaClientes = Clientes.ToList();
            
            return View();
        }

        public ActionResult Pesquisa(FormCollection frm)
        {
            int _tipo = int.Parse(frm["selTipo"].ToString());
            int _condicao = int.Parse(frm["selCondicao"].ToString());
            string _txtpesquisa = frm["txtpesquisa"].ToString();
            var Clientes = new ClienteRepository().PesquisaCliente(_tipo, _condicao, UsuarioAtual.ID, _txtpesquisa, UsuarioAtual.ID);
            ViewBag.ListaClientes = Clientes.ToList();    
            return View("Index");
        }

        public ActionResult Create()
        {
            return View();
        }

        public ActionResult Edit(string cpfcnpj)
        {
            ViewBag.Cliente = new ClienteRepository().GetClienteCPFCNPJ(cpfcnpj, UsuarioAtual.ID);
            return View("Edit");
        }

        [HttpPost]
        public ActionResult Alterar(FormCollection frm)
        {
            try
            {
                var cli = new Cliente()
                {
                    TipoAcao = 2,
                    CPF_CNPJ = Decimal.Parse(frm["cpfcnpj"].ToString()),
                    Descricao = frm["nome"].ToString(),
                    Status = frm["status"]==null ? 0 : int.Parse(frm["status"].ToString()),
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
                TempData["Msg"] = Retorno.Mensagem;
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                TempData["Error"] = ex.Message;
                return View("Create");
                //throw new Exception(ex.Message); 
            }
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
                ViewBag.Cliente = cli;
                var RetCPF = new UsuarioRepository().VerificaCPFDisponivel(UsuarioAtual.ID, decimal.Parse(frm["cpfcnpj"].ToString()));
                if (RetCPF.CodigoRetorno == 1)
                    throw new Exception(RetCPF.Mensagem);

                var cliRet = new ClienteRepository();
                Retorno = cliRet.Incluir(cli);
                if (Retorno.CodigoRetorno < 0)
                {
                    throw new Exception(Retorno.Mensagem);
                }
                TempData["Msg"] = Retorno.Mensagem;
                return RedirectToAction("Index");
            }
            catch(Exception ex)
            { 
                ViewBag.Error = ex.Message;
                TempData["Error"] = ex.Message;
                return View("Create",frm);
                //throw new Exception(ex.Message); 
            }
        }

        [HttpPost]
        public JsonResult AjaxExcluir(string cpf_cnpj)
        {
            try
            {
                var cli = new Cliente()
                {
                    TipoAcao = 3, //TipoAcao 3 é Exclusão
                    CPF_CNPJ = Decimal.Parse(cpf_cnpj),
                    idUsuarioAtual = UsuarioAtual.ID
                };
                var cliRet = new ClienteRepository();
                var Retorno = cliRet.Incluir(cli);
                if (Retorno.CodigoRetorno < 0)
                {
                    throw new Exception(Retorno.Mensagem);
                }
                //ViewBag.Msg = Retorno.Mensagem;
                return Json(Retorno, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //ViewBag.Error = ex.Message;
                //throw new Exception(ex.Message);
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
