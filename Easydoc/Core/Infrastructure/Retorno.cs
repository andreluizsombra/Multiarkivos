using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management;
using System.Net.NetworkInformation;
using System.Net;

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
            this.Mensagem = "";
            this.Dados = null;
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

        public static string GetIP()
        {
            string strHostName = "";
            strHostName = System.Net.Dns.GetHostName();

            IPHostEntry ipEntry = System.Net.Dns.GetHostEntry(strHostName);

            IPAddress[] addr = ipEntry.AddressList;

            return addr[addr.Length - 2].ToString();

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
            string ip_for = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            string local_ip = context.Request.ServerVariables["LOCAL_ADDR"];

            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            IPGlobalProperties properties = IPGlobalProperties.GetIPGlobalProperties();
            string ret = String.Format("IPv4 interface HostName: {0} Dominio: {1}",
               properties.HostName, properties.DomainName);

            string UserHost = System.Web.HttpContext.Current.Request.UserHostAddress.ToString();
            string hostname = System.Web.HttpContext.Current.Request.UserHostName.ToString();

            string strEnderecoIP;
            strEnderecoIP = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (strEnderecoIP == null)
                strEnderecoIP = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];


            string HostName2 = System.Net.Dns.GetHostName();

            string nome = System.Net.Dns.GetHostName();
            System.Net.IPAddress[] ipnet = System.Net.Dns.GetHostAddresses(nome);
            string ip4 = ipnet[4].ToString();

            for (int i = 0; i < ipnet.Length; i++)
            {
                ip += "[ Ip_TESTE_" + i.ToString() + "_" + ipnet[i].ToString()+" ] \n ";
            }

            ip += ret + " GetIP: " + GetIP() + " UserHost: " + UserHost + " HostName: " + hostname + "\n Hostname2: " + HostName2 + "\n IP_3:" + strEnderecoIP + " IP_FOR: " + ip_for + " IP4: " + ip4 + " LOCAL_ADDR: " + local_ip;
            return ip == "::1" ? "127.0.0.1" : ip;
        }
        
    }


}
