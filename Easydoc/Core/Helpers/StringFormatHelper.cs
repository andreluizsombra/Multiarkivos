using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Globalization;

namespace MK.Easydoc.Core.Helpers
{
    public class StringFormatHelper
    {
        public static string CnpjFormat(string cnpj)
        {
            var mtpCnpj = new MaskedTextProvider(@"00\.000\.000/0000-00");
            mtpCnpj.Set(LeadingZeros(cnpj, 11));
            return mtpCnpj.ToString();
        }

        public static string LeadingZeros(string str, int length)
        {
            var result = string.Empty;

            for (var intCont = 1; intCont <= (length - str.Length); intCont++)
            {
                result += "0";
            }
            return result + str;
        }

        public static string ClearFormatting(string cnpj)
        {
            return cnpj.Length == 14 ? cnpj : cnpj.Replace("/", "").Replace("-", "").Replace(".", "");
        }

        public static string ClearDateTime(DateTime date)
        {
            return date.ToString("yyyy-MM-dd");
        }
        public static string RemoveSpecialCharacters(string input)
        {
            Regex r = new Regex("(?:[^a-z0-9 ]|(?<=['\"])s)", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);

            string texto = r.Replace(input, "_");
            texto = texto.Normalize(NormalizationForm.FormD);
            StringBuilder sb = new StringBuilder();
            foreach (char c in texto.ToCharArray())
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                    sb.Append(c);

            input = sb.ToString();
            texto = r.Replace(input, "_");

            return texto;
        }

    }
}
