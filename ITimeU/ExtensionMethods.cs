using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ITimeU.Models;

namespace ITimeU
{
    public static class ExtensionMethods
    {
        public enum ListboxSorting
        {
            None,
            Ascending,
            Descending
        }
        public static string ToListboxvalues(this Dictionary<int, int> dictionary, ListboxSorting sorting = ListboxSorting.None, bool toTimer = false)
        {
            StringBuilder listboxlist = new StringBuilder();
            var tmpDic = new Dictionary<int, int>();
            if (sorting == ListboxSorting.Descending) tmpDic = dictionary.OrderByDescending(dic => dic.Value).ToDictionary(dic => dic.Key, dic => dic.Value);
            else if (sorting == ListboxSorting.Ascending) tmpDic = dictionary.OrderBy(dic => dic.Value).ToDictionary(dic => dic.Key, dic => dic.Value);
            else tmpDic = dictionary;
            foreach (var kvp in tmpDic)
            {
                listboxlist.Append(string.Format("<option value=\"{0}\">{1}</option>", kvp.Key.ToString(), toTimer ? kvp.Value.ToTimerString() : kvp.Value.ToString()));
            }
            return listboxlist.ToString();
        }

        public static string ToListboxvalues(this Dictionary<int, DateTime?> dictionary)
        {
            StringBuilder listboxlist = new StringBuilder();
            foreach (var kvp in dictionary)
            {
                listboxlist.Append(string.Format("<option value=\"{0}\">{1}</option>", kvp.Key.ToString(), (kvp.Value.HasValue ? kvp.Value.Value.ToShortTimeString() : "")));
            }
            return listboxlist.ToString();
        }

        public static string ToListboxvalues(this Dictionary<int, string> dictionary)
        {
            StringBuilder listboxlist = new StringBuilder();
            foreach (var kvp in dictionary)
            {
                listboxlist.Append(string.Format("<option value=\"{0}\">{1}</option>", kvp.Key.ToString(), kvp.Value));
            }
            return listboxlist.ToString();
        }

        public static string ToListboxvalues(this List<CheckpointOrder> lstCheckpointOrder)
        {
            StringBuilder listboxlist = new StringBuilder();
            foreach (var checkpointOrder in lstCheckpointOrder)
            {
                listboxlist.Append(string.Format("<option value=\"{0}\">{1}</option>", checkpointOrder.ID, checkpointOrder.StartingNumber));
            }
            return listboxlist.ToString();
        }

        public static string ToListboxvalues(this List<RaceIntermediate> lstRaceintermediates)
        {
            StringBuilder listboxlist = new StringBuilder();
            foreach (var raceintermediate in lstRaceintermediates)
            {
                using (var context = new Entities())
                {
                    listboxlist.Append(string.Format("<option value=\"{0}\">{1}: {2}</option>", raceintermediate.CheckpointOrderID, context.CheckpointOrders.
                        Where(checkpointOrder => checkpointOrder.ID == raceintermediate.CheckpointOrderID).
                        Single().StartingNumber, context.Runtimes.
                        Where(runtime => runtime.RuntimeID == raceintermediate.RuntimeId).
                        Single().Runtime1.ToTimerString()));
                }
            }
            return listboxlist.ToString();
        }

        public static string ToListboxvalues(this List<RaceIntermediateModel> lstRaceintermediates)
        {
            StringBuilder listboxlist = new StringBuilder();
            foreach (var raceintermediate in lstRaceintermediates)
            {
                using (var context = new Entities())
                {
                    listboxlist.Append(string.Format("<option value=\"{0}\">{1}: {2}</option>", raceintermediate.CheckpointOrderID, context.CheckpointOrders.
                        Where(checkpointOrder => checkpointOrder.ID == raceintermediate.CheckpointOrderID).
                        Single().StartingNumber, context.Runtimes.
                        Where(runtime => runtime.RuntimeID == raceintermediate.RuntimeId).
                        Single().Runtime1.ToTimerString()));
                }
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