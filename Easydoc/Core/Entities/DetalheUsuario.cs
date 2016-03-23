using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MK.Easydoc.Core.Entities
{
    public class DetalheUsuario
    {
        public string CPF { get; set; }
        public string Login { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string TentativasErradas { get; set; }
        public string NomeCliente { get; set; }
        public string CPFCNPJCliente { get; set; }
        public string DataCriacaoCliente { get; set; }
        public string DataExclusaoCliente { get; set; }
        public string DataAlteracaoCliente { get; set; }
        public string StatusCliente { get; set; }
        public string QuantidadeMaximaUsuarioCliente { get; set; }
        public string EmailCorporativoCliente { get; set; }
        public string Servico { get; set; }
        public string DataCriacaoServico { get; set; }
        public string ArquivosDeDadosServico { get; set; }
        public string ControleAtencaoServico { get; set; }
        public string DashboardCapturaServico { get; set; }
        public string DashboardPendenciasServico { get; set; }
        public string DashboardDocumentosPorModuloServico { get; set; }
        public string ScriptRegraNegocioServico { get; set; }
        public string Perfil { get; set; }

        public DetalheUsuario() {
         CPF = string.Empty;
         Login = string.Empty;
         Nome = string.Empty;
         Email = string.Empty;
         TentativasErradas = string.Empty;
         NomeCliente = string.Empty;
         CPFCNPJCliente = string.Empty;
         DataCriacaoCliente = string.Empty;
         DataExclusaoCliente = string.Empty;
         DataAlteracaoCliente = string.Empty;
         StatusCliente = string.Empty;
         QuantidadeMaximaUsuarioCliente = string.Empty;
         EmailCorporativoCliente = string.Empty;
         Servico = string.Empty;
         DataCriacaoServico = string.Empty;
         ArquivosDeDadosServico = string.Empty;
         ControleAtencaoServico = string.Empty;
         DashboardCapturaServico = string.Empty;
         DashboardPendenciasServico = string.Empty;
         DashboardDocumentosPorModuloServico = string.Empty;
         ScriptRegraNegocioServico = string.Empty;
         Perfil = string.Empty;
        }
    }
}
