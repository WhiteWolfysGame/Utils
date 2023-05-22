using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils.MethodExtension
{
    /// <summary>
    /// Diese statische Klasse enthält erweiterte Methoden für den Datentyp <see cref="ListView"/>.
    /// </summary>
    public static class ListViewExtension
    {
        /// <summary>
        /// Überprüft, ob ein ListViewItem mit den angegebenen Werten bereits in der ListView vorhanden ist.
        /// </summary>
        /// <param name="listView">Die ListView, die überprüft werden soll.</param>
        /// <param name="values">Die Werte, die mit den Einträgen in der ListView verglichen werden sollen.</param>
        /// <returns><c>true</c>, wenn ein ListViewItem mit den gleichen Werten bereits vorhanden ist, andernfalls <c>false</c>.</returns>
        public static bool ListViewItemExist(this ListView listView, params string[] values)
        {
            if (values.Length == 0)
                return false;

            foreach (ListViewItem item in listView.Items)
            {
                bool match = true;

                // Überprüfe die Werte für jede Spalte des ListviewItems
                for (int i = 0; i < values.Length; i++)
                {
                    // Vergleiche den Wert des aktuellen ListviewItems mit dem entsprechenden Wert aus den übergebenen Werten
                    if (item.SubItems[i].Text != values[i])
                    {
                        // Wenn die Werte nicht übereinstimmen, setze 'match' auf false und breche die Schleife ab
                        match = false;
                        break;
                    }
                }

                // Wenn alle Werte übereinstimmen, wurde ein Duplikat gefunden und wir geben true zurück
                if (match)
                    return true;
            }

            return false;
        }
    }
}
