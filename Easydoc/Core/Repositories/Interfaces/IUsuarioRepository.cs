using System;
using System.Collections.Generic;
using MK.Easydoc.Core.Entities;

namespace MK.Easydoc.Core.Repositories.Interfaces
{
    public interface IUsuarioRepository
    {
        #region Methods

        bool ValidarUsuario(Usuario usuario);
        Usuario GetUsuario(IDictionary<string, object> _queryParams);

        #endregion
    }
}
