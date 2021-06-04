using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Hotel_Administration2._0
{
    /// <summary>
    /// Логика взаимодействия для manager.xaml
    /// </summary>
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
