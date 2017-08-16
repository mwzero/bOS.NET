using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace bOS.Commons.FormHelper
{
    public class ViewEngineException : Exception 
    {
        public ViewEngineException() : base()
        {
            // Add implementation.
        }

        public ViewEngineException(string message) : base (message)
        {
            // Add implementation.
        }
    }

}
