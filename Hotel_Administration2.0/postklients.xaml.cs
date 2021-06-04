using System.Data;
using System.Windows;
using System.Windows.Controls;
using Excel = Microsoft.Office.Interop.Excel;

namespace Hotel_Administration2._0
{
    public partial class postklients : Page
    {
        public postklients()
        {
            InitializeComponent();
        }

        private void Print_OnClick(object sender, RoutedEventArgs e)
        {
            Excel.Application app = new Excel.Application();
            app.Visible = true;
            Excel.Workbook book = app.Workbooks.Add();
            Excel.Worksheet lst = book.Sheets[1];
            lst.Name = "Постоянные клиенты";
            int i = 0, j = 0;
            for (i = 1; i < postoyannieKlienti.Columns.Count; i++)
            {
                lst.Cells[1, i] = postoyannieKlienti.Columns[i].Header;
            }
            for (i = 0; i < postoyannieKlienti.Items.Count; i++)
            {
                DataRowView klientRow = (DataRowView)postoyannieKlienti.Items[i];
                for (j = 1; j < postoyannieKlienti.Columns.Count; j++)
                {
                    lst.Cells[i + 2, j] = klientRow[j];
                }
            }
            lst.Range[lst.Cells[1, 1], lst.Cells[1, j]].HorizontalAlignment = 3;
            lst.Cells.Columns.EntireColumn.AutoFit();
            lst.Range[lst.Cells[1, 1], lst.Cells[i + 1, j - 1]].Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
            lst.Rows[1].Insert();
            lst.Range[lst.Cells[1, 1], lst.Cells[1, j - 1]].Merge();
            lst.Cells[1, 1] = "Период формирования отчёта с " + dataDogovora_Start.Text + " по " + dataDogovora_End.Text;
            lst.Cells[1, 1].HorizontalAlignment = 3;
        }

        private void Filter(object sender, RoutedEventArgs e)
        {
            if (dataDogovora_Start.SelectedDate != null && dataDogovora_End.SelectedDate != null) 
            {
                Pull(dataDogovora_Start.SelectedDate.Value.ToShortDateString(), dataDogovora_End.SelectedDate.Value.ToShortDateString());
                postoyannieKlienti.ItemsSource = Connect.Ds.Tables["postklients"].DefaultView;
            }
        }

        private void Postklients_OnLoaded(object sender, RoutedEventArgs e)
        {
            dataDogovora_Start.Text = dataDogovora_Start.DisplayDate.Date.ToShortDateString();
            dataDogovora_End.Text = dataDogovora_End.DisplayDate.Date.ToShortDateString();
            postoyannieKlienti.ItemsSource = Connect.Ds.Tables["postklients"].DefaultView;
            postoyannieKlienti.Columns[0].Visibility = Visibility.Hidden;
            postoyannieKlienti.AutoGenerateColumns = true;
            postoyannieKlienti.CanUserAddRows = false;
            postoyannieKlienti.HeadersVisibility = DataGridHeadersVisibility.Column;
            postoyannieKlienti.IsReadOnly = true;
            postoyannieKlienti.SelectionMode = DataGridSelectionMode.Extended;
        }

        private void Pull(string date_start, string date_end)
        {
            string sql = "SELECT Dogovor.IdKlienta, " +
                         "CONCAT(Familiya, ' ', Imya, ' ', Otchestvo) AS [ФИО клиента], " +
                         "BonusnayaKarta AS [Вид бонусной карты], " +
                         "COUNT(*) AS [Количество договоров за указанный период], " +
                         "SUM(SummaOplati) AS [Общая сумма оплаты] " +
                         "FROM Dogovor INNER JOIN Klient ON Dogovor.IdKlienta = Klient.IdKlienta " +
                         "WHERE DataDogovora >= '" + date_start + "' AND DataDogovora <= '" + date_end + "' " +
                         "GROUP BY Dogovor.IdKlienta, Familiya, Imya, Otchestvo, BonusnayaKarta " +
                         "HAVING COUNT(*) >= 3";
            Connect.Table_Fill("postklients", sql);
        }
    }
}
