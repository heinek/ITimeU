using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
            StringBuilder listboxlist = new StringBuilder("");
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
            StringBuilder listboxlist = new StringBuilder("");
            foreach (var kvp in dictionary)
            {
                listboxlist.Append(string.Format("<option value=\"{0}\">{1}</option>", kvp.Key.ToString(), (kvp.Value.HasValue ? kvp.Value.Value.ToShortTimeString() : "")));
            }
            return listboxlist.ToString();
        }

        public static string ToListboxvalues(this Dictionary<int, string> dictionary)
        {
            StringBuilder listboxlist = new StringBuilder("");
            foreach (var kvp in dictionary)
            {
                listboxlist.Append(string.Format("<option value=\"{0}\">{1}</option>", kvp.Key.ToString(), kvp.Value));
            }
            return listboxlist.ToString();
        }

        public static string ToListboxvalues(this List<CheckpointOrder> lstCheckpointOrder)
        {
            StringBuilder listboxlist = new StringBuilder("");
            foreach (var checkpointOrder in lstCheckpointOrder)
            {
                listboxlist.Append(string.Format("<option value=\"{0}\">{1}</option>", checkpointOrder.ID, checkpointOrder.StartingNumber));
            }
            return listboxlist.ToString();
        }

        public static string ToListboxvalues(this List<RaceIntermediate> lstRaceintermediates)
        {
            StringBuilder listboxlist = new StringBuilder("");
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
            StringBuilder listboxlist = new StringBuilder("");
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

        public static string ToListboxvalues(this List<AthleteModel> lstAthletes)
        {
            StringBuilder listboxlist = new StringBuilder("");
            foreach (var athlete in lstAthletes)
            {
                using (var context = new Entities())
                {
                    listboxlist.Append(string.Format("<option value=\"{0}\">{1} {2}</option>", athlete.Id, athlete.FirstName, athlete.LastName));
                }
            }
            return listboxlist.ToString();
        }

        public static string ToListboxvalues(this AthleteModel Athlete)
        {
            StringBuilder listboxlist = new StringBuilder("");
            string seperator = " //// ";
            Type type = Athlete.GetType();
            foreach (PropertyInfo property in type.GetProperties())
            {
                listboxlist.Append(string.Format("{0}{1}", property.GetValue(Athlete, null), seperator));
            }

            return listboxlist.ToString();
        }

        public static string ToTable(this List<RaceIntermediateModel> lstRaceintermediates)
        {
            var sortedList = lstRaceintermediates.OrderBy(raceintermediate => raceintermediate.RuntimeModel.Runtime);
            StringBuilder table = new StringBuilder("");
            table.Append("<table><th>Plassering</th><th>Startnummer</th><th>Navn</th><th>Klubb</th><th>Tid</th>");
            int rank = 1;
            foreach (var raceintermediate in sortedList)
            {
                using (var context = new Entities())
                {
                    table.Append(string.Format("<tr><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td></tr>",
                        rank,
                        context.CheckpointOrders.
                        Where(checkpointOrder => checkpointOrder.ID == raceintermediate.CheckpointOrderID).
                        Single().StartingNumber,
                        raceintermediate.AthleteId.HasValue ?
                        raceintermediate.AthleteModel.FullName :
                        " - ",
                        raceintermediate.AthleteModel.Club.Name,
                        context.Runtimes.
                        Where(runtime => runtime.RuntimeID == raceintermediate.RuntimeId).
                        Single().Runtime1.ToTimerString()));
                }
                rank++;
            }
            table.Append("</table>");
            return table.ToString();
        }

        public static string ToTable(this List<ResultsViewModel> lstResults)
        {
            var sortedList = lstResults.OrderBy(result => result.Time);
            StringBuilder table = new StringBuilder("");
            table.Append("<table style='width: 800'><tr><th align='left' style='width: 100'>Passeringspunkt</th><th align='left' style='width: 80'>Plassering</th><th align='left' style='width: 100'>Startnummer</th><th align='left'>Navn</th><th align='left'>Klubb</th><th align='left'>Tid</th></tr>");
            int rank = 1;
            foreach (var result in sortedList)
            {
                using (var context = new Entities())
                {
                    table.Append(string.Format("<tr><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td><td>{5}</td></tr>",
                        result.Checkpointname,
                        rank,
                        result.Startnumber,
                        result.Fullname,
                        result.Clubname,
                        result.Time));
                }
                rank++;
            }
            table.Append("</table>");
            return table.ToString();
        }

        public static string ToTimerString(this int milliseconds)
        {
            TimeSpan ts = new TimeSpan(0, 0, 0, 0, milliseconds);
            return String.Format("{0}:{1}:{2}.{3}", ts.Hours.ToString("0"), ts.Minutes.ToString("00"), ts.Seconds.ToString("00"), ts.Milliseconds.ToString().Substring(0, 1));
        }
    }
}