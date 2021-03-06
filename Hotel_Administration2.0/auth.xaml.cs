using System.Windows;

namespace Hotel_Administration2._0
{
    public partial class auth : Window
    {
        public auth()
        {
            InitializeComponent();
            Login.Focus();
        }

        private void auth_Loaded(object sender, RoutedEventArgs e)
        {
            Connect.Table_Fill("Users", "SELECT * FROM Users");
        }

        private void SignIn_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < Connect.Ds.Tables["Users"].Rows.Count; i++)
            {
                if ((Login.Text == Connect.Ds.Tables["Users"].Rows[i][1].ToString()) && (Password.Password == Connect.Ds.Tables["Users"].Rows[i][2].ToString()))
                {
                    if (Connect.Ds.Tables["Users"].Rows[i][3].ToString() == "Администратор")
                    {
                        Hide();
                        admin admin = new admin();
                        admin.ShowDialog();
                        this.Close();
                        return;
                    }
                    else if (Connect.Ds.Tables["Users"].Rows[i][3].ToString() == "Управляющий")
                    {
                        Hide();
                        manager manager = new manager();
                        manager.ShowDialog();
                        this.Close();
                        return;
                    }
                }
            }
            MessageBox.Show("Неверно введены логин или пароль!", "Ошибка");
        }
    }
}
