using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MK.Easydoc.Core.Entities;
using MK.Easydoc.Core.Repositories.Interfaces;
using MK.Easydoc.Core.Services.Interfaces;

namespace MK.Easydoc.Core.Services
{
    public class ClienteService : IClienteService
    {
        #region Private Fields

        //private readonly ILog _logger;
        private IClienteRepository _repository;
        private IDictionary<string, object> _queryParams;

        #endregion

        #region Public Constructors

        //public UsuarioService(ILog logger, IUsuarioRepository repository)
        public ClienteService(IClienteRepository repository)
        {
            //this._logger = logger;
            this._repository = repository;
            this._queryParams = new Dictionary<string, object>();
        }

        #endregion

        #region IClienteService Members
        public List<Cliente> ListarClientesUsuario(int Usuario_ID)
        {

            try
            {
                this._queryParams.Clear();
                this._queryParams["Usuario_ID"] = Usuario_ID;

                return this._repository.ListarClientesUsuario(this._queryParams);
            }
            catch (Exception ex) { throw ex; }
        }

        public Cliente GetCliente(int Usuario_ID, int Cliente_ID)
        {

            try
            {
                this._queryParams.Clear();
                this._queryParams["Usuario_ID"] = Usuario_ID;
                this._queryParams["Cliente_ID"] = Cliente_ID;

                return this._repository.GetCliente(this._queryParams);
            }
            catch (Exception ex) { throw ex; }
        }

        #endregion IClienteService Members


    }
}
