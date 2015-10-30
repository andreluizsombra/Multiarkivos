using System;
using System.Collections;
using System.Collections.Generic;

namespace MK.Easydoc.Core.Infrastructure
{
    public class Error
    {
        #region Private Fields

        #endregion

        #region Public Properties

        public string PropertyName { get; set; }

        public string ErrorMessage { get; set; }

        #endregion

        #region Public Constructors

        public Error()
            : this(string.Empty, string.Empty) { }

        public Error(string errorMessage)
            : this(string.Empty, errorMessage) { }

        public Error(string propertyName, string errorMessage)
        {
            this.PropertyName = propertyName;
            this.ErrorMessage = errorMessage;
        }

        #endregion

        #region Public Methods

        public bool Equals(Error obj)
        {
            if (obj == null) return false;

            return (obj.PropertyName == this.PropertyName &&
                    obj.ErrorMessage == this.ErrorMessage);
        }

        #endregion

        #region Public Override Methods

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            int hash = 17;

            unchecked
            {
                hash = ((hash ^ 8) * (String.IsNullOrEmpty(this.PropertyName) ? this.PropertyName.GetHashCode() : -1));
                hash = ((hash ^ 8) * (String.IsNullOrEmpty(this.ErrorMessage) ? this.ErrorMessage.GetHashCode() : -1));
            }

            return hash;
        }

        public override string ToString()
        {
            return this.ErrorMessage;
        }

        #endregion
    }

    public class ErrorCollection : ICollection<Error>
    {
        #region Private Read-Only Fields

        private readonly List<Error> _errors = new List<Error>();

        #endregion

        #region Public Methods

        public void Add(string errorMessage)
        {
            this.Add(string.Empty, errorMessage);
        }

        public void Add(string propertyName, string errorMessage)
        {
            this.Add(new Error(propertyName, errorMessage));
        }

        public string[] GetErrors()
        {
            List<string> errors = new List<string>();

            this._errors.ForEach(delegate(Error error) { errors.Add(error.ErrorMessage); });

            return errors.ToArray();
        }

        #endregion

        #region ICollection<Error> Members

        public void Add(Error error)
        {
            if (!this._errors.Contains(error))
                this._errors.Add(error);
        }

        public void Clear()
        {
            this._errors.Clear();
        }

        public bool Contains(Error error)
        {
            return this._errors.Contains(error);
        }

        public void CopyTo(Error[] array, int arrayIndex)
        {
            this._errors.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return this._errors.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(Error error)
        {
            return this._errors.Remove(error);
        }

        public Error this[int idx]
        {
            get { return this._errors[idx]; }
            set { this._errors[idx] = value; }
        }

        #endregion

        #region IEnumerable<Error> Members

        public IEnumerator<Error> GetEnumerator()
        {
            return this._errors.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)this._errors).GetEnumerator();
        }

        #endregion
    }
}
