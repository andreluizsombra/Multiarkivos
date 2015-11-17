using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MK.Easydoc.Core.Entities
{
    public class Graficos
    {
        public string label { get; set; }
        public int value { get; set; }
        public Graficos()
        {
            label = string.Empty;
            value = 0;
        }
    }
}
