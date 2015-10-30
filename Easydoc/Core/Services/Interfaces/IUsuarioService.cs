using System;
using MK.Easydoc.Core.Entities;

namespace MK.Easydoc.Core.Services.Interfaces
{
    public interface IUsuarioService
    {
        #region Methods

        bool ValidarUsuario(string nomeUsuario, string senha);
        Usuario GetUsuario(string nomeUsuario);

        #endregion
    }
}
