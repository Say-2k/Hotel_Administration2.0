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
    /// Логика взаимодействия для admin.xaml
    /// </summary>
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
