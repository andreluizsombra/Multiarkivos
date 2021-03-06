﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MK.Easydoc.Core.Entities
{
    public class Usuario
    {
        public int ID { get; set; }
        public string NomeUsuario { get; set; }
        public string Senha { get; set; }
        public string NomeCompleto { get; set; }
        public string NomeCliente { get; set; }
        public string NomeServico { get; set; }
        public string Email { get; set; }
        public string Perfil { get; set; }
        public int PerfilID { get; set; }
        public List<Cliente> Clientes { get; set; }
        public string CPF { get; set; }
        public int Bloqueado { get; set; }
        public int TipoAcao { get; set; }
        public int ClienteID { get; set; }
        public int ServicoID { get; set; }
        public int Situacao { get; set; }
        public int TrocarSenha { get; set; }
        
        public Usuario() { 
            ID = 0;
            NomeUsuario = string.Empty;
            Senha  = string.Empty;
            NomeCompleto  = string.Empty;
            Email  = string.Empty;
            Perfil  = string.Empty;
            Clientes = new List<Cliente>();
            Bloqueado = 0;
            TrocarSenha = 0;
        }
    }

    public class ClienteServico
    {
        public int ClienteID { get; set; }
        public int ServicoID { get; set; }
        public string NomeCliente { get; set; }
        public string NomeServico { get; set; }
        public int PerfilID { get; set; }
        public int? Bloqueado { get; set; }
        public int? Situacao { get; set; }
    }
}
