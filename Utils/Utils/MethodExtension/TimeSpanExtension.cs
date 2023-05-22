using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils.MethodExtension
{
    /// <summary>
    /// Diese statische Klasse enthält erweiterte Methoden für den Datentyp <see cref="TimeSpan"/>.
    /// </summary>
    public static class TimeSpanExtension
    {
        /// <summary>
        /// Diese Funktion rundet eine TimeSpan auf zwei Nachkommastellen und gibt eine formatierte Zeichenfolge zurück, 
        /// die die gerundete Zeit in Stunden, Minuten, Sekunden und Millisekunden darstellt.
        /// </summary>
        /// <param name="time">Die TimeSpan, die gerundet werden soll.</param>
        /// <returns>Eine Zeichenfolge, die die gerundete Zeit in Stunden, Minuten, Sekunden und
        /// Millisekunden in der Form "hh:mm:ss.ff" darstellt.</returns>
        public static string GetRoundedTimeString(this TimeSpan time)
        {
            return time.GetRoundedTimeString(2);
        }

        /// <summary>
        /// Diese Funktion rundet eine TimeSpan auf eine bestimmte Anzahl von Nachkommastellen 
        /// und gibt eine formatierte Zeichenfolge zurück, die die gerundete Zeit in 
        /// Stunden, Minuten, Sekunden und Millisekunden darstellt.
        /// </summary>
        /// <param name="time">Die TimeSpan, die gerundet werden soll.</param>
        /// <param name="decimalPlaces">Die Anzahl der Nachkommastellen, auf die die TimeSpan gerundet werden soll.</param>
        /// <returns>Eine Zeichenfolge, die die gerundete Zeit in Stunden, Minuten, Sekunden und Millisekunden in der Form "hh:mm:ss.ff" darstellt, wobei "ff" die angegebene Anzahl von Nachkommastellen enthält.</returns>
        public static string GetRoundedTimeString(this TimeSpan time, int decimalPlaces)
        {
            double roundedMilliseconds = Math.Round(time.TotalMilliseconds, decimalPlaces);
            TimeSpan roundedTimeSpan = TimeSpan.FromMilliseconds(roundedMilliseconds);

            if (time.TotalSeconds < 60) { return time.ToString(@"ss\." + new String('f', decimalPlaces)); }
            else if (time.TotalMinutes < 60) { return time.ToString(@"mm\:ss\." + new String('f', decimalPlaces)); }
            else { return time.ToString(@"hh\:mm\:ss\." + new String('f', decimalPlaces)); }
        }
    }
}
