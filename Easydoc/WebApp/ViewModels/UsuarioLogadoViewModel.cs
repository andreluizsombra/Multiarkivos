using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Collections.Generic;
using MK.Easydoc.Core.Entities;

namespace MK.Easydoc.WebApp.ViewModels
{
    public class UsuarioLogadoViewModel
    {
        public int Usuario_ID { get; set; }
        public string NomeUsuario { get; set; }
        public Usuario Usuario { get; set; }
        public List<Servico> Servicos { get; set; }
        public List<Cliente> Clientes { get; set; }

        public UsuarioLogadoViewModel() { 
            Usuario_ID = 0;
            NomeUsuario = string.Empty;
            Usuario = new Usuario();
            List<Servico> Servicos = new List<Servico>();
            List<Cliente> Clientes = new List<Cliente>();
        
        }
    }
}