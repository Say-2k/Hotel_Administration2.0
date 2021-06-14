using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Hotel_Administration2._0
{
    public partial class viselenie : Page
    {
        public viselenie()
        {
            InitializeComponent();
        }

        private void Viselenie_OnLoaded(object sender, RoutedEventArgs e)
        {
            string sql = "SELECT GostinichniyNomer.CenaZaSutki, Klient.BonusnayaKarta, NomerDogovora AS [Номер договора], Dogovor.NomerPomesheniya AS [Номер помещения], CONCAT(Familiya, ' ', Imya, ' ', Otchestvo) AS [ФИО клиента], DataZaezda AS [Дата заезда], DataViezda AS [Дата выезда], Dogovor.SummaOplati " +
                         "FROM (Dogovor INNER JOIN Klient ON Dogovor.IdKlienta = Klient.IdKlienta) " +
                         "INNER JOIN GostinichniyNomer ON Dogovor.NomerPomesheniya = GostinichniyNomer.NomerPomesheniya " +
                         "WHERE Status = 'занят' AND Dogovor.IdKlienta = GostinichniyNomer.IdKlienta";
            Connect.Table_Fill("Dogovora", sql);
            Connect.Ds.Tables["Dogovora"].DefaultView.RowFilter = "[Дата выезда] = '" + dataViseleniya_Filter.DisplayDate.Date + "'";
            dogovora.ItemsSource = Connect.Ds.Tables["Dogovora"].DefaultView;
            dogovora.Columns[0].Visibility = Visibility.Hidden;
            dogovora.Columns[1].Visibility = Visibility.Hidden;
            dogovora.Columns[7].Visibility = Visibility.Hidden;
            (dogovora.Columns[5] as DataGridTextColumn).Binding.StringFormat = "dd.MM.yyyy";
            (dogovora.Columns[6] as DataGridTextColumn).Binding.StringFormat = "dd.MM.yyyy";
            dogovora.AutoGenerateColumns = true;
            dogovora.CanUserAddRows = false;
            dogovora.HeadersVisibility = DataGridHeadersVisibility.Column;
            dogovora.IsReadOnly = true;
            dogovora.SelectionMode = DataGridSelectionMode.Extended;
            dogovor.IsEnabled = false;
            Confirm.IsEnabled = false;
        }

        private void Dogovora_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dogovora.SelectedIndex >= 0)
            {
                DataRowView dogovorRow = (DataRowView)dogovora.SelectedItem;
                nomerDogovora.Text = dogovorRow[2].ToString();
                nomerPomasheniya.Text = dogovorRow[3].ToString();
                fioklienta.Text = dogovorRow[4].ToString();
                dataVseleniya.Text = dogovorRow[5].ToString();
                dataViseleniya.Text = dogovorRow[6].ToString();
                dogovor.IsEnabled = true;
                Confirm.IsEnabled = true;
            }
        }

        private void DataViseleniya_Filter_OnSelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            Connect.Ds.Tables["Dogovora"].DefaultView.RowFilter = "[Дата выезда] = '" + dataViseleniya_Filter.Text + "'";
        }

        private void DataViseleniya_OnSelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dogovora.SelectedIndex > -1)
            {
                DataRowView dogovorRow = (DataRowView)dogovora.SelectedItem;
                if (Convert.ToDateTime(dataViseleniya.Text) == Convert.ToDateTime(dogovorRow[6]))
                {
                    Confirm.Content = "Выселить";
                }
                else
                {
                    Confirm.Content = "Перенести";
                }
            }
        }

        private void Confirm_OnClick(object sender, RoutedEventArgs e)
        {
            DataRowView dogovorRow = (DataRowView)dogovora.SelectedItem;
            if (Convert.ToDateTime(dataViseleniya.Text) == Convert.ToDateTime(dogovorRow[6]))
            {
                string sql = "UPDATE GostinichniyNomer SET Status = 'свободен', IdKlienta = null WHERE NomerPomesheniya = " + nomerPomasheniya.Text;
                Connect.Modification_Execute(sql);
                MessageBox.Show("Клиент выселился");
                NavigationService.Navigate(new viselenie());
            }
            else
            {
                string sql = "UPDATE Dogovor SET DataViezda = '" + dataViseleniya.Text + "' WHERE NomerDogovora = " + nomerDogovora.Text;
                Connect.Modification_Execute(sql);
                int span = dataViseleniya.SelectedDate.Value.Subtract(dataVseleniya.SelectedDate.Value).Days;
                double procent = 0;
                if (dogovorRow[1].ToString() == "платиновая")
                {
                    procent = 0.95;
                }
                else if (dogovorRow[1].ToString() == "золотая")
                {
                    procent = 0.97;
                }
                else if (dogovorRow[1].ToString() == "обычная")
                {
                    procent = 0.99;
                }
                else if (dogovorRow[1].ToString() == "нет")
                {
                    procent = 1;
                }
                double plus = span * procent * Convert.ToDouble(dogovorRow[0]);
                sql = "UPDATE Dogovor SET SummaOplati = " + plus + " WHERE NomerDogovora = " + nomerDogovora.Text;
                Connect.Modification_Execute(sql);
                MessageBox.Show("Дата выезда клиента успешно перенесена");
                NavigationService.Navigate(new viselenie());
            }
        }
    }
}
