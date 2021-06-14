using System.Windows;
using System.Windows.Controls;

namespace Hotel_Administration2._0
{
    public partial class managePaswd : Page
    {
        public managePaswd()
        {
            InitializeComponent();
        }

        private void manageLoad(object sender, RoutedEventArgs e)
        {
            if (Login.Items.Count == 0)
            {
                for (int i = 0; i < Connect.Ds.Tables["Users"].Rows.Count; i++)
                {
                    Login.Items.Add(Connect.Ds.Tables["Users"].Rows[i][1].ToString());
                }
            }
        }

        private void manage(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < Connect.Ds.Tables["Users"].Rows.Count; i++)
            {
                if (Connect.Ds.Tables["Users"].Rows[i][1].ToString() == Login.Text)
                {
                    string sql = "UPDATE Users SET Password = '" + Password.Password + "' WHERE Login = '" +
                                 Login.Text + "'";
                    Connect.Modification_Execute(sql);
                    MessageBox.Show("Изменение пароля прошло успешно");
                    return;
                }
            }
            MessageBox.Show("Ошибка ввода логина");
        }
    }
}
