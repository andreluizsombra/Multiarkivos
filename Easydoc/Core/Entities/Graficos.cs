using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MK.Easydoc.Core.Entities
{
    public class AcessoModulo
    {
        public int idModulo { get; set; }
        public int Habilitado { get; set; }
    }
    public class Graficos
    {
        public string label { get; set; }
        public int value { get; set; }
        public string Header { get; set; }
        public Graficos()
        {
            label = string.Empty;
            value = 0;
            Header = string.Empty;
        }
    }
    //TODO: AndreSombra 18/11/2015
    public class GraficoAreaChart
    {
        public string Data_Captura { get; set; }
        public int Quantidade_Capturada { get; set; }
        public string Header { get; set; }
        public GraficoAreaChart()
        {
            Data_Captura = string.Empty;
            Quantidade_Capturada = 0;
            Header = string.Empty;
        }
    }

    public class GraficoPendencias
    {
        public string Descricao { get; set; }
        public int Quantidade { get; set; }
        public string Header { get; set; }
        public GraficoPendencias()
        {
            Descricao = string.Empty;
            Quantidade = 0;
            Header = string.Empty;
        }
    }
}
