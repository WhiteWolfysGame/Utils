using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils.MethodExtension
{
    /// <summary>
    /// Diese statische Klasse enthält erweiterte Methoden für den Datentyp <see cref="string"/>.
    /// </summary>
    public static class StringExtension
    {
        /// <summary>
        /// Erstes Zeichen in Großschreibung
        /// </summary>
        /// <param name="input">String, bei der das erste Zeichen in ein Großbuchstaben gesetzt wird.</param>
        /// <returns></returns>
        public static string FirstCharToUpper(this string input)
        {
            if (String.IsNullOrEmpty(input))
                throw new ArgumentException();
            return input.First().ToString().ToUpper() + input.Substring(1);
        }

        /// <summary>
        /// Gibt einen Teilstring zwischen zwei angegebenen Parametern innerhalb eines Strings zurück.
        /// </summary>
        /// <param name="source">Der Eingabestring, in dem der Teilstring gesucht wird.</param>
        /// <param name="start">Der String, der den Anfang des gewünschten Teilstrings markiert.</param>
        /// <param name="end">Der String, der das Ende des gewünschten Teilstrings markiert.</param>
        /// <returns>Der Teilstring zwischen den Start- und Endparametern oder ein leerer String, wenn einer der Parameter nicht gefunden wird.</returns>
        public static string GetSubstringBetween(this string source, string start, string end)
        {
            int startIndex = source.IndexOf(start) + start.Length;
            int endIndex = source.IndexOf(end, startIndex);
            return source.Substring(startIndex, endIndex - startIndex);
        }

        /// <summary>
        /// Überprügt ob die Eingabe eine numerische Zeichenfolge ist
        /// </summary>
        /// <param name="valueToCheck">Der zu prüfende Wert</param>
        /// <returns>true, wenn die eingegebene Zeichenkette eine numerische Zeichenfolge ist</returns>
        public static bool IsNumeric(this string valueToCheck)
        {
            int num;
            return int.TryParse(valueToCheck, out num);
        }

        /// <summary>
        /// Entfernt bestimmte Zeichen aus einem <see cref="string"/>.
        /// </summary>
        /// <param name="text">Die zu bearbeitende </param>
        /// <param name="characterToRemove">zu entfernende Zeichen</param>
        /// <returns>Zeichenkette nach den entfernten Zeichen</returns>
        public static string RemoveCharacters(this string text, params string[] characterToRemove)
        {
            foreach (string s in characterToRemove)
            {
                text = text.Replace(s, "");
            }
            return text;
        }

        /// <summary>
        /// Konvertiert einen String in ein Enum-Value vom definierten Typenparameter "T".
        /// </summary>
        /// <typeparam name="T">Typ des Enum</typeparam>
        /// <param name="value">String der in ein Enum-Wert konvertiert werden soll</param>
        /// <returns>Enum-Value vom definierten Typ "T".</returns>
        public static T ToEnum<T>(this string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }

        /// <summary>
        /// Konvertiert einen String in ein Integer
        /// </summary>
        /// <param name="value">String, der in ein Integer-Wert konvertiert werden soll</param>
        /// <returns>Zahlenwert</returns>
        public static int ToInt(this string value)
        {
            int v = 0;
            int.TryParse(value, out v);
            return v;
        }

        /// <summary>
        /// Entfernt doppelte Elemente aus einem String-Array, während die Reihenfolge der Elemente beibehalten wird.
        /// </summary>
        /// <param name="inputArray">Eingabe-Array mit möglichen Duplikaten</param>
        /// <returns>Ein neues Array mit eindeutigen Elementen in der ursprünglichen Reihenfolge.</returns>
        public static string[] RemoveDuplicates(this string[] inputArray)
        {
            HashSet<string> uniqueSet = new HashSet<string>();
            List<string> uniqueList = new List<string>();

            for(int i = 0; i < inputArray.Length; i++)
            {
                if (uniqueSet.Add(inputArray[i]))
                {
                    uniqueList.Add(inputArray[i]);
                }
            }

            return uniqueList.ToArray();
        }
    }
}
