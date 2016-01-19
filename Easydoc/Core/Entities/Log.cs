using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MK.Easydoc.Core.Entities
{
    public class Log
    {
        public decimal IDLOG { get; set; }
        public string Cliente { get; set; }
        public string Servico { get; set; }
        public string Acao { get; set; }
        public string Localizador { get; set; }
        public DateTime DataHora { get; set; }

        public Log() { }

    }
}
