using System.Data;
using System.Windows;
using System.Windows.Controls;
using Excel = Microsoft.Office.Interop.Excel;

namespace Hotel_Administration2._0
{
    public partial class migrants : Page
    {
        public migrants()
        {
            InitializeComponent();
        }

        private void Print_OnClick(object sender, RoutedEventArgs e)
        {
            Excel.Application app = new Excel.Application();
            app.Visible = true;
            Excel.Workbook book = app.Workbooks.Add();
            Excel.Worksheet lst = book.Worksheets[1];
            lst.Name = "Иностранные клиенты";
            int i = 0, j = 0;
            for (i = 0; i < migrantsKlienti.Columns.Count; i++)
            {
                lst.Cells[1, i + 1] = dataGridView1.Columns[i].HeaderText;
            }
            for (i = 0; i < dataGridView1.RowCount; i++)
            {
                for (j = 0; j < dataGridView1.ColumnCount; j++)
                {
                    lst.Cells[i + 2, j + 1] = dataGridView1[j, i].Value;
                }
            }
            lst.Range[lst.Cells[1, 1], lst.Cells[1, j + 1]].HorizontalAlignment = 3;
            lst.Cells.Columns.EntireColumn.AutoFit();
            lst.Range[lst.Cells[1, 1], lst.Cells[i + 1, j]].Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
            lst.Rows[1].Insert();
            lst.Range[lst.Cells[1, 1], lst.Cells[1, j]].Merge();
            lst.Cells[1, 1] = "Период формирования отчёта с " + dateTimePicker1.Value.ToShortDateString() + " до " + dateTimePicker2.Value.ToShortDateString();
            lst.Cells[1, 1].HorizontalAlignment = 3;
        }

        private void Filter(object sender, RoutedEventArgs e)
        {

        }
    }
}
