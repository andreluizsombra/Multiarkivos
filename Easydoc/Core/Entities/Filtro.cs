using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MK.Easydoc.Core.Entities
{
    public class Filtro
    {
        public int Tipo { get; set; }
        public int Condicao { get; set; }
        public int IdClienteAtual { get; set; }
        public string Pesquisa { get; set; }
        public int IdUsuarioAtual { get; set; }
        public int IdServico { get; set; }
        public int IdCliente { get; set; }
        public int PesquisaPor { get; set; }
        public string Localizador { get; set; }
    }
}
