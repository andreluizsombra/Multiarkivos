using System;
using System.Web;
using System.Text;
using Newtonsoft.Json;
using MK.Easydoc.Core.Entities;

namespace MK.Easydoc.Core.Helpers
{
    public static class UserDataCookieHelper
    {
        #region Public Constants

        public const string USER_DATA_COOKIE_NAME = "easydoc.USER_DATA";

        #endregion

        #region Private Methods

        private static string EncryptData(object data)
        {
            var hash = default(string);
            var buffer = default(byte[]);
            var jsonData = default(string);

            try
            {
                jsonData = JsonConvert.SerializeObject(data);

                if (jsonData == null) { return null; }

                buffer = Encoding.ASCII.GetBytes(jsonData);
                hash = Convert.ToBase64String(buffer);
            }
            catch (Exception) { throw; }

            return hash;
        }

        private static TKey DecryptData<TKey>(string hash)
        {
            var data = default(string);
            var dataObject = default(TKey);

            try
            {
                data = Encoding.ASCII.GetString(Convert.FromBase64String(hash));
                if (data == null) { return default(TKey); }

                dataObject = JsonConvert.DeserializeObject<TKey>(data);
            }
            catch (Exception) { throw; }

            return dataObject;
        }

        #endregion

        #region Public Methods

        public static void CreateUserDataCookie(UserDataEntity entity)
        {
            var ticket = default(string);

            try
            {
                if (entity == null) { return; }

                ticket = EncryptData(entity);

                if (ticket == null) { return; }

                var cookie = new HttpCookie(USER_DATA_COOKIE_NAME, ticket);
                cookie.Expires = DateTime.Now.AddDays(1);

                HttpContext.Current.Response.Cookies.Add(cookie);
            }
            catch (Exception) { throw; }
        }

        public static UserDataEntity GetUserDataCookie()
        {
            var cookie = default(HttpCookie);
            var entity = default(UserDataEntity);

            try
            {
                cookie = HttpContext.Current.Request.Cookies[USER_DATA_COOKIE_NAME];
                if (cookie == null) { return new UserDataEntity(); }

                entity = DecryptData<UserDataEntity>(cookie.Value);
            }
            catch (Exception) { throw; }

            return entity;
        }

        public static string RenderStylesheetCSSClienteAtual()
        {
            var url = default(string);

            try
            {
                url = GetUserDataCookie().UrlCSSClienteAtual;
            }
            catch (Exception) { url = null; }

            if (url != null)
                HttpContext.Current.Response.Write(string.Format("<link href='{0}' rel='stylesheet' />", url));
               //return string.Format("<link href='{0}' rel='stylesheet' />", url);


            return string.Empty;


                //HttpContext.Current.Response.Write(string.Format("<link href='{0}' rel='stylesheet' />", url));
        }

        #endregion
    }
}
