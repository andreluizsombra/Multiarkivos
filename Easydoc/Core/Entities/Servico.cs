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
        public int IdCliente { get; set; }
        public List<Modulo> Modulos { get; set; }
        public List<DocumentoModelo> Documentos { get; set; }

        public Servico()
        {
            ID = 0;
            Descricao = string.Empty;
            Default = false;
            IdCliente = 0;
            //Cliente = new Cliente();
            Modulos = new List<Modulo>();
            Documentos = new List<DocumentoModelo>();
        }
    }



}