using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MK.Easydoc.Core.Entities;
using MK.Easydoc.Core.Repositories;
using MK.Easydoc.Core.Infrastructure;
using MK.Easydoc.WebApp.Controllers;


namespace MK.Easydoc.WebApp.Areas.Manutencao.Controllers
{
    public class OcorrenciaController : BaseController
    {
        //
        // GET: /Manutencao/Ocorrencia/

        public ActionResult Index()
        {
            RegistrarLOGSimples(20, 18, UsuarioAtual.NomeUsuario);
            //ListaClienteServico(IdCliente_Atual.ToString(), IdServico_Atual);
            CarregarComboCliente();
            ViewBag.idCliente = -1; //Selecione
            //ViewBag.idServico = IdServico_Atual;
            List<Ocorrencia> lst = null;
            return View(lst);
        }
        public ActionResult ListaServico(string idCliente)
        {
            ListaClienteServico(idCliente);

            return View("Index");
        }
        public ActionResult ListaServicoNovaOcorrencia(string idCliente)
        {
            ListaClienteServico(idCliente);

            return View("Create");
        }
        public ActionResult ListaOcorrencias()
        {
            var lstOcorrencia = new OcorrenciaRepository().PesquisaServicoCliente((Filtro)Session["Filtro"]);
            CarregarComboCliente();
            return View("Index", lstOcorrencia);
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
        void CarregarComboCliente()
        {
            var listaCliente = new ClienteRepository().ListaClientePorUsuario(Session["NomeUsuario"].ToString());
            var lista = new SelectList(
                            listaCliente.ToList(),
                            "ID",
                            "Descricao"
                        );
            ViewBag.ListaCliente = lista;
        }
        public ActionResult Pesquisa(FormCollection frm)
        {
            int _idcli = int.Parse(frm["selCliente"].ToString());
            int _idserv = int.Parse(frm["selServico"].ToString());
            
            Session["Filtro"] = new Filtro() { IdCliente = _idcli, IdServico=_idserv, IdUsuarioAtual = UsuarioAtual.ID };
            var lstOcorrencia = new OcorrenciaRepository().PesquisaServicoCliente((Filtro)Session["Filtro"]);

            CarregarComboCliente();
            return View("Index", lstOcorrencia);
        }

        [HttpPost]
        public ActionResult Incluir(FormCollection frm)
        {
            try
            {
                if (Session["Filtro"] == null) { RedirectToRoute("Logout"); }

                var ocr = new Ocorrencia()
                {
                    TipoAcao = 1,
                    descOcorrencia = frm["nomeocorrencia"].ToString(),
                    idServico = int.Parse(frm["idservico"].ToString()),
                    IdOcorrencia = 0,
                    idUsuario = UsuarioAtual.ID
                };

                var Ret = new OcorrenciaRepository();
                var Retorno = Ret.Incluir(ocr);
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
                return RedirectToAction("Create");
            }
        }

        [HttpPost]
        public ActionResult Alterar(FormCollection frm)
        {
            try
            {
                if (Session["Filtro"] == null) { RedirectToRoute("Logout"); }

                var ocr = new Ocorrencia()
                {
                    TipoAcao = 2,
                    descOcorrencia = frm["nomeocorrencia"].ToString(),
                    idServico = int.Parse(frm["idservico"].ToString()),
                    IdOcorrencia = int.Parse(frm["idocorrencia"].ToString()),
                    idUsuario = UsuarioAtual.ID
                };

                var Ret = new OcorrenciaRepository();
                var Retorno = Ret.Incluir(ocr);
                if (Retorno.CodigoRetorno < 0)
                {
                    throw new Exception(Retorno.Mensagem);
                }
                ViewBag.Msg = Retorno.Mensagem;
                TempData["Msg"] = Retorno.Mensagem;
                return RedirectToAction("ListaOcorrencias");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                TempData["Error"] = ex.Message;
                return RedirectToAction("ListaOcorrencias");
            }
        }

        [HttpPost]
        public JsonResult AjaxExcluir(string idocorrencia, string idservico, string nomeocorrencia)
        {
            try
            {
                var ocr = new Ocorrencia()
                {
                    TipoAcao = 3, //Excluir Ocorrencia
                    descOcorrencia = nomeocorrencia,
                    idServico = int.Parse(idservico),
                    IdOcorrencia = int.Parse(idocorrencia),
                    idUsuario = UsuarioAtual.ID
                };

                var Ret = new OcorrenciaRepository();
                var Retorno = Ret.Incluir(ocr);
                if (Retorno.CodigoRetorno < 0)
                {
                    throw new Exception(Retorno.Mensagem);
                }
                //ViewBag.Msg = Retorno.Mensagem;
                return Json(Retorno, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
        //
        // GET: /Manutencao/Ocorrencia/Create

        public ActionResult Create()
        {
            CarregarComboCliente();
            ViewBag.idCliente = -1; //Selecione
            List<Ocorrencia> lst = null;
            return View(lst);
        }

        //
        // POST: /Manutencao/Ocorrencia/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Manutencao/Ocorrencia/Edit/5

        public ActionResult Edit(string id, string nomeocorrencia, string idservico)
        {
            ViewBag.idOcorrencia = id;
            ViewBag.nomeOcorrencia = nomeocorrencia;
            ViewBag.idServico = idservico;
            return View();
        }

        //
        // POST: /Manutencao/Ocorrencia/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Manutencao/Ocorrencia/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Manutencao/Ocorrencia/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
