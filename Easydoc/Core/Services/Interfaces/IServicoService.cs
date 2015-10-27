using System;
using System.Collections.Generic;
using MK.Easydoc.Core.Entities;

namespace MK.Easydoc.Core.Services.Interfaces
{
    public interface IServicoService
    {
        #region Methods
        List<Servico> ListarServicosUsuario(int Usuario_ID);
        Servico GetServico(int Usuario_ID, int Servico_ID);
        #endregion Methods
    }
}
