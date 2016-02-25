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
                var resultado = "";
                if (value.Length == 11)
                    resultado = String.Format("{0:000-000-000/00}", long.Parse(value));
                else if (value.Length == 14)
                    resultado = String.Format("{0:00.000.000/0000-00}", long.Parse(value));

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
