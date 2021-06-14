using System.Windows;

namespace Hotel_Administration2._0
{
    public partial class admin : Window
    {
        public admin()
        {
            InitializeComponent();
        }

        private void reg(object sender, RoutedEventArgs e)
        {
            FrameAdmin.Navigate(new reg()); ;
        }

        private void vselenie(object sender, RoutedEventArgs e)
        {
            FrameAdmin.Navigate(new vselenie());
        }

        private void managePaswd(object sender, RoutedEventArgs e)
        {
            FrameAdmin.Navigate(new managePaswd());
        }

        private void SignOut_OnClick(object sender, RoutedEventArgs e)
        {
            Hide();
            auth auth = new auth();
            auth.ShowDialog();
            this.Close();
            return;
        }
    }
}
