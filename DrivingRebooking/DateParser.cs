using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrivingRebooking
{
    public static class DateParser
    {
        public static DateTime FromUkDate(string date)
        {
            //"Wednesday 2 November 2016 1:25pm"
            try
            {
                return DateTime.ParseExact(date, "dddd d MMMM yyyy h:mmtt", new CultureInfo("en-GB"));
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException("date", string.Format("Cannot parse '{0}' date", date), ex);
            }
        }

        public static string ToUkString(DateTime date)
        {
            return date.ToString("dddd d MMMM yyyy h:mmtt");
        }
    }
}
