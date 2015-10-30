using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Principal;

namespace MK.Easydoc.Core.Security
{
    public sealed class easydocPrincipal : IPrincipal
    {
		#region Public Static Read-Only Fields

		public static readonly easydocPrincipal Anonymous = new easydocPrincipal(easydocIdentity.Anonymous);

		#endregion

		#region Private Read-Only Fields

		private readonly easydocIdentity _identity;

		#endregion

		#region Public Properties

		public easydocIdentity Identity
		{
			get { return this._identity; }
		}

		#endregion

		#region Public Constructors

		public easydocPrincipal(easydocIdentity identity)
		{			
			this._identity = identity;
		}

		#endregion

		#region Public Methods

		public bool IsInRole(string role)
		{
			return this.Identity.Role.Contains(role);
		}

		#endregion

		#region IPrincipal Members

		IIdentity IPrincipal.Identity
		{
			get { return this.Identity; }
		}

		bool IPrincipal.IsInRole(string role)
		{
            return this.IsInRole(role);            
		}

		#endregion
    }
}
