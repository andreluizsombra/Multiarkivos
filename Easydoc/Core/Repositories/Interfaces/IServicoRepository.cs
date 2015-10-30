using System;
using System.Collections.Generic;
using MK.Easydoc.Core.Entities;

namespace MK.Easydoc.Core.Repositories.Interfaces
{
    public interface IServicoRepository
    {
        List<Servico> ListarServicosUsuario(IDictionary<string, object> _queryParams);
        Servico GetServico(IDictionary<string, object> _queryParams);
    }
}
