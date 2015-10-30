using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MK.Easydoc.Core.Entities;
using MK.Easydoc.Core.Repositories.Interfaces;
using MK.Easydoc.Core.Services.Interfaces;

namespace MK.Easydoc.Core.Services
{
    public sealed class ServicoService : IServicoService
    {
        #region Private Fields

        //private readonly ILog _logger;
        private IServicoRepository _repository;
        private IDictionary<string, object> _queryParams;

        #endregion

        #region Public Constructors

        //public UsuarioService(ILog logger, IUsuarioRepository repository)
        public ServicoService(IServicoRepository repository)
        {
            //this._logger = logger;
            this._repository = repository;
            this._queryParams = new Dictionary<string, object>();
        }

        #endregion

        #region IUserService Members
        public List<Servico> ListarServicosUsuario(int Usuario_ID) {

            try
            {
                this._queryParams.Clear();
                this._queryParams["Usuario_ID"] = Usuario_ID;

                return this._repository.ListarServicosUsuario(this._queryParams);
            }
            catch (Exception ex) { throw ex; }
        }

        public Servico GetServico(int Usuario_ID, int Servico_ID)
        {

            try
            {
                this._queryParams.Clear();
                this._queryParams["Usuario_ID"] = Usuario_ID;
                this._queryParams["Servico_ID"] = Servico_ID;

                return this._repository.GetServico(this._queryParams);
            }
            catch (Exception ex) { throw ex; }
        } 

        #endregion IUserService Members

    }
}
