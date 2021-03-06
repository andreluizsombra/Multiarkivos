﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using MK.Easydoc.Core.Services.Interfaces;
using MK.Easydoc.WebApp.Controllers;
using MK.Easydoc.Core.Entities;
using MK.Easydoc.Core.Repositories;
using MK.Easydoc.Core.Infrastructure;
using MK.Easydoc.WebApp.ViewModels;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.IO;


namespace MK.Easydoc.WebApp.Areas.Documento.Controllers
{
    public class VinculoController : BaseController
    {
        #region Private Read-Only Fields

        private readonly ILoteService _loteService;
        private readonly IDocumentoService _docService;
        
        #endregion
        public VinculoController() //Método construtor da classe VnculoController
        {
            _loteService = DependencyResolver.Current.GetService<ILoteService>();
            _docService = DependencyResolver.Current.GetService<IDocumentoService>();
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Digitar(int id)
        {
            //RegistrarLOGSimples(4, 14, UsuarioAtual.NomeUsuario);
            // LOG: Entrou no modulo de formalizacao

            bool UsaArquivoDados = _loteService.UsaArquivoDados(ServicoAtual.ID);
            bool EmUso;

            MK.Easydoc.Core.Entities.Documento _documento = new Core.Entities.Documento();
            if (id == 0)
            {
                _documento = _docService.GetDocumentoVincular(UsuarioAtual.ID, ServicoAtual.ID);
                EmUso = _docService.EmUso(_documento.ID, UsuarioAtual.ID, 1, ServicoAtual.ID);

                if (EmUso)
                {
                    _documento.ID = 0;
                    //return View(_documento); 
                }

            }
            else
            {
                _documento = _docService.SelecionaDocumentoDigitar(UsuarioAtual.ID, ServicoAtual.ID, id);
            }

            List<Ocorrencia> _motivo;
            ViewData["dupliciadade"] = "";
            if (_documento.StatusDocumento == 3000)
            {
                _motivo = _docService.GetMotivoDigitar(_documento.Modelo.ID, 1);
            }
            else
            {
                _motivo = _docService.GetMotivoDigitar(_documento.Modelo.ID, 2);
                ViewData["dupliciadade"] = "";
                if (_documento.Modelo.ID == 10)
                {
                    ViewData["dupliciadade"] = _docService.GetDuplicidade(id, ServicoAtual.ID);
                }
            }
            
            //------------------------------------
            if (_documento.ID > 0)
            {
                DocumentoLoteViewModel _documentoView = new DocumentoLoteViewModel();
                _documentoView.Documento = _documento;
                _documentoView.CamposDocumentoJSON = _docService.PesquisarDocumentosModulo(ServicoAtual.ID, _documento.Modelo.ID, "Caixa, Lote", string.Format("IdDocumento={0}", _documento.ID));

                JavaScriptSerializer jss = new JavaScriptSerializer();
                List<DocumentoDetalhe> _docs = jss.Deserialize<List<DocumentoDetalhe>>(_documentoView.CamposDocumentoJSON).ToList<DocumentoDetalhe>();

                if (_documento.StatusDocumento == 3010)
                {
                    ViewData["breadonly"] = "true";
                }
                else
                {
                    ViewData["breadonly"] = "false";
                }

                string status = "";

                switch (_documento.StatusDocumento)
                {
                    case 3000:
                        status = "A digitar";
                        break;
                    case 1020:
                        status = "Supervisão";
                        break;
                    case 1029:
                        status = "Excluído";
                        break;
                    case 3010:
                        status = "Digitado";
                        break;
                    default:
                        status = "";
                        break;
                }

                string det;
                ViewData["Det"] = "";
                foreach (DocumentoDetalhe detalhe in _docs)
                {
                    if (_documento.StatusDocumento == 3000 || _documento.StatusDocumento == 3010)
                    {
                        if (UsaArquivoDados) { ViewData["Det"] = string.Format(status + " | Caixa: {0} | Lote: {1} ", detalhe.Caixa, detalhe.Lote); }
                    }
                    else
                    {
                        det = _docService.PesquisarMotivo(detalhe.IdDocumento, IdServico_Atual);
                        if (UsaArquivoDados)
                        {
                            ViewData["Det"] = string.Format(status + "| Caixa: {0} | Lote: {1} | Motivo: {2}", detalhe.Caixa, detalhe.Lote, det);
                        }
                        else
                        {
                            ViewData["Det"] = string.Format(status + "| Motivo: {0}", det);
                        }
                    }
                }
            }
            ViewData["Just"] = "";
            ViewData["Valida"] = "";
            ViewData["Just"] = "<table>";
            ViewData["Just"] = ViewData["Just"] + "<tr><td>Cod.<td>Descrição</td></tr> ";
            foreach (Ocorrencia motivo in _motivo)
            {

                ViewData["Just"] = ViewData["Just"] + "<tr><td><li>" + motivo.IdOcorrencia + "<td> " + motivo.descOcorrencia + "</td></tr>";
                ViewData["Valida"] = ViewData["Valida"] + motivo.IdOcorrencia.ToString();
            }
            ViewData["Just"] = ViewData["Just"] + "</table>";

            // Se o ID passado como parametr for diferente do retornado pelo metodo, direciona poara sup... 

            if (_documento.StatusDocumento != 3010)
            {
                if (id != 0 && _documento.ID == 0)
                {
                    return RedirectToAction("ListarPendentes", new { area = "Documento", controller = "Supervisao" });
                }
            }

            ViewBag.ListaDocsPai = new DocumentoRepository().ListarDocsVinculoPai(ServicoAtual.ID, _documento.Modelo.ID, 1);   //TODO: 30/06/2016 - Conforme solicitação do Paulo
            ViewBag.ListaOcorrencia = new DocumentoRepository().ListarOcorrencia(IdServico_Atual, 1);
            _documento.Nuvem = new StoragePrivateRepository(IdCliente_Atual).Nuvem; //TODO: 07/07/2016
            return View("Vincular", _documento);
        }

        [HttpPost]
        public JsonResult AjaxMudaStatusDocumento(string idDocumento)
        {
            var ret = new Retorno();
            try
            {
                _docService.MudaStatusDocumento(int.Parse(idDocumento), UsuarioAtual.ID, 3010, ServicoAtual.ID);
                bool EmUso = _docService.EmUso(int.Parse(idDocumento), UsuarioAtual.ID, 2, ServicoAtual.ID);
            }
            catch (Exception erx)
            {
                ret.CodigoRetorno = 1;
                ret.Mensagem = erx.Message;
                return Json(ret, JsonRequestBehavior.AllowGet);
            }
            return Json(ret, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AjaxCallEnviarDocumentoSupervisao(int id_documento, int id_motivo)
        {
            try
            {
                MK.Easydoc.Core.Entities.Documento _doc = (new Core.Entities.Documento { ID = id_documento, StatusDocumento = 1020 });
                _docService.AtualizarDocumento(_doc, ServicoAtual.ID);
                bool EmUso = _docService.EmUso(id_documento, UsuarioAtual.ID, 2, ServicoAtual.ID);
                _docService.IncluirMotivo(IdServico_Atual, id_documento, id_motivo, 1, UsuarioAtual.ID);

                RegistrarLOGSimples(4, 16, UsuarioAtual.NomeUsuario);
                // LOG: Enviou documento a supervisão
            }
            catch (Exception ex) { return Json(new RetornoViewModel(false, ex.Message)); }
            return Json(new RetornoViewModel(true, "Documento enviado para a supervisão!", null));
        }

        [HttpPost]
        public JsonResult AjaxVoltarDocumentoEmUso(string idDocumento)
        {
            var ret = new Retorno();
            try
            {
                bool EmUso = _docService.EmUso(int.Parse(idDocumento), UsuarioAtual.ID, 2, ServicoAtual.ID);
            }
            catch (Exception erx)
            {
                ret.CodigoRetorno = 1;
                ret.Mensagem = erx.Message;
                return Json(ret, JsonRequestBehavior.AllowGet);
            }
            return Json(ret, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AjaxVincular(int id_documento, int id_servico, int id_documentopai)
        {
            var ret = new Retorno();
            try
            {
                new DocumentoRepository().VincularDocumento(new Vinculo() { idDocumento = id_documento, idServico = id_servico, idDocumentoPai = id_documentopai });
                ret.CodigoRetorno = 1;
                ret.Mensagem = "Vinculo efetuado com sucesso.";
            }
            catch (Exception erx)
            {
                ret.CodigoRetorno = -1;
                ret.Mensagem = erx.Message;
                return Json(ret, JsonRequestBehavior.AllowGet);
            }
            return Json(ret, JsonRequestBehavior.AllowGet);
        }
    }
}
