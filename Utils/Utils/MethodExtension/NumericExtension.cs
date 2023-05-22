using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils.MethodExtension
{
    /// <summary>
    /// Diese statische Klasse enthält erweiterte Methoden für numerische Datentypen.
    /// </summary>
    public static class NumericExtension
    {
        /// <summary>
        /// Konvertiert einen Decimal in ein Integer
        /// </summary>
        /// <param name="value">Decimal, der in ein Integer-Wert konvertiert werden soll</param>
        /// <returns>Zahlenwert</returns>
        public static int ToInt(this decimal value)
        {
            return Convert.ToInt32(value);
        }

        /// <summary>
        /// Prüft ob eine Zahl zwischen zwei definierten Werten liegt
        /// </summary>
        /// <param name="valueToCheck">Der zu überprüfende Wert</param>
        /// <param name="minValue">Kleinster Wert</param>
        /// <param name="maxValue">Höchster Wert</param>
        /// <returns>True wenn die Zahl zwischen dem höchsten und kleinsten Wert liegt</returns>
        public static bool IsBetween(this int valueToCheck, int minValue, int maxValue)
        {
            if (valueToCheck >= minValue && valueToCheck <= maxValue)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
