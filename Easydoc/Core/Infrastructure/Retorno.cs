using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management;

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

        public static string GetMacAdress()
        {
            string id = "";
            ManagementObjectSearcher query = null;
            ManagementObjectCollection queryCollection = null;

            query = new ManagementObjectSearcher("SELECT * FROM Win32_NetworkAdapterConfiguration");
            queryCollection = query.Get();
            foreach (ManagementObject mo in queryCollection)
            {
                if (mo["MacAddress"] != null)
                {
                    id = mo["MacAddress"].ToString();
                    return id;
                }
            }
            return id;

        }

        public static string GetUserIPAddress()
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            string ipAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (!string.IsNullOrEmpty(ipAddress))
            {
                string[] addresses = ipAddress.Split(',');
                if (addresses.Length != 0)
                {
                    return addresses[0];
                }
            }
            string ip = context.Request.ServerVariables["REMOTE_ADDR"];
            return ip == "::1" ? "127.0.0.1" : ip;
        }
    }


}
