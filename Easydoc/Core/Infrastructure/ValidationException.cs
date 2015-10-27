using System;
using System.Runtime.Serialization;
namespace MK.Easydoc.Core.Infrastructure
{
    [Serializable]
    public class ValidationException : Exception
    {

        #region Private Read-Only Fields

        private readonly ErrorCollection _errors = new ErrorCollection();

        #endregion

        #region Public Properties

        public ErrorCollection Errors
        {
            get { return this._errors; }
        }

        #endregion

        #region Public Constructors

        public ValidationException()
            : base("O objeto é inválido.") { }

        public ValidationException(ErrorCollection errors)
            : base("O objeto é inválido.")
        {
            foreach (Error error in errors)
                this.Errors.Add(error);
        }

        public ValidationException(string message)
            : base(message) { }

        public ValidationException(string message, ErrorCollection errors)
            : base(message)
        {
            foreach (Error error in errors)
                this.Errors.Add(error);
        }

        public ValidationException(string message, Exception inner)
            : base(message, inner) { }

        public ValidationException(string message, Exception inner, ErrorCollection errors)
            : base(message, inner) { }

        #endregion

        #region Protected Constructors

        protected ValidationException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }

        #endregion
    }
}
