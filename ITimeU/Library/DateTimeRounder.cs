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
            int millisecondsRounded = roundMilliseconds(input);
            return createResult(input, millisecondsRounded);
        }

        private static int roundMilliseconds(DateTime input)
        {
            int exact = input.Millisecond; // Example: 250
            double dividedByHundred = exact / 100.0; //  2,5
            // Must use MidpointRounding.AwayFromZero in order to round 2,5 to 3 instead of 2!
            double rounded = Math.Round(dividedByHundred, MidpointRounding.AwayFromZero); // 3
            int multipliedByHundred = (int)(rounded * 100); // 300

            return multipliedByHundred;
        }

        private static DateTime createResult(DateTime input, int multipliedByHundred)
        {
            bool increaseSecond = false;
            if (multipliedByHundred >= 1000)
            {
                // Example: 5 seconds and 950 millseconds will be rounded up to 6 seconds and 0 milliseconds.
                multipliedByHundred = 0;
                increaseSecond = true;
            }

            DateTime result = new DateTime(
                input.Year, input.Month, input.Day, input.Hour, input.Minute, input.Second, multipliedByHundred);

            if (increaseSecond)
                result = result.AddSeconds(1);

            return result;
        }
        
    }
}