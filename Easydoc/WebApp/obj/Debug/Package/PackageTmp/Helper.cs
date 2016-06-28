using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

    namespace System.Web.Mvc.Html
    {
        public static class CustomHelper
        {
            public static MvcHtmlString Input(string name, string value)
            {
                return MvcHtmlString.Create(String.Format("<input  name='{0}' value='{1}' id='{0}' type=”text” class='form-control' />", name, value));
            }
            public static MvcHtmlString FormatToCpfOrCnpj(string value)
            {
                //Formatar string passada por parametro
                var resultado = "";
                if (value.Length == 11)
                    resultado = value.Substring(0, 3) + "." + value.Substring(3, 3) + "." + value.Substring(6, 3) + "-" + value.Substring(9, 2);
                //resultado = String.Format("{0:000.000.000-00}", long.Parse(value));
                else if (value.Length == 14)
                    resultado = value.Substring(0, 2) + "." + value.Substring(2, 3) + "." + value.Substring(5, 3) + "/" + value.Substring(8, 4) + "-" + value.Substring(12, 2);

                return MvcHtmlString.Create(String.Format("{0}", resultado));
            }

            public static string SimNao(string value)
            {
                string resultado="";
                resultado = value=="True"? "Sim" : "Não";
                return resultado;
            }
        }
    }
