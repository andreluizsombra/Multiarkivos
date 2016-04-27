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
        Retorno VerificaServicoPerfil(string login, string senha);
        Usuario GetUsuario(IDictionary<string, object> _queryParams);
        Usuario GetUsuarioSessao(string UserName, int idServico = 0); //TODO: Andre 27/04/2016
        Usuario RetornaUsuarioSessao(); //TODO: Andre 27/04/2016

        #endregion
    }
}
