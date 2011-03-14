using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ITimeU.Library
{
    public class NameParser
    {
        public static string FirstName(string s)
        {
            s = s.Trim();

            if (s.IndexOf(" ") == -1) // Example: "Per"
                return s;
            return s.Substring(0, s.LastIndexOf(" "));
        }

        public static string LastName(string s)
        {
            s = s.Trim();

            if (s.IndexOf(" ") == -1) // Example: "Per"
                return s;
            else return s.Substring(s.LastIndexOf(" ") + 1);
        }
    }
}