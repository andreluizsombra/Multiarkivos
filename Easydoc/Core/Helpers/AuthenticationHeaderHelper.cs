using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MK.Easydoc.Core.Helpers
{
    /*
    public static class AuthenticationHeaderHelper
    {

        #region Private Static Methods

        private static string[] DecryptUserData(string userdata)
        {
            var hash = Encoding.ASCII
                .GetString(Convert.FromBase64String(userdata))
                .Split(':');

            if (hash.Length != 2 ||
                string.IsNullOrWhiteSpace(hash[0]) ||
                string.IsNullOrWhiteSpace(hash[1])) return null;

            return hash;
        }

        #endregion        

        #region Public Static Methods

        public static string EncryptUserData(string username, string password)
        {
            var buffer = default(byte[]);

            buffer = Encoding.ASCII.GetBytes(string.Format("{0}:{1}"
                 , username
                 , password));

            return Convert.ToBase64String(buffer);
        }

        public static string[] GetUserData()
        {
            var authCookie = default(HttpCookie);
            var authTicket = default(FormsAuthenticationTicket);

            try
            {
                authCookie = HttpContext.Current.Request.Cookies.Get(FormsAuthentication.FormsCookieName);

                if (authCookie == null) { return null; }

                authTicket = FormsAuthentication.Decrypt(authCookie.Value);

                if (authTicket == null) { return null; }

                return DecryptUserData(authTicket.UserData);
            }
            catch (Exception) { throw; }
        }

        public static AuthenticationHeaderValue GenerateAuthenticationHeader(string username, string password)
        {
            var buffer = default(byte[]);            
            var authHeader = default(AuthenticationHeaderValue);

            if (username == null || password == null) { return null; }
            
            buffer = Encoding.ASCII.GetBytes(string.Format("{0}:{1}", username, password));
            authHeader = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(buffer));

            return authHeader;
        }

        public static AuthenticationHeaderValue GetAuthenticationHeader()
        {
            var buffer = default(byte[]);
            var username = default(string);
            var password = default(string);
            var authHeader = default(AuthenticationHeaderValue);
            var userData = default(string[]);

            userData = GetUserData();
            if (userData == null) { return null; }

            username = userData[0];
            password = userData[1];

            buffer = Encoding.ASCII.GetBytes(string.Format("{0}:{1}", username, password));
            authHeader = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(buffer));

            return authHeader;
        }

        #endregion

    }
     */
}
