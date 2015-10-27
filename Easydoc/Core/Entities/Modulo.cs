using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MK.Easydoc.Core.Entities
{
    public class Modulo
    {
        public int ID { get; set; }
        public string Descricao { get; set; }
        public string RotaURL { get; set; }

        public Modulo() {
            ID = 0;
            Descricao = string.Empty;
            RotaURL = string.Empty;
        }
    }
}