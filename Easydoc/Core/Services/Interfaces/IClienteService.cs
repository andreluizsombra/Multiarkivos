using System;
using System.Collections.Generic;
using MK.Easydoc.Core.Entities;

namespace MK.Easydoc.Core.Services.Interfaces
{
    public interface IClienteService
    {
        #region Methods
        List<Cliente> ListarClientesUsuario(int Usuario_ID);
        Cliente GetCliente(int Usuario_ID, int Cliente_ID);
        #endregion Methods
    }
}

