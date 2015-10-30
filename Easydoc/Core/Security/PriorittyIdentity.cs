using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace MK.Easydoc.Core.Security
{
    public sealed class easydocIdentity : IIdentity
    {
		#region Public Static Read-Only Fields

		public static readonly easydocIdentity Anonymous = new easydocIdentity(".easydoc.ANONYMOUS."
			, false
			, "Anonymous"
            , 0
			, "Anonymous"
			, "anonymous@anonymous.com"
			, string.Empty);

		#endregion

		#region Private Read-Only Fields

		private readonly string _authenticationType;
		private readonly bool _isAuthenticated;
		private readonly string _name;
        private readonly int _id;
		private readonly string _fullName;
		private readonly string _email;
		private readonly string _role;

		#endregion

		#region Public Properties

		public int Id
		{
			get { return this._id; }
		}

		public string FullName
		{
			get { return this._fullName; }
		}

		public string Email
		{
			get { return this._email; }
		}

		public string Role
		{
			get { return this._role; }
		}

		#endregion

		#region Public Constructors

        public easydocIdentity(string authenticationType, bool isAuthenticated, string name, int id, string fullName, string email, string role)
		{
			this._authenticationType = authenticationType;
			this._isAuthenticated = isAuthenticated;
			this._name = name;
			this._id = id;
			this._fullName = fullName;
			this._email = email;
			this._role = role;
		}

		#endregion

		#region IIdentity Members

		public string AuthenticationType
		{
			get { return this._authenticationType; }
		}

		public bool IsAuthenticated
		{
			get { return this._isAuthenticated; }
		}

		public string Name
		{
			get { return this._name; }
		}

		#endregion
    }
}
