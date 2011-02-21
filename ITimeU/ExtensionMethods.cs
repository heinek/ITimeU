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
                listboxlist.Append(string.Format("<option value=\"{0}\">{1}</option>", kvp.Key.ToString(), kvp.Value.ToString()));
            }
            return listboxlist.ToString();
        }
    }
}