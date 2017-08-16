using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace bOS.Commons
{
    public class DateHelper
    {
        public static String GetDateTimeToString(DateTime dt, String format)
        {
            return dt.ToString(format);
        }

        public static String GetCurrentDateTimeToString(String format)
        {
            return DateTime.Now.ToString(format);
        }

        public static String GetDateToString(DateTime dt, String format)
        {
            return dt.ToString(format);
        }

        public static DateTime? GetDateTime(String sDate, String format, DateTime? defaultValue)
        {
            if ( String.IsNullOrEmpty (sDate ) )
                return defaultValue;

            try
            {

                return DateTime.ParseExact(sDate, format, null);
            }
            catch
            {
                return defaultValue;
            }
        }

        public static String GetDateToString(DateTime? dt, String format, String defaultValue)
        {
            if (dt.HasValue)
            {
                return dt.GetValueOrDefault().ToString(format);
            }
            else
                return defaultValue;
        }

    }
}
