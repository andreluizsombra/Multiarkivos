using System;
using System.Configuration;
using System.Collections.Specialized;
namespace MK.Easydoc.Core.Infrastructure
{
    public class ConfigManager
    {
        #region Public Static Properties

        public static NameValueCollection AppSettings
        {
            get { return ConfigManager.AppSettings; }
        }

        #endregion

        #region Public Static Methods

        public static T GetSection<T>(string nomeSecao)
        {
            var section = ConfigurationManager.GetSection(nomeSecao);

            if (section == null)
                throw new InvalidOperationException(string.Format("Não existe no web.config a seção com o nome \"{0}\".", nomeSecao));

            if (section.GetType() != typeof(T))
                throw new InvalidOperationException(string.Format("Não foi possível retornar a seção do tipo \"{0}\"", typeof(T).FullName));

            return (T)section;
        }

        #endregion
    }
}
