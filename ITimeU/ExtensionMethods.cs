using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ITimeU
{
    public static class ExtensionMethods
    {
        public static string ToListboxvalues(this Dictionary<int, int> dictionary, bool sortDesc = false)
        {
            StringBuilder listboxlist = new StringBuilder();
            var tmpDic = new Dictionary<int, int>();
            if (sortDesc) tmpDic = dictionary.OrderByDescending(dic => dic.Value).ToDictionary(dic => dic.Key, dic => dic.Value);
            else tmpDic = dictionary;
            foreach (var kvp in tmpDic)
            {
                listboxlist.Append(string.Format("<option value=\"{0}\">{1}</option>", kvp.Key.ToString(), kvp.Value.ToTimerString()));
            }
            return listboxlist.ToString();
        }

        public static string ToTimerString(this int milliseconds)
        {
            TimeSpan ts = new TimeSpan(0, 0, 0, 0, milliseconds);
            return String.Format("{0}:{1}:{2}.{3}", ts.Hours.ToString("0"), ts.Minutes.ToString("00"), ts.Seconds.ToString("00"), ts.Milliseconds.ToString().Substring(0, 1));
        }
    }
}