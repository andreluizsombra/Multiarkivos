using System;
using System.Collections.Generic;
using MK.Easydoc.Core.Entities;
using MK.Easydoc.Core.Infrastructure;

namespace MK.Easydoc.Core.Repositories.Interfaces
{
    public interface IUsuarioRepository
    {
        #region Methods

        bool ValidarUsuario(Usuario usuario);
        Retorno VerificaServicoPerfil(int idUsuario, string login);
        Usuario GetUsuario(IDictionary<string, object> _queryParams);

        #endregion
    }
}
