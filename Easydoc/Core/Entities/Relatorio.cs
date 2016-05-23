using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MK.Easydoc.Core.Entities
{
    public class Relatorio
    {
        public int idServico { get; set; }
        public int idRelatorio { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public string ScriptSQLRelatorio { get; set; }
        public List<RelatorioParametros> ListaRelatorioParametro { get; set; }
        public Relatorio()
        {
            Titulo = String.Empty;
            Descricao = String.Empty;
            ScriptSQLRelatorio = String.Empty;
            ListaRelatorioParametro = new List<RelatorioParametros>();
        }
    }
}
