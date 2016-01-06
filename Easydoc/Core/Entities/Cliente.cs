using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MK.Easydoc.Core.Entities
{
    public class Cliente
    {
        public int ID { get; set; }
        public decimal CPF_CNPJ { get; set; }
        public string Descricao { get; set; }
        public int Status { get; set; }
        public int QtdeUsuario { get; set; }
        public string EmailPrincipal { get; set; }
        public List<Servico> Servicos { get; set; }
        public string UrlCSS { get; set; }

        public Cliente() {
            ID = 0;
            Descricao = string.Empty;
            UrlCSS = string.Empty;
            Servicos = new List<Servico>();
        }

    }
}