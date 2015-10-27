using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MK.Easydoc.Core.Entities;
using MK.Easydoc.Core.Helpers;
using MK.Easydoc.Core.Services.Interfaces;
using MK.Easydoc.Core.Repositories;

namespace MK.Easydoc.WebApp.Controllers
{
    [Authorize]
    public class ClienteController :  BaseController
    {
        #region Private Read-Only Fields

        private readonly IUsuarioService _usuarioService;
        private readonly IServicoService _servicoService;
        #endregion

        #region Constructors

        public ClienteController()
        {
            this._servicoService = DependencyResolver.Current.GetService<IServicoService>();
            this._usuarioService = DependencyResolver.Current.GetService<IUsuarioService>();
            
        }

        #endregion

        #region Private Methods

        private void DefineClienteAtual(Usuario usuario, int idCliente)
        {                        
            try
            {
                UserDataCookieHelper.CreateUserDataCookie(new UserDataEntity() { 
                    IdClienteAtual = idCliente,
                    ClienteAtual = usuario.Clientes.Where(c => c.ID == idCliente).FirstOrDefault(), //(new Cliente {ID= Servico.IdCliente}), //Servico.Cliente, 
                    UrlCSSClienteAtual = Url.Content("~/assets/themes/sanofi-theme.css") 
                });
            }
            catch (Exception) { throw; }
        }

        #endregion

        #region Public Methods

        public ActionResult Lista()
        {
            var listaCliente = new ClienteRepository().ListaClientePorUsuario(Session["NomeUsuario"].ToString());
            //ViewBag.Teste = listaCliente;
            var lista = new SelectList(
                            listaCliente.ToList(),
                            "ID",
                            "Descricao"
                        );
            ViewBag.ListaCliente = lista;
            return View("Lista");
        }
        public ActionResult ListaServico(string idCliente)
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
            var listaServico = new ClienteRepository().ListaServicoPorCliente(Session["NomeUsuario"].ToString(),idCliente);
            
            var lista = new SelectList(
                            listaServico.ToList(),
                            "ID",
                            "Descricao"
                        );
            ViewBag.ListaServico = lista;

            return View("Lista");
        }

        [HttpPost]
        public ActionResult Confirma(FormCollection frm)
        {
            string selcli = frm["cliente"].ToString();
            string selsrv = frm["servico"].ToString();

            int _idcliente = int.Parse(frm["idcliente"].ToString());
            int _idservico = int.Parse(frm["idservico"].ToString());

            Session["NomeCliente"] = selcli;
            Session["NomeServico"] = selsrv;

            AjaxCallTrocarClienteAtual(_idcliente);
            AjaxCallTrocarServicoAtual(_idservico);
            
            return RedirectToRoute("Principal");
        } 
        //
        // GET: /Cliente/
        public ActionResult Index()
        {
            var usuario = default(Usuario);

            try
            {
                usuario = this._usuarioService.GetUsuario(User.Identity.Name);
                //usuario.Servicos = _servicoService.ListarServicosUsuario(usuario.ID);

                if (usuario.Clientes.Count == 1)
                {
                    this.DefineClienteAtual(usuario, usuario.Clientes[0].ID);
                    return RedirectToAction("Index", new { area = "", controller = "Home" });
                    //return RedirectToAction("CriarNovo", new { area = "Pedidos", controller = "Pedido" });
                }                
            }
            catch (Exception ex) { ModelState.AddModelError("error", ex); }
         
            return View();
        }

        //
        // GET: /Cliente/        
        public ActionResult SelecionarCliente(int id)
        {
            var usuario = default(Usuario);            
            
            try
            {
                usuario = this._usuarioService.GetUsuario(User.Identity.Name);
                //usuario.Servicos = _servicoService.ListarServicosUsuario(id);

                this.DefineClienteAtual(usuario, id);
                return RedirectToAction("Index", new { area = "", controller = "Home" });                                                
                //return RedirectToAction("CriarNovo", new { area = "Pedidos", controller = "Pedido" });
            }
            catch (Exception ex) { ModelState.AddModelError("error", ex); }

            return View("Index");
        }

        #endregion
    }
}
