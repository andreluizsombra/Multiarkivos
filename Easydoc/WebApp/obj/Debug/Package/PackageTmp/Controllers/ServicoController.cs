using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MK.Easydoc.Core.Entities;
using MK.Easydoc.Core.Helpers;
using MK.Easydoc.Core.Services.Interfaces;

namespace MK.Easydoc.WebApp.Controllers
{
    public class ServicoController : BaseController
    {
        #region Private Read-Only Fields

        private readonly IServicoService _servicoService;
        private readonly IClienteService _clienteService;
        #endregion

        #region Constructors

        public ServicoController()
        {
            this._servicoService = DependencyResolver.Current.GetService<IServicoService>();
            this._clienteService = DependencyResolver.Current.GetService<IClienteService>();
        }

        #endregion

        #region Private Methods

        private void DefineServicoAtual(Usuario usuario, int idServico)
        {
            try
            {
                var _servico = _servicoService.GetServico(usuario.ID, idServico);

                if (_servico == null)
                    throw new Exception("Não foi possível selecionar a Cliente.");


                UserDataCookieHelper.CreateUserDataCookie(new UserDataEntity()
                {
                    IdClienteAtual = _servico.IdCliente,
                    ClienteAtual = _clienteService.GetCliente(usuario.ID, _servico.IdCliente),
                    UrlCSSClienteAtual = Url.Content("~/assets/themes/empresa02-theme.css")
                });
            }
            catch (Exception) { throw; }
        }

        #endregion

    }
}
