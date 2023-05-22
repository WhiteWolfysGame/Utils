using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils.Lib.Csv
{
    /// <summary>
    /// Die Klasse "Csv" ist eine öffentliche Klasse, 
    /// die eine einfache Implementierung zum Lesen und Schreiben von CSV-Dateien bietet. 
    /// Die Klasse enthält Methoden zum Lesen und Schreiben von CSV-Dateien sowie Eigenschaften, 
    /// die die Trennzeichen und Zeilenenden der CSV-Dateien steuern.
    /// </summary>
    public class Csv
    {
        private int cols;
        private string filename;
        private char separator;
        private StreamReader stream;

        private CsvCell[] headerCells;
        private List<CsvCell[]> rows;

        /// <summary>
        /// Gibt die Spalten der CSV-Datei zurück
        /// </summary>
        public CsvColumn[] Columns { get; private set; }

        /// <summary>
        /// Gibt den Namen der Datei zurück
        /// </summary>
        public string Filename { get { return this.filename; } }

        /// <summary>
        /// Gibt die Anzahl der Zeilen zurück
        /// </summary>
        public int NumRows { get { return this.rows.Count; } }

        /// <summary>
        /// Gibt die Anzahl der Spalten zurück
        /// </summary>
        public int NumColumns { get { return this.cols; } }

        /// <summary>
        /// Erstellt eine neue Instanz vom Typ <see cref="Csv"/> um CSV-Dateien zu lesen
        /// </summary>
        /// <param name="file">Dateipfad der zu öffnenden Datei</param>
        /// <param name="separator">Trennzeichen in der CSV-Datei</param>
        public Csv(string file, char separator)
        {
            this.filename = file;
            this.separator = separator;
            this.cols = 0;
            this.rows = new List<CsvCell[]>();

            readCsvFile();
            manageCsvColumns();
        }

        /// <summary>
        /// Liest die CSV-Datei und Organisiert die Einträge
        /// </summary>
        private void readCsvFile()
        {
            getFilestream();
            bool headerSetted = false;

            while (!this.stream.EndOfStream)
            {
                headerSetted = buildHeader(headerSetted);
                buildRows();
            }
            this.stream.Close();
        }

        /// <summary>
        /// Liefert den zu lesenden Datenstream
        /// </summary>
        /// <returns>StreamReader</returns>
        /// <exception cref="FileNotFoundException">Ausnahme, wenn die angegebene Datei nicht existiert</exception>
        private void getFilestream()
        {
            if (File.Exists(filename))
            {
                FileStream fs = new FileStream(this.filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                this.stream = new StreamReader(fs);
            }
            else
            {
                throw new FileNotFoundException(filename);
            }
        }

        /// <summary>
        /// Stellt Headerdaten aus der CSV-Datei bereit
        /// </summary>
        /// <param name="headerSetted">Wurde der Header gesetzt?</param>
        /// <returns>true, wenn Header gesetzt wurde. Liefert immer true zurück</returns>
        private bool buildHeader(bool headerSetted)
        {
            if (!headerSetted)
            {
                string[] row = this.stream.ReadLine().Split(this.separator);
                this.cols = row.Length; // Anzahl Spalten
                this.headerCells = new CsvCell[this.cols];
                // Spalten füllen
                for (int i = 0; i < this.cols; i++)
                {
                    this.headerCells[i] = new CsvCell(i, row[i]);
                }
                headerSetted = true;
            }
            return headerSetted;
        }

        /// <summary>
        /// Stellt die Zeilen aus der CSV-Datei bereit
        /// </summary>
        private void buildRows()
        {
            string[] r = this.stream.ReadLine().Split(this.separator);
            CsvCell[] rowCells = new CsvCell[this.cols];
            // Zeilen füllen
            for (int i = 0; i < this.cols; i++)
            {
                rowCells[i] = new CsvCell(i, r[i]);
            }
            this.rows.Add(rowCells);
        }

        /// <summary>
        /// Organisiert die Spalten mit den zugehörigen Einträgen
        /// </summary>
        private void manageCsvColumns()
        {
            List<CsvColumn> cols = new List<CsvColumn>();
            for (int col = 0; col < headerCells.Length; col++)
            {
                CsvColumn c = new CsvColumn(headerCells[col]);
                CsvCell[] columnEntries = new CsvCell[rows.Count];
                for (int r = 0; r < rows.Count; r++)
                {
                    columnEntries[r] = new CsvCell(rows[r][col].Index, rows[r][col].Value);
                }
                c.Rows = columnEntries;
                cols.Add(c);
            }
            this.Columns = cols.ToArray();
        }

        /// <summary>
        /// Gibt die Spalte zurück die im index definiert wird
        /// </summary>
        /// <param name="index">Spaltenindex</param>
        /// <returns>CSV-Spalte</returns>
        public CsvColumn this[int index]
        {
            get
            {
                return Columns[index];
            }
        }

        /// <summary>
        /// Gibt den Spaltenindex zurück, der im keyword definiert wird
        /// </summary>
        /// <param name="keyword">Spaltenname</param>
        /// <returns>Spaltenindex</returns>
        public CsvColumn this[string keyword]
        {
            get
            {
                return (getIndex(keyword));
            }
        }

        /// <summary>
        /// Findet einen bestimmten Spaltentitel
        /// </summary>
        /// <param name="keyword">Spaltentitel</param>
        /// <returns>Index</returns>
        private CsvColumn getIndex(string keyword)
        {
            for (int j = 0; j < Columns.Length; j++)
            {
                if (Columns[j].Title == keyword)
                {
                    return Columns[j];
                }
            }

            throw new System.ArgumentOutOfRangeException(keyword, "keyword muss im Format CSV-Titel vorliegen.");
        }
    }
}
