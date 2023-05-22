using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils.MethodExtension
{
    /// <summary>
    /// Diese statische Klasse enthält erweiterte Methoden für den Datentyp <see cref="ListExtension"/>.
    /// </summary>
    public static class ListExtension
    {
        private static Random rng = new Random();

        /// <summary>
        /// Diese Funktion mischt ihre Elemente in einer zufälligen Reihenfolge.
        /// Diese Funktion verwendet den Fisher-Yates-Shuffle Algorithmus.
        /// </summary>
        /// <typeparam name="T">generische Typvariable für einen Datentyp</typeparam>
        /// <param name="list">Liste die gemischt werden soll</param>
        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        /// <summary>
        /// Diese Funktion gibt eine neue gemischte Liste zurück und ändert die ursprüngliche Liste nicht.
        /// Diese Funktion verwendet den Fisher-Yates-Shuffle Algorithmus.
        /// </summary>
        /// <typeparam name="T">generische Typvariable für einen Datentyp</typeparam>
        /// <param name="list">Liste die gemischt werden soll</param>
        /// <returns>Gibt eine neue gemischte Liste zurück.</returns>
        public static IList<T> ShuffleAndReturn<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }

            return list;
        }
    }
}
