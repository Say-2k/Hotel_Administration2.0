using System.Windows;

namespace Hotel_Administration2._0
{
    public partial class manager : Window
    {
        public manager()
        {
            InitializeComponent();
        }

        private void SignOut_OnClick(object sender, RoutedEventArgs e)
        {
            Hide();
            auth auth = new auth();
            auth.ShowDialog();
            this.Close();
            return;
        }

        private void viselenie(object sender, RoutedEventArgs e)
        {
            frameManager.Navigate(new viselenie());
        }

        private void postklients(object sender, RoutedEventArgs e)
        {
            frameManager.Navigate(new postklients());
        }

        private void migrants(object sender, RoutedEventArgs e)
        {
            frameManager.Navigate(new migrants());
        }
    }
}
