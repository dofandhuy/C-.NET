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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Assignment.WPF
{
    /// <summary>
    /// Interaction logic for CustomerDashBoard.xaml
    /// </summary>
    public partial class CustomerDashBoard : Page
    {
        private MainWindow GetMainWindow() => (MainWindow)Application.Current.MainWindow;
        public CustomerDashBoard()
        {
            InitializeComponent();
            var mainWindow = GetMainWindow();

            if (mainWindow.CurrentCustomer == null && !mainWindow.IsAdmin)
            {
                MessageBox.Show("Truy cập bị từ chối. Vui lòng đăng nhập lại.", "Lỗi Phân Quyền", MessageBoxButton.OK, MessageBoxImage.Stop);
                mainWindow.MainFrame.Navigate(new Login());
                return;
            }
            ViewBooking_Click(ViewBooking, null);
        }


        private void HighlightMenuItem(MenuItem clickedItem)
        {
            ViewBooking.FontWeight = FontWeights.Normal;
            ViewBooking.Foreground = Brushes.Black;
            Profile.FontWeight = FontWeights.Normal;
            Profile.Foreground = Brushes.Black;

            if (clickedItem != null)
            {
                clickedItem.FontWeight = FontWeights.Bold;
                clickedItem.Foreground = (Brush)new BrushConverter().ConvertFromString("#007ACC");
            }
        }

        private void MenuItem_OnClickLogout(object sender, RoutedEventArgs e)
        {
            GetMainWindow().IsAdmin = false;
            GetMainWindow().MainFrame.Navigate(new Login());
        }

        private void ViewBooking_Click(object sender, RoutedEventArgs e)
        {
            var menuItem = sender as MenuItem;
            HighlightMenuItem(menuItem);

          ContentFrame.Navigate(new CustomerBookingHistoryPage());
        }

        private void Profile_Click(object sender, RoutedEventArgs e)
        {
            var menuItem = sender as MenuItem;
            HighlightMenuItem(menuItem);

            ContentFrame.Navigate(new CustomerProfilePage());
        }
    }
}
