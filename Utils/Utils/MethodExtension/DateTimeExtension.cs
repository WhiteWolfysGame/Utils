using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils.MethodExtension
{
    /// <summary>
    /// Diese statische Klasse enthält erweiterte Methoden für den Datentyp <see cref="DateTime"/>.
    /// </summary>
    public static class DateTimeExtension
    {

        /// <summary>
        /// Convertiert ein <see cref="DateTime"/> aus einer bestimmten Zeitzone in das UTC-Format
        /// </summary>
        /// <param name="instance">das aktuelle <see cref="DateTime"/>-Objekt</param>
        /// <param name="fullLengthedTimezone">Voll ausgeschriebene Zeitzone</param>
        /// <param name="utcZone">UTC Score (Beispiel: Berlin, Brüssel, Rom UTC+1)</param>
        /// <returns>Datum der UTC-Zone</returns>
        public static DateTime ConvertFromTimezoneToUtc(this DateTime instance, string fullLengthedTimezone, int utcZone)
        {
            var utcTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(instance, fullLengthedTimezone, "UTC");
            return utcTime.AddHours(utcZone);
        }

        /// <summary>
        /// Convertiert ein <see cref="DateTime"/> vom UTC in eine bestimmte Zeitzone
        /// </summary>
        /// <param name="instance">das aktuelle <see cref="DateTime"/>-Objekt</param>
        /// <param name="fullLenghedTimezone">Voll ausgeschriebene Zeitzone, in dass die Zeit convertiert werden soll</param>
        /// <param name="utcZone">UTC Score (Beispiel: Berlin, Brüssel, Rom UTC+1)</param>
        /// <returns>Datum der UTC-Zone</returns>
        public static DateTime ConvertToSpecificTimezone(this DateTime instance, string fullLenghedTimezone, int utcZone)
        {
            DateTime dt = DateTime.Parse(instance.ToString("dd.MM.yyyy HH:mm"));
            var utcTime = dt.AddHours(utcZone * -1);
            var customTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(utcTime, "UTC", fullLenghedTimezone);
            return customTime;
        }

        /// <summary>
        /// Gibt das Alter in Jahren basierend auf diesem Datum zurück
        /// </summary>
        /// <param name="birthDate">das aktuelle <see cref="DateTime"/>-Objekt</param>
        /// <returns>Alter in Jahren</returns>
        public static int GetAge(this DateTime birthDate)
        {
            var today = DateTime.Today;
            var age = today.Year - birthDate.Year;
            if (birthDate > today.AddYears(-age))
                age--;
            return age;
        }
        
        /// <summary>
        /// Sucht einen bestimmten Tag in einem Monat
        /// </summary>
        /// <param name="instance">das aktuelle <see cref="DateTime"/>-Objekt.</param>
        /// <param name="dayOfWeek">der zu suchende Tag</param>
        /// <param name="occurance">Der 1. Tag - maximal 5. Tag im Monat</param>
        /// <param name="dateOfMonth">Gibt ein <see cref="DateTime"/>-Objekt mit dem gesuchten Tag heraus</param>
        /// <returns>True, wenn Tag im Monat dem gesuchten Tag in demselben Monat entspricht</returns>
        public static bool TryGetDayOfMonth(this DateTime instance, DayOfWeek dayOfWeek, int occurance, out DateTime dateOfMonth)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("instance");
            }

            if (occurance <= 0 || occurance > 5)
            {
                throw new ArgumentOutOfRangeException("occurance", "Occurance must be greater than zero and less than 6.");
            }

            bool result;
            dateOfMonth = new DateTime();

            // Change to first day of the month
            DateTime dayOfMonth = instance.AddDays(1 - instance.Day);

            // Find first dayOfWeek of this month;
            if (dayOfMonth.DayOfWeek > dayOfWeek)
            {
                dayOfMonth = dayOfMonth.AddDays(7 - (int)dayOfMonth.DayOfWeek + (int)dayOfWeek);
            }
            else
            {
                dayOfMonth = dayOfMonth.AddDays((int)dayOfWeek - (int)dayOfMonth.DayOfWeek);
            }

            // add 7 days per occurance
            dayOfMonth = dayOfMonth.AddDays(7 * (occurance - 1));

            // make sure this occurance is within the original month
            result = dayOfMonth.Month == instance.Month;


            if (result)
            {
                dateOfMonth = dayOfMonth;
            }

            return result;
        }
    }
}
