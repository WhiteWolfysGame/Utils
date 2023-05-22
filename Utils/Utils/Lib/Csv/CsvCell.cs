using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils.Lib.Csv
{
    /// <summary>
    /// Repräsentiert eine einzelne Zelle in einer CSV-Datei.
    /// </summary>
    public class CsvCell
    {
        /// <summary>
        /// Der Wert dieser CSV-Zelle.
        /// </summary>
        public string Value { get; private set; }
        /// <summary>
        /// Der Index dieser CSV-Zelle.
        /// </summary>
        public int Index { get; private set; }

        /// <summary>
        /// Initialisiert eine neue Instanz der CsvCell-Klasse.
        /// </summary>
        public CsvCell() { }

        /// <summary>
        /// Initialisiert eine neue Instanz der CsvCell-Klasse mit einem angegebenen Index und Wert.
        /// </summary>
        /// <param name="id">Der Index dieser CSV-Zelle innerhalb der CSV-Zeile.</param>
        /// <param name="value">Der Wert dieser CSV-Zelle.</param>
        public CsvCell(int id, string value)
        {
            this.Index = id;
            this.Value = value;
        }
    }
}
