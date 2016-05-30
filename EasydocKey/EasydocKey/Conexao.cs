using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Security.Cryptography;
using System.IO;
namespace EasydocKey
{
    class Conexao
    {

        public string DataSource { get; set; }
        public string InitialCatalog { get; set; }
        public bool IntegratedSecurity { get; set; }
        public string UserID { get; set; }
        
        public string Password { get; set; }
        private string Key { get; set; }
        private const string chave = "12a1496321458799";
        private const string vetorInicializacao = "1472583691234567";
        public Conexao()
        {
            
        }
        public Conexao(string valor)
        {
            Key = valor;
        }

        public string Encript()
        {
            string _enc = Key.Replace(";", "PPP").Replace("=", "GGG").Replace(".", "XXX").Replace("@", "WWW").Replace("*", "HHH");
            return Encriptar(chave, vetorInicializacao, _enc);
        }
        public string Decript()
        {
            string _dec = Decriptar(chave, vetorInicializacao, Key);
            return _dec.Replace("PPP", ";").Replace("GGG", "=").Replace("XXX", ".").Replace("WWW", "@").Replace("HHH", "*");
        }

        //public static void VerificarConexaoString(string name)
        //{
        //    bool isNew = false;
        //    string path = System.Web.HttpContext.Current.Server.MapPath("~/Web.Config");
        //    XmlDocument doc = new XmlDocument();
        //    doc.Load(path);
        //    XmlNodeList list = doc.DocumentElement.SelectNodes(string.Format("connectionStrings/add[@name='{0}']", name));
        //    XmlNode node;
        //    isNew = list.Count == 0;
        //    if (isNew)
        //    {
        //        node = doc.CreateNode(XmlNodeType.Element, "add", null);
        //        XmlAttribute attribute = doc.CreateAttribute("name");
        //        attribute.Value = name;
        //        node.Attributes.Append(attribute);

        //        attribute = doc.CreateAttribute("connectionString");
        //        attribute.Value = "";
        //        node.Attributes.Append(attribute);

        //        attribute = doc.CreateAttribute("providerName");
        //        attribute.Value = "System.Data.SqlClient";
        //        node.Attributes.Append(attribute);
        //    }
        //    else
        //    {
        //        node = list[0];
        //    }
        //    string conString = node.Attributes["connectionString"].Value;

        //    //string chave = "12a1496321458799";
        //    //string vetorInicializacao = "1472583691234567";
        //    string conexaolimpa = "";
        //    if (conString.Substring(0, 4) == "Data")
        //    {
        //        conexaolimpa = conString.Replace(";", "PPP").Replace("=", "GGG").Replace(".", "XXX").Replace("@", "WWW").Replace("*", "HHH");
        //        conexaolimpa = Encriptar(chave, vetorInicializacao, conexaolimpa);
        //        node.Attributes["connectionString"].Value = conexaolimpa;
        //        doc.Save(path);
        //    }

        //    //string[] _campos = conString.Split(';');

        //    //var cnx = new Conexao();

        //    //foreach (string part in _campos)
        //    //{
        //    //    string[] _prop = part.Split('=');
        //    //    string prm1 = _prop[0].ToString();
        //    //    string prm2 = _prop[1].ToString();

        //    //    foreach (string param in _prop)
        //    //    {
        //    //        string p1 = _prop[0].ToString();
        //    //        string p2 = _prop[1].ToString();
        //    //        if (p1 == "Data Source") cnx.DataSource = p2;
        //    //        if (p1 == "Initial Catalog") cnx.InitialCatalog = p2;
        //    //        if (p1 == "Integrated Security") cnx.IntegratedSecurity = bool.Parse(p2);
        //    //        if (p1 == "User ID") cnx.UserID = p2;
        //    //        if (p1 == "Password") cnx.Password = p2;
        //    //    }
        //    //}



        //    //SqlConnectionStringBuilder conStringBuilder = new SqlConnectionStringBuilder(conString);
        //    //conStringBuilder.DataSource = Encriptar(chave, vetorInicializacao, cnx.DataSource);
        //    //conStringBuilder.InitialCatalog = Encriptar(chave, vetorInicializacao, cnx.InitialCatalog);
        //    //conStringBuilder.IntegratedSecurity = cnx.IntegratedSecurity;
        //    //conStringBuilder.UserID = Encriptar(chave, vetorInicializacao, cnx.UserID);
        //    //conStringBuilder.Password = Encriptar(chave, vetorInicializacao, cnx.Password);

        //    //conStringBuilder.InitialCatalog = "TestDB";
        //    //conStringBuilder.DataSource = "myserver";
        //    //conStringBuilder.IntegratedSecurity = false;
        //    //conStringBuilder.UserID = "test";
        //    //conStringBuilder.Password = "12345";


        //    //node.Attributes["connectionString"].Value = conexaolimpa;  //conStringBuilder.ConnectionString;
        //    //node.Attributes["connectionString"].Value = conStringBuilder.ConnectionString;
        //    if (isNew)
        //    {
        //        doc.DocumentElement.SelectNodes("connectionStrings")[0].AppendChild(node);
        //    }
        //    //doc.Save(path);
        //}

        private static Rijndael CriarInstanciaRijndael(
            string chave, string vetorInicializacao)
        {
            if (!(chave != null &&
                  (chave.Length == 16 ||
                   chave.Length == 24 ||
                   chave.Length == 32)))
            {
                throw new Exception(
                    "A chave de criptografia deve possuir " +
                    "16, 24 ou 32 caracteres.");
            }

            if (vetorInicializacao == null ||
                vetorInicializacao.Length != 16)
            {
                throw new Exception(
                    "O vetor de inicialização deve possuir " +
                    "16 caracteres.");
            }

            Rijndael algoritmo = Rijndael.Create();
            algoritmo.Key =
                Encoding.ASCII.GetBytes(chave);
            algoritmo.IV =
                Encoding.ASCII.GetBytes(vetorInicializacao);

            return algoritmo;
        }

        public static string Encriptar(
            string chave,
            string vetorInicializacao,
            string textoNormal)
        {
            if (String.IsNullOrWhiteSpace(textoNormal))
            {
                throw new Exception(
                    "O conteúdo a ser encriptado não pode " +
                    "ser uma string vazia.");
            }

            using (Rijndael algoritmo = CriarInstanciaRijndael(
                chave, vetorInicializacao))
            {
                ICryptoTransform encryptor =
                    algoritmo.CreateEncryptor(
                        algoritmo.Key, algoritmo.IV);

                using (MemoryStream streamResultado =
                       new MemoryStream())
                {
                    using (CryptoStream csStream = new CryptoStream(
                        streamResultado, encryptor,
                        CryptoStreamMode.Write))
                    {
                        using (StreamWriter writer =
                            new StreamWriter(csStream))
                        {
                            writer.Write(textoNormal);
                        }
                    }

                    return ArrayBytesToHexString(
                        streamResultado.ToArray());
                }
            }
        }

        private static string ArrayBytesToHexString(byte[] conteudo)
        {
            string[] arrayHex = Array.ConvertAll(
                conteudo, b => b.ToString("X2"));
            return string.Concat(arrayHex);
        }

        public static bool CriptografiaValida(string textoEncriptado)
        {
            return (textoEncriptado.Length % 2 != 0);
        }
        public static string Decriptar(
            string chave,
            string vetorInicializacao,
            string textoEncriptado)
        {
            if (String.IsNullOrWhiteSpace(textoEncriptado))
            {
                throw new Exception(
                    "O conteúdo a ser decriptado não pode " +
                    "ser uma string vazia.");
            }

            if (textoEncriptado.Length % 2 != 0)
            {
                throw new Exception(
                    "O conteúdo a ser decriptado é inválido.");
            }


            using (Rijndael algoritmo = CriarInstanciaRijndael(
                chave, vetorInicializacao))
            {
                ICryptoTransform decryptor =
                    algoritmo.CreateDecryptor(
                        algoritmo.Key, algoritmo.IV);

                string textoDecriptografado = null;
                using (MemoryStream streamTextoEncriptado =
                    new MemoryStream(
                        HexStringToArrayBytes(textoEncriptado)))
                {
                    using (CryptoStream csStream = new CryptoStream(
                        streamTextoEncriptado, decryptor,
                        CryptoStreamMode.Read))
                    {
                        using (StreamReader reader =
                            new StreamReader(csStream))
                        {
                            textoDecriptografado =
                                reader.ReadToEnd();
                        }
                    }
                }

                return textoDecriptografado;
            }
        }

        private static byte[] HexStringToArrayBytes(string conteudo)
        {
            int qtdeBytesEncriptados =
                conteudo.Length / 2;
            byte[] arrayConteudoEncriptado =
                new byte[qtdeBytesEncriptados];
            for (int i = 0; i < qtdeBytesEncriptados; i++)
            {
                arrayConteudoEncriptado[i] = Convert.ToByte(
                    conteudo.Substring(i * 2, 2), 16);
            }

            return arrayConteudoEncriptado;
        }

        //public static string DescriptografarConexaoString(string name)
        //{
        //    //bool isNew = false;
        //    string path = System.Web.HttpContext.Current.Server.MapPath("~/Web.Config");
        //    XmlDocument doc = new XmlDocument();
        //    doc.Load(path);
        //    XmlNodeList list = doc.DocumentElement.SelectNodes(string.Format("connectionStrings/add[@name='{0}']", name));
        //    XmlNode node;
        //    //isNew = list.Count == 0;
        //    //if (isNew)
        //    //{
        //    //    node = doc.CreateNode(XmlNodeType.Element, "add", null);
        //    //    XmlAttribute attribute = doc.CreateAttribute("name");
        //    //    attribute.Value = name;
        //    //    node.Attributes.Append(attribute);

        //    //    attribute = doc.CreateAttribute("connectionString");
        //    //    attribute.Value = "";
        //    //    node.Attributes.Append(attribute);

        //    //    attribute = doc.CreateAttribute("providerName");
        //    //    attribute.Value = "System.Data.SqlClient";
        //    //    node.Attributes.Append(attribute);
        //    //}
        //    //else
        //    //{
        //    node = list[0];
        //    //}
        //    string conString = node.Attributes["connectionString"].Value;

        //    string chave = "12a1496321458799";
        //    string vetorInicializacao = "1472583691234567";
        //    string conexaolimpa = "";
        //    string conexao_descript = "";
        //    if (conString.Substring(0, 4) != "Data")
        //    {
        //        conexao_descript = Decriptar(chave, vetorInicializacao, conString);
        //        conexaolimpa = conexao_descript.Replace("PPP", ";").Replace("GGG", "=").Replace("XXX", ".").Replace("WWW", "@").Replace("HHH", "*");
        //    }

        //    return conexaolimpa;
        //}
    }
}
