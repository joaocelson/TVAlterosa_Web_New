using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace ASSO.Domain.Helpers
{
    public static class DateTimeHelper
    {
        public static string ConvertDateShortMonthToIntMonth(string dateShortMonth)
        {

            if (String.IsNullOrEmpty(dateShortMonth) || String.IsNullOrEmpty(dateShortMonth.Trim()))
                return "";

            string[] results = Regex.Split(dateShortMonth, @"\-|\s");

            int n;

            if (!int.TryParse(results[1], out n))
            {
                var month = DateTime.ParseExact(results[1], "MMM", CultureInfo.InvariantCulture).Month;
                var dateNumberMonth = results[0] + "/" + month.ToString().PadLeft(2, '0') + "/" + results[2];
                if (results.Count() == 4)
                    dateNumberMonth += " " + results[3];

                return dateNumberMonth;
            }
            else
            {
                return dateShortMonth;
            }
        }

        public static string FormatMinutesToHour(int minutes)
        {
            var formatedStr = "";

            if (minutes < 60)
            {
                formatedStr = "00:" + minutes.ToString().PadLeft(2, '0'); 
            }
            else
            {
                var hour = (minutes / 60);
                int dummy = 0;

                if (!Int32.TryParse(hour.ToString(), out dummy))
                {
                    hour = hour - 1;
                }

                formatedStr = hour.ToString().PadLeft(2, '0') + ":" +
                                            (minutes % 60).ToString().PadLeft(2, '0');
            }

            return formatedStr;
        }
    }
}