using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils.MethodExtension
{
    /// <summary>
    /// Diese statische Klasse enthält erweiterte Methoden für den Datentyp <see cref="DataGridViewRowCollection"/>.
    /// </summary>
    public static class DataGridViewRowCollectionExtension
    {
        /// <summary>
        /// Erstellt eine neue Zeile im DataGridView und fügt diese in die DataGridViewRowCollection hinzu
        /// </summary>
        /// <param name="dgvCollection">Die Collection, in der die neue Zeile hinzugefügt werden soll</param>
        /// <param name="values">Wert der Spalte(n)</param>
        public static void CreateRow(this DataGridViewRowCollection dgvCollection, params object[] values)
        {
            DataGridViewRow row = new DataGridViewRow();

            foreach (object value in values)
            {
                Type type = value.GetType();

                //muss gegebenfalls erweitert werden!
                if (type == typeof(Image)) { CreateImageCell(row, value); }
                else if (type == typeof(Bitmap)) { CreateImageCell(row, value); }
                else if (type == typeof(string)) { CreateTextCell(row, value); }
                else if (type == typeof(int)) { CreateIntCell(row, value); }
                else if (type == typeof(bool)) { CreateCheckBoxCell(row, value); }
                else if (type == typeof(DataGridViewComboBoxCell)) { CreateComboBoxCell(row, value); }
                else if (type == typeof(DataGridViewButtonCell)) { CreateButtonCell(row, value); }
                else { CreateTextCell(row, value); }
            }

            dgvCollection.Add(row);
        }

        private static void CreateButtonCell(DataGridViewRow row, object value)
        {
            DataGridViewButtonCell buttonCell = (DataGridViewButtonCell)value;
            row.Cells.Add(buttonCell);
        }

        private static void CreateCheckBoxCell(DataGridViewRow row, object value)
        {
            DataGridViewCheckBoxCell checkBoxCell = new DataGridViewCheckBoxCell();
            checkBoxCell.Value = (bool)value;
            row.Cells.Add(checkBoxCell);
        }

        private static void CreateComboBoxCell(DataGridViewRow row, object value)
        {
            DataGridViewComboBoxCell comboBoxCell = (DataGridViewComboBoxCell)value;
            row.Cells.Add(comboBoxCell);
        }

        private static void CreateImageCell(DataGridViewRow row, object value)
        {
            DataGridViewImageCell imageCell = new DataGridViewImageCell();
            imageCell.Value = (Image)value;
            imageCell.ImageLayout = DataGridViewImageCellLayout.Zoom;
            row.Cells.Add(imageCell);
        }

        private static void CreateIntCell(DataGridViewRow row, object value)
        {
            DataGridViewTextBoxCell intCell = new DataGridViewTextBoxCell();
            intCell.Value = (int)value;
            row.Cells.Add(intCell);
        }

        private static void CreateTextCell(DataGridViewRow row, object value)
        {
            DataGridViewTextBoxCell textCell = new DataGridViewTextBoxCell();
            textCell.Value = (string)value;
            row.Cells.Add(textCell);
        }
    }
}
