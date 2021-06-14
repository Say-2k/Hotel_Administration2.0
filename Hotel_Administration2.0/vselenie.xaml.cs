using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;

namespace Hotel_Administration2._0
{
    public partial class vselenie : Page
    {
        public vselenie()
        {
            InitializeComponent();
        }

        private static int k = 0, n = 0;

        private void Vselenie_OnLoaded(object sender, RoutedEventArgs e)
        {
            string sql = "SELECT IdKlienta, " +
                         "CONCAT(Familiya, ' ', Imya, ' ', Otchestvo) AS [ФИО клиента], " +
                         "BonusnayaKarta AS [Вид бонусной карты] " +
                         "FROM Klient";
            Connect.Table_Fill("Klient", sql);
            klients.ItemsSource = Connect.Ds.Tables["Klient"].DefaultView;
            klients.Columns[0].Visibility = Visibility.Hidden;
            klients.AutoGenerateColumns = true;
            klients.CanUserAddRows = false;
            klients.HeadersVisibility = DataGridHeadersVisibility.Column;
            klients.IsReadOnly = true;
            klients.SelectionMode = DataGridSelectionMode.Extended;
            ///
            sql = "SELECT NomerPomesheniya, " +
                  "Oboznachenie AS [Обозначение категории], " +
                  "Opisanie AS [Описание номера], " +
                  "CenaZaSutki AS [Цена за сутки], " +
                  "KolichestvoMest AS [Количество мест] " +
                "FROM GostinichniyNomer " +
                  "INNER JOIN Kategoriya ON GostinichniyNomer.IdKategorii = Kategoriya.IdKategorii " +
                  "WHERE Status = 'свободен'";
            Connect.Table_Fill("Nomer", sql);
            aparts.ItemsSource = Connect.Ds.Tables["Nomer"].DefaultView;
            aparts.Columns[0].Visibility = Visibility.Hidden;
            aparts.AutoGenerateColumns = true;
            aparts.CanUserAddRows = false;
            aparts.HeadersVisibility = DataGridHeadersVisibility.Column;
            aparts.IsReadOnly = true;
            aparts.SelectionMode = DataGridSelectionMode.Extended;
            ///
            sql = "SELECT * FROM Dogovor";
            Connect.Table_Fill("Dogovor", sql);
            ///
            sql = "SELECT * FROM Kategoriya";
            Connect.Table_Fill("Kategoriya", sql);
            for (int i = 0; i < Connect.Ds.Tables["Kategoriya"].Rows.Count; i++)
            {
                kategory.Items.Add(Connect.Ds.Tables["Kategoriya"].Rows[i]["Oboznachenie"].ToString());
            }
            ///
            vmestimost.Text = "0";
            /// 
            Confirm.IsEnabled = false;

        }

        private void fioFilter_OnKeyUp(object sender, KeyEventArgs e)
        {
            Connect.Ds.Tables["Klient"].DefaultView.RowFilter = "[ФИО клиента] LIKE '" + fioFilter.Text + "*'";
        }

        private void found(object sender, RoutedEventArgs e)
        {
            string str = "";
            if (kategory.Text != "")
            {
                str = "[Обозначение категории] = '" + kategory.Text + "'";
            }
            if (money.Text != "" && str != "")
            {
                str += " AND [Цена за сутки] <= " + money.Text;
            }
            else if (str == "" && money.Text != "")
            {
                str += "[Цена за сутки] <= " + money.Text;
            }
            if (Convert.ToInt32(vmestimost.Text) > 0 && str != "")
            {
                str += " AND [Количество мест] = " + vmestimost.Text;
            }
            else if (str == "" && Convert.ToInt32(vmestimost.Text) > 0)
            {
                str += "[Количество мест] = " + vmestimost.Text;
            }
            Connect.Ds.Tables["Nomer"].DefaultView.RowFilter = str;
        }

        private void Up_OnClick(object sender, RoutedEventArgs e)
        {
            vmestimost.Text = (Convert.ToInt32(vmestimost.Text) + 1).ToString();
            found(sender, e);
        }

        private void Down_OnClick(object sender, RoutedEventArgs e)
        {
            if (Convert.ToInt32(vmestimost.Text) >= 1)
            {
                vmestimost.Text = (Convert.ToInt32(vmestimost.Text) - 1).ToString();
                found(sender, e);
            }
        }

        private void Klients_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (klients.SelectedIndex >= 0)
            {
                DataRowView klientRow = (DataRowView)klients.SelectedItem;
                k = Convert.ToInt32(klientRow[0]);
                fio.Text = klientRow[1].ToString();
                bonusnayaKarta.Text = klientRow[2].ToString();
                Sum(sender, e);
            }
        }

        private void Aparts_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (aparts.SelectedIndex >= 0)
            {
                DataRowView apartRow = (DataRowView)aparts.SelectedItem;
                n = Convert.ToInt32(apartRow[0]);
                Sum(sender, e);
            }
        }

        private decimal sum = 0;

        private void Sum(object sender, SelectionChangedEventArgs e)
        {
            if (dataVseleniya.SelectedDate != null && dataViseleniya.SelectedDate != null)
            {
                int day = dataViseleniya.SelectedDate.Value.Subtract(dataVseleniya.SelectedDate.Value).Days;
                if (klients.SelectedIndex != -1 && aparts.SelectedIndex != -1 && day > 0)
                {
                    DataRowView apartRow = (DataRowView) aparts.SelectedItem;
                    if (bonusnayaKarta.Text == "платиновая")
                    {
                        sum = Convert.ToDecimal(Convert.ToDouble(apartRow[3]) * day * 0.95);
                    }
                    else if (bonusnayaKarta.Text == "золотая")
                    {
                        sum = Convert.ToDecimal(Convert.ToDouble(apartRow[3]) * day * 0.97);
                    }
                    else if (bonusnayaKarta.Text == "обычная")
                    {
                        sum = Convert.ToDecimal(Convert.ToDouble(apartRow[3]) * day * 0.99);
                    }
                    else if (bonusnayaKarta.Text == "нет")
                    {
                        sum = Convert.ToDecimal(Convert.ToDouble(apartRow[3]) * day);
                    }
                    sumOplati.Text = sum.ToString();
                    Confirm.IsEnabled = true;
                }
            }
        }

        private void Confirm_OnClick(object sender, RoutedEventArgs e)
        {
            int id = 0;
            if (Connect.Ds.Tables["Dogovor"].Compute("MAX(NomerDogovora)", "").ToString() == "")
            {
                id = 1;
            }
            else
            {
                id = 1 + Convert.ToInt32(Connect.Ds.Tables["Dogovor"].Compute("MAX(NomerDogovora)", ""));
            }
            string sql = "INSERT INTO Dogovor VALUES (" + id + ", '" + DateTime.Now.Date + "', " + k + ", " + n + "," +
                         " '" + dataVseleniya.SelectedDate.Value + "', '" + dataViseleniya.SelectedDate.Value + "', " + 
                         sum.ToString().Replace(",", ".") + ", '" + vidOplati.Text + "')";
            Connect.Modification_Execute(sql);
            sql = "UPDATE GostinichniyNomer SET Status = 'занят', IdKlienta = " + k + " WHERE NomerPomesheniya = " + n;
            Connect.Modification_Execute(sql);
            MessageBox.Show("Операция выполнена успешно!");
            NavigationService.Navigate(new vselenie());
        }
    }
}
