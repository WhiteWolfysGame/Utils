using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils.MethodExtension
{
    /// <summary>
    /// Diese statische Klasse stellt erweiterte Methoden für das Control <see cref="DataGridView"/> bereit.
    /// </summary>
    public static class DataGridViewExtension
    {
        /// <summary>
        /// Scrollt die DataGridView zur ausgewählten Zeile, um sicherzustellen, dass sie vollständig sichtbar ist.
        /// </summary>
        /// <param name="dgv">Die DataGridView, auf die die Erweiterung angewendet wird.</param>
        /// <remarks>
        /// Diese Methode stellt sicher, dass die ausgewählte Zeile in der DataGridView sichtbar ist, indem sie gegebenenfalls scrollt.
        /// </remarks>
        /// <example>
        /// <code>
        /// // Verwenden Sie die Erweiterungsmethode, um zur ausgewählten Zeile zu scrollen.
        /// dgv.ScrollToSelectedRow();
        /// </code>
        /// </example>
        /// <seealso cref="IsRowVisible(DataGridView, int)"/>
        public static void ScrollToSelectedRow(this DataGridView dgv)
        {
            if (dgv.SelectedRows.Count > 0)
            {
                int selectedRowIndex = dgv.SelectedRows[0].Index;

                // Überprüfen Sie, ob die ausgewählte Zeile sichtbar ist
                if (!IsRowVisible(dgv, selectedRowIndex))
                {
                    // Wenn nicht sichtbar, scrollen Sie zur ausgewählten Zeile
                    dgv.CurrentCell = dgv.Rows[selectedRowIndex].Cells[0];
                    dgv.FirstDisplayedScrollingRowIndex = selectedRowIndex;
                }
            }
        }

        /// <summary>
        /// Überprüft, ob die angegebene Zeile in der DataGridView sichtbar ist.
        /// </summary>
        /// <param name="dgv">Die DataGridView, auf die die Erweiterung angewendet wird.</param>
        /// <param name="rowIndex">Der Index der zu überprüfenden Zeile.</param>
        /// <returns>
        /// <c>true</c>, wenn die Zeile sichtbar ist, andernfalls <c>false</c>.
        /// </returns>
        /// <seealso cref="ScrollToSelectedRow"/>
        private static bool IsRowVisible(DataGridView dgv, int rowIndex)
        {
            // Überprüfen Sie, ob die Zeile sichtbar ist
            if (rowIndex < dgv.FirstDisplayedScrollingRowIndex || rowIndex > dgv.FirstDisplayedScrollingRowIndex + dgv.DisplayedRowCount(true) - 1)
            {
                return false;
            }

            return true;
        }
    }
}
