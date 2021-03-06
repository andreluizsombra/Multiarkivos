﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MK.Easydoc.Core.Entities
{
    public class Cliente
    {
        public int ID { get; set; }
        public decimal CPF_CNPJ { get; set;}
        public string Descricao { get; set; }
        public int Status { get; set; }
        public int QtdeUsuario { get; set; }
        public string EmailPrincipal { get; set; }
        public List<Servico> Servicos { get; set; }
        public string UrlCSS { get; set; }
        public int TipoAcao { get; set; }
        public int idUsuarioAtual { get; set; }
        public int IdCliente { get; set; }
        public int IdServico { get; set; }

        public Cliente() {
            ID = 0;
            Descricao = string.Empty;
            UrlCSS = string.Empty;
            Servicos = new List<Servico>();
        }
        public string TipoCPFCNPJ(){
            return this.CPF_CNPJ.ToString().Length > 11 ? "J" : "F";
        }

    }
}