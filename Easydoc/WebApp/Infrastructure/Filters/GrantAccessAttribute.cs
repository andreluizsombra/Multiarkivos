using System;

namespace MK.Easydoc.WebApp.Infrastructure.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class GrantAccessAttribute : Attribute
    {
        #region Public Properties

        public string Roles { get; private set; }

        #endregion

        #region Constructors

        public GrantAccessAttribute(string roles)
        {
            this.Roles = roles;
        }

        #endregion
    }
}