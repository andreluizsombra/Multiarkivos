using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MK.Easydoc.Core.Entities
{
    public class Perfil
    {
        public int ID { get; set; }
        public string Descricao { get; set; }
        public int idServico { get; set; }
        public int idPerfil { get; set; }
        public string idModulo { get; set; }
        public int qtdeModulo { get; set; }
        public int TipoAcao { get; set; }

    }
}
