using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MK.Easydoc.Core.Entities
{
    public class Licensa
    {
        public int TipoAcao { get; set; }
        public int idCliente { get; set; }
        public int idServico { get; set; }
        public int idUsuario { get; set; }
        public string IPServerHost { get; set; }
        public string HostnameServer { get; set; }
        public string IPPublicClient { get; set; }
        public string IPPrivateClient { get; set; }
        public string HostnameClient { get; set; }

        public Licensa()
        {
            TipoAcao = -1;
            IPServerHost="";
            HostnameServer ="";
            IPPublicClient ="";
            IPPrivateClient ="";
            HostnameClient = "";
        }
    }
}
