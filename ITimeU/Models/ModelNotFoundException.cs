using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ITimeU.Models
{
    public class ModelNotFoundException : ApplicationException
    {
        public readonly string Message;

        public ModelNotFoundException(string message)
        {
            Message = message;
        }
    }
}