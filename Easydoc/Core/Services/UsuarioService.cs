using System;
using System.Linq;
using System.Collections.Generic;
using MK.Easydoc.Core.Services.Interfaces;
using MK.Easydoc.Core.Repositories.Interfaces;
using MK.Easydoc.Core.Entities;

namespace MK.Easydoc.Core.Services
{
    public sealed class UsuarioService : IUsuarioService
    {
        #region Private Fields

        //private readonly ILog _logger;
        private IUsuarioRepository _repository;
        private IServicoService _servico;
        private IClienteService _cliente;
        private IDictionary<string, object> _queryParams;

        #endregion

        #region Public Constructors

        //public UsuarioService(ILog logger, IUsuarioRepository repository)
        public UsuarioService( IUsuarioRepository repository)
        {
            //this._logger = logger;
            this._repository = repository;
            this._queryParams = new Dictionary<string, object>();
        }
        public UsuarioService(IUsuarioRepository repository, IServicoService servico, IClienteService cliente)
        {
            //this._logger = logger;
            this._repository = repository;
            this._queryParams = new Dictionary<string, object>();
            _servico = servico;
            _cliente = cliente;
        }
        #endregion

        #region IUserService Members

        public bool ValidarUsuario(string nomeUsuario, string senha)
        {
            try
            {
                if (String.IsNullOrEmpty(nomeUsuario) || String.IsNullOrEmpty(senha))
                    throw new ArgumentNullException("O login de acesso e senha são de preenchimentos obrigatório.");

                    this._repository.ValidarUsuario(new Usuario() { NomeUsuario = nomeUsuario, Senha = senha });
            }

            catch (ArgumentNullException ex) { throw ex; }
            catch (Exception ex)
            {
                //this._logger.Error(ex.Message, ex);
                //throw new Exception("Login ou senha inválida. Favor verificar!");
                throw new Exception(ex.Message);
            }

            return true;
        }

        public Usuario GetUsuario(string nomeUsuario)
        {
            try
            {

                Usuario _usuario = new Usuario();
                this._queryParams.Clear();
                this._queryParams["nomeUsuario"] = nomeUsuario;

                //TODO: Andre 27/04/2016
                Usuario clsusu = new Usuario();
                if (System.Web.HttpContext.Current.Session==null || (Usuario)System.Web.HttpContext.Current.Session["ClsUsuario"] == null)
                    clsusu = this._repository.GetUsuarioSessao(nomeUsuario);
                else clsusu = (Usuario)System.Web.HttpContext.Current.Session["ClsUsuario"];

                _usuario = clsusu; //_usuario = this._repository.GetUsuario(this._queryParams);
                _usuario.Clientes = new List<Cliente>();
                _usuario.Clientes.AddRange(_cliente.ListarClientesUsuario(_usuario.ID));


                //List<Servico> _sevs = new List<Servico>();

               /* var _servs = _servico.ListarServicosUsuario(_usuario.ID);
                
                foreach (Cliente _cli in _usuario.Clientes)
                {
                    var _servcli = (from s in _servs where s.IdCliente == _cli.ID select s).ToList<Servico>();
                    _cli.Servicos.AddRange(_servcli);
                }
                */

                //_usuario.Servicos = new List<Servico>();
                //_usuario.Servicos.AddRange(_servico.ListarServicosUsuario(_usuario.ID));

                return _usuario;
            }
            catch (Exception ex) {throw ex; }
            //catch (Exception ex) { this._logger.Error(ex.Message, ex); throw ex; }
        }

        #endregion
    }
}
