using System;
using System.Globalization;

namespace Utilities.Convert
{
    public static class DateConvertExtensions
    {
        public static string ToPersianDateTimeString(this DateTime dateTime)
        {
            PersianCalendar persianCalendar = new PersianCalendar();
            int year = persianCalendar.GetYear(dateTime);
            int month = persianCalendar.GetMonth(dateTime);
            int day = persianCalendar.GetDayOfMonth(dateTime);
            int hour = dateTime.Hour;
            int minute = dateTime.Minute;

            return $"{year}/{month:D2}/{day:D2} {hour:D2}:{minute:D2}";
        }

        public static string ToPersianDateString(this DateTime dateTime)
        {
            PersianCalendar pc = new PersianCalendar();
            int year = pc.GetYear(dateTime);
            int month = pc.GetMonth(dateTime);
            int day = pc.GetDayOfMonth(dateTime);

            return $"{year}/{month:D2}/{day:D2}";
        }

        public static DateTime ToGregorianDate(this string persianDate)
        {
            PersianCalendar persianCalendar = new PersianCalendar();
            int year = int.Parse(persianDate.Substring(0, 4));
            int month = int.Parse(persianDate.Substring(5, 2));
            int day = int.Parse(persianDate.Substring(8, 2));

            return persianCalendar.ToDateTime(year, month, day, 0, 0, 0, 0);
        }
    }
}
