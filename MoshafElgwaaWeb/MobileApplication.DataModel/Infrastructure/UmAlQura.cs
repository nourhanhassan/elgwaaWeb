using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

public class UmAlQura
{
    private static readonly UmAlQuraCalendar umalqura = new UmAlQuraCalendar();
    private static readonly CultureInfo cInfoSA = new CultureInfo("ar-SA", false);
    private static readonly CultureInfo cInfoEG = new CultureInfo("ar-EG", false);
   
    
    public static int GetDiffDays(DateTime? d1, DateTime? d2)
    {
        DateTime startDate = d1.Value > d2.Value ? d2.Value : d1.Value;
        DateTime endDate = d1.Value > d2.Value ? d1.Value : d2.Value;
        TimeSpan diff = endDate.Subtract(startDate);
        return diff.Days;
    }
    public static int GetDiffMonths(DateTime? d1, DateTime? d2)
    {
        if (d1 == d2) return 0;

        UmAlQuraCalendar umalqura = new UmAlQuraCalendar();
        DateTime startDate = d1.Value > d2.Value ? d2.Value : d1.Value;
        DateTime endDate = d1.Value > d2.Value ? d1.Value : d2.Value;
        DateTime tmpDate = startDate;

        int monthCounter = 0;
        while (umalqura.AddMonths(tmpDate, 1).AddDays(-1) <= endDate)
        {
            monthCounter++;
            tmpDate = umalqura.AddMonths(tmpDate, 1);
        }
        return monthCounter;
    }
    public static int GetDiffYears(DateTime? d1, DateTime? d2)
    {
        UmAlQuraCalendar umalqura = new UmAlQuraCalendar();
        DateTime startDate = d1.Value > d2.Value ? d2.Value : d1.Value;
        DateTime endDate = d1.Value > d2.Value ? d1.Value : d2.Value;
        DateTime tmpDate = startDate;

        int yearCounter = 0;
        while (umalqura.AddYears(tmpDate, 1).AddDays(-1) <= endDate)
        {
            yearCounter++;
            tmpDate = umalqura.AddYears(tmpDate, 1);
        }
        return yearCounter;
    }

    public static DateTime GetGregDate(string hijriDate)
    {
        if (string.IsNullOrWhiteSpace(hijriDate) || !hijriDate.Contains('-')) return DateTime.MinValue;

        string[] hijriDateParts = hijriDate.Split('-');
        int year = Convert.ToInt32(hijriDateParts[0]);
        int month = Convert.ToInt32(hijriDateParts[1]);
        int day = Convert.ToInt32(hijriDateParts[2]);
        return umalqura.ToDateTime(year, month, day, 0, 0, 0, 0);
    }

    public static string GetHijriDateString(DateTime gregDate)
    {
        if (gregDate == DateTime.MinValue) return "";
     

        return string.Format("{0}-{1:00}-{2:00}",
                   umalqura.GetYear(gregDate),
                   umalqura.GetMonth(gregDate),
                   umalqura.GetDayOfMonth(gregDate)
               );
    }
    public static string GetHijriDateFullString(DateTime gregDate)
    {
        return string.Format("{0} - {1} - {2}",
                   gregDate.ToString("ddd d MMM yyyy", cInfoSA),
                   gregDate.ToString("d MMM yyyy", cInfoEG),
                   gregDate.AddHours(3).ToString("hh:mm tt", cInfoSA)
               );
    }

    public static string GetHijriYearString()
    {
        return string.Format("{0}",umalqura.GetYear(DateTime.Now));
    }
}