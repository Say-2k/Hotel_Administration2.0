using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Hotel_Administration2._0
{
    public partial class reg : Page
    {
        public reg()
        {
            InitializeComponent();
        }

        private void Confirm_OnClick(object sender, RoutedEventArgs e)
        {
            if (Fam.Text != "" && Imya.Text != "" && Otch.Text != "" && Pol.Text != "" && BonusnayaKarta.Text != "" && Pasport_serial.Text != "" && Pasport_number.Text != "")
            {
                int id = Convert.ToInt32(Connect.Ds.Tables["Klient"].Compute("MAX(IdKlienta)", "")) + 1;
                string sql = "INSERT INTO Klient VALUES (" + id + ", '" + Fam.Text + "', '" + Imya.Text + "', '" + Otch.Text + "', '" + Pol.Text + "', '" + DataRojdeniya.Text + "', '" + Pasport_vid.Text + "')";
                Connect.Modification_Execute(sql);
                sql = "INSERT INTO Pasport VALUES (" + id + ", '" + Pasport_serial.Text + "', '" + Pasport_number.Text + "', '" + Pasport_vid.Text + "', '" + Pasport_vidan.Text + "', '" + Pasport_strana_vidachi.Text + "')";
                Connect.Modification_Execute(sql);

                if (Pasport_vid.Text == "Иностранный паспорт" && Migration_number.Text != "" && Migration_from.Text != "" && Cel_vizita.Text != "")
                {
                    sql = "INSERT INTO MigracionnayaKarta VALUES (" + id + ", '" + Migration_number.Text + "', '" + Migration_from.Text + "', '" + DataPriezda.Text + "', '" + DataViezda.Text + "', '" + Cel_vizita.Text + "')";
                    Connect.Modification_Execute(sql);
                }
                else
                {
                    sql = "INSERT INTO Address VALUES (" + id + ", '" + Pasport_city.Text + "', '" + Pasport_street.Text +
                          "', '" + Pasport_house.Text + "', '" + Pasport_apart.Text + "')";
                    Connect.Modification_Execute(sql);
                }
                MessageBox.Show("Клиент добавлен");
                NavigationService.Navigate(new reg());
            }
            else
            {
                MessageBox.Show("Обязательное заполнение всех указанных полей", "Ошибка");
            }
            NavigationService.Navigate(new reg());
        }

        private void Reg_OnLoaded(object sender, RoutedEventArgs e)
        {
            string sql = "SELECT IdKlienta FROM Klient";
            Connect.Table_Fill("Klient", sql);
            ///
            MigrantKart.IsEnabled = false;
            Pasport_strana_vidachi.IsEnabled = false;
            Pasport_vid.Text = "Паспорт РФ";
        }

        private void RF_OnSelected(object sender, RoutedEventArgs e)
        {
            Pasport_strana_vidachi.IsEnabled = false;
            MigrantKart.IsEnabled = false;
            Pasport_city.IsReadOnly = false;
            Pasport_street.IsReadOnly = false;
            Pasport_house.IsReadOnly = false;
            Pasport_apart.IsReadOnly = false;
        }

        private void Inostranniy_OnSelected(object sender, RoutedEventArgs e)
        {
            Pasport_strana_vidachi.IsEnabled = true;
            MigrantKart.IsEnabled = true;
            Pasport_city.IsReadOnly = true;
            Pasport_street.IsReadOnly = true;
            Pasport_house.IsReadOnly = true;
            Pasport_apart.IsReadOnly = true;
        }
    }
}
