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
                lst.Cells[1, i + 1] = migrantsKlienti.Columns[i].Header;
            }

            for (i = 0; i < migrantsKlienti.Items.Count; i++)
            {
                DataRowView klientRow = (DataRowView) migrantsKlienti.Items[i];
                for (j = 0; j < migrantsKlienti.Columns.Count; j++)
                {
                    lst.Cells[i + 2, j + 1] = klientRow[j];
                }
            }

            lst.Range[lst.Cells[1, 1], lst.Cells[1, j + 1]].HorizontalAlignment = 3;
            lst.Cells.Columns.EntireColumn.AutoFit();
            lst.Range[lst.Cells[1, 1], lst.Cells[i + 1, j]].Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
            lst.Rows[1].Insert();
            lst.Range[lst.Cells[1, 1], lst.Cells[1, j]].Merge();
            lst.Cells[1, 1] = "Период формирования отчёта с " + dataEnd.Text + " до " + dataStart.Text;
            lst.Cells[1, 1].HorizontalAlignment = 3;
        }

        private void Filter(object sender, RoutedEventArgs e)
        {
            if (dataStart.SelectedDate != null && dataStart.SelectedDate != null)
            {
                Pull(dataStart.Text, dataEnd.Text);
                migrantsKlienti.ItemsSource = Connect.Ds.Tables["InostrKli"].DefaultView;
            }
        }

        private void Migrants_OnLoaded(object sender, RoutedEventArgs e)
        {
            dataStart.Text = dataStart.DisplayDate.Date.ToShortDateString();
            dataEnd.Text = dataEnd.DisplayDate.Date.ToShortDateString();
            migrantsKlienti.AutoGenerateColumns = true;
            migrantsKlienti.CanUserAddRows = false;
            migrantsKlienti.HeadersVisibility = DataGridHeadersVisibility.Column;
            migrantsKlienti.IsReadOnly = true;
            migrantsKlienti.SelectionMode = DataGridSelectionMode.Extended;
        }

        private void Pull(string date_start, string date_end)
        {
            string sql = "SELECT CONCAT(Familiya, ' ', Imya, ' ', Otchestvo) AS [ФИО клиента], " +
                         "CONCAT(Seriya, ', ', Nomer, ', ', VidDokumenta, ', ', Vidan, ', ', StranaVidachi) AS [Паспортные данные], " +
                         "CONCAT(NomerKarti, ', ', Otkuda, ', ', PrebivanieS, ', ', PrebivaniePo, ', ', CelPoezdki) AS [Данные миграционной карты] " +
                         "FROM ((Klient INNER JOIN Dogovor ON Klient.IdKlienta = Dogovor.IdKlienta) " +
                         "INNER JOIN Pasport ON Klient.IdKlienta = Pasport.IdKlienta) " +
                         "INNER JOIN MigracionnayaKarta ON Klient.IdKlienta = MigracionnayaKarta.IdKlienta " +
                         "WHERE DataZaezda >= '" + date_start + "' AND DataViezda <= '" + date_end + "'";
            Connect.Table_Fill("InostrKli", sql);
        }
    }
}
