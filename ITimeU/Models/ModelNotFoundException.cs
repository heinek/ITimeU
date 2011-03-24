using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ITimeU.Models
{
    public class ModelNotFoundException : Exception
    {
        public ModelNotFoundException(string message)
            : base(message)
        {
        }
    }
}