using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MK.Easydoc.Core.Entities
{
    public class Servico
    {
        public int ID { get; set; }
        public string Descricao { get; set; }
        public bool Default { get; set; }
        //public Cliente Cliente { get; set; }
        public int TipoAcao { get; set; }
        public int IdUsuarioAtual { get; set; }
        public int IdCliente { get; set; }
<<<<<<< HEAD


        public string NomeCliente { get; set; }
        public bool ServicoDefault { get; set; }
        public bool ArquivoDados { get; set; }
        public bool ControleAtencao { get; set; }

=======
        public string NomeCliente { get; set; }
        public int ServicoDefault { get; set; }
        public int ArquivoDados { get; set; }
        public int ControleAtencao { get; set; }
>>>>>>> 9c14b3e5243ff18ed9e1a853bf2100e3b6e4b869
        public List<Modulo> Modulos { get; set; }
        public List<DocumentoModelo> Documentos { get; set; }
        public string ScriptSQLDashboard_Captura { get; set; }
        public string ScriptSQLDashboard_Pendencias { get; set; }
        public string ScriptSQLDashboard_Doc_Modulo { get; set; }

        public Servico()
        {
            ID = 0;
            Descricao = string.Empty;
            Default = false;
            IdCliente = 0;
            //Cliente = new Cliente();
            Modulos = new List<Modulo>();
            Documentos = new List<DocumentoModelo>();
            NomeCliente = string.Empty;
            ServicoDefault = 0;
            ArquivoDados = 0;
            ControleAtencao = 0; 
            ScriptSQLDashboard_Captura = string.Empty;
            ScriptSQLDashboard_Pendencias = string.Empty;
            ScriptSQLDashboard_Doc_Modulo = string.Empty;

            NomeCliente = string.Empty;
            ServicoDefault = false;
            ArquivoDados = false;
            ControleAtencao = false;
        }
    }



}