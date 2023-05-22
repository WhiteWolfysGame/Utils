using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils.Lib.Csv
{
    /// <summary>
    /// Eine Klasse, die eine Spalte in einem CSV-Dokument darstellt.
    /// </summary>
    public class CsvColumn
    {
        /// <summary>
        /// Der Titel der Spalte
        /// </summary>
        public string Title { get; private set; }
        /// <summary>
        /// Der Index der Spalte
        /// </summary>
        public int ColumnIndex { get; private set; }
        /// <summary>
        /// Ein Array von <see cref="CsvCell"/>-Objekten, das die Zellen in der Spalte darstellt.
        /// </summary>
        public CsvCell[] Rows { get; set; }

        /// <summary>
        /// Initialisiert eine neue <see cref="CsvColumn"/>-Instanz mit einem gegebenen Titel
        /// Der Index der Spalte wird aus der Index-Eigenschaft des übergebenen <see cref="CsvCell"/>-Objekts extrahiert
        /// </summary>
        /// <param name="title">Das <see cref="CsvCell"/>-Objekt, das den Titel der Spalte enthält.</param>
        public CsvColumn(CsvCell title)
        {
            this.Title = title.Value;
            this.ColumnIndex = title.Index;
        }
    }
}
