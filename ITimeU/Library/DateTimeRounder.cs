using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ITimeU.Library
{
    public class DateTimeRounder
    {

        public static DateTime RoundToOneDecimal(DateTime input)
        {
            int org = input.Millisecond; // Example: 250
            double div = org / 100.0; //  2,5
            // Must use MidpointRounding.AwayFromZero in order to round 2,5 to 3 instead of 2!
            double round = Math.Round(div, MidpointRounding.AwayFromZero); // 3
            int mult = (int)(round * 100); // 300

            bool increaseSecond = false;
            if (mult >= 1000)
            {
                mult = 0;
                increaseSecond = true;
            }

            DateTime result = new DateTime(
                input.Year, input.Month, input.Day, input.Hour, input.Minute, input.Second, mult);

            if (increaseSecond)
                result = result.AddSeconds(1);

            return result;
        }
        
    }
}