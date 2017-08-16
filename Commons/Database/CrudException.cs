using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace bOS.Commons.Database
{
    public class CrudException : Exception
    {
        private CrudResult codeError;
        private String messageError;

        public CrudException(CrudResult code, String message)
        {
            this.codeError = code;
            this.messageError = message;
        }

        public CrudException(CrudResult code, Exception err)
        {
            this.codeError = code;
            if ((err.InnerException != null) && (!String.IsNullOrEmpty(err.InnerException.Message)))
                this.messageError = err.InnerException.Message;
            else
                this.messageError = err.Message;
        }

        public CrudResult Code
        {
            get { return codeError; }
            set { codeError = value; }
        }
        public String message
        {
            get { return messageError; }
            set { messageError = value; }
        }
    }
}