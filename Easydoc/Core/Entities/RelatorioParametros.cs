using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MK.Easydoc.Core.Entities
{
    public class RelatorioParametros
    {
        public string Descricao { get; set; }
        public string MascaraEntrada { get; set; }
        public string ProcSqlValidacao { get; set; }
        public string ControleWEB { get; set; }
        public string TipoSQL { get; set; }
        public string RotuloAbreviado { get; set; }
        public int MaxLength { get; set; }
        public string MascaraSaida { get; set; }
        public RelatorioParametros()
        {
            Descricao = String.Empty;
            MascaraEntrada = String.Empty;
            ProcSqlValidacao = String.Empty;
            ControleWEB = String.Empty;
            TipoSQL = String.Empty;
            RotuloAbreviado = String.Empty;
            MaxLength = 0;
            MascaraSaida = String.Empty;
        }
    }
}
