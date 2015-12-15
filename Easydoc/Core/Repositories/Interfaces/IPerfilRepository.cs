using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MK.Easydoc.Core.Entities;

namespace MK.Easydoc.Core.Repositories.Interfaces
{
    public interface IPerfilRepository
    {
        List<Perfil> ListaPerfil(int idServico);
    }
}
