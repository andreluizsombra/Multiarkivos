using System;
using System.Collections.Generic;
using MK.Easydoc.Core.Entities;

namespace MK.Easydoc.Core.Repositories.Interfaces
{
    public interface IClienteRepository
    {
        List<Cliente> ListarClientesUsuario(IDictionary<string, object> _queryParams);
        Cliente GetCliente(IDictionary<string, object> _queryParams);
    }
}
