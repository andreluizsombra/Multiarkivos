using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MK.Easydoc.Core.Infrastructure
{
    public class Retorno
    {
        public long CodigoRetorno { get; set; }
        public string Mensagem { get; set; }
        public object Dados { get; set; }
        public Exception Erro { get; set; }
        public int Bloqueado { get; set; }
        public Retorno()
        {
            this.Erro = null;
            this.CodigoRetorno = 0;
            this.Mensagem ="";
            this.Dados =null;
            this.Bloqueado = -1;
        }

        public Retorno(long codigoRetorno, string mensagem, object dados, Exception erro)
        {
            this.Erro = erro;
            this.CodigoRetorno = codigoRetorno;
            this.Mensagem = mensagem;
            this.Dados = dados;
        }

    
    }


}
