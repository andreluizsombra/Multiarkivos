using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MK.Easydoc.Core.Repositories.Interfaces
{
    public interface IStoragePrivate
    {
        void GetPath(int idCliente);
        void CriarDiretorio(string arquivoLocal, string arquivo);
    }
}
