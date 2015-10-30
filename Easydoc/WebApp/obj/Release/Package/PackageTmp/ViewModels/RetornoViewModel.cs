using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MK.Easydoc.WebApp.ViewModels
{

    #region Enumerators

    public enum RetornoType { Default = 0, ErrorExceptions = 1, ValidationExceptions = 2 }

    #endregion
    public class RetornoViewModel
    {
        #region Public Properties

        public bool success { get; set; }
        public string message { get; set; }
        public RetornoType type { get; set; }
        public object output { get; set; }

        #endregion

        #region Constructors

        public RetornoViewModel(bool _success)
            : this(_success, null, null, RetornoType.Default) { }

        public RetornoViewModel(bool _success, string _message)
            : this(_success, _message, null, RetornoType.Default) { }

        public RetornoViewModel(bool _success, string _message, object _output)
            : this(_success, _message, _output, RetornoType.Default) { }

        public RetornoViewModel(bool _success, string _message, object _output, RetornoType _type)
        {
            this.success = _success;
            this.message = _message;
            this.output = _output;
            this.type = _type;
        }

        #endregion
    }
}