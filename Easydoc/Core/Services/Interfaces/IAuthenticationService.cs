using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MK.Easydoc.Core.Services.Interfaces
{
    public interface IAuthenticationService
    {
        bool AuthenticateUser(HttpContextBase context, string username, string password, bool remember);
        void PerformApplicationAuthenticateRequest(HttpApplication application);
    }
}
