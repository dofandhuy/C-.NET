using Assignment.Model.Models;
using Assignment.Repositories.Repository;
using Azure;
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
    /// Interaction logic for AdminDashBoard.xaml
    /// </summary>
    public partial class AdminDashBoard : Page
    {
        private MainWindow GetMainWindow() => (MainWindow)Application.Current.MainWindow;

        public AdminDashBoard()
        {
            InitializeComponent();
            ManageCustomer_Click(MenuCustomer, null);
        }


        private void HighlightMenuItem(MenuItem clickedItem)
        {
            MenuCustomer.FontWeight = FontWeights.Normal;
            MenuRoom.FontWeight = FontWeights.Normal;
            MenuBooking.FontWeight = FontWeights.Normal;
            MenuReport.FontWeight = FontWeights.Normal;

            if (clickedItem != null)
            {
                clickedItem.FontWeight = FontWeights.Bold;
            }
        }

        private void ManageCustomer_Click(object sender, RoutedEventArgs e)
        {
            HighlightMenuItem(MenuCustomer);

            ContentFrame.Navigate(new AdminManageUser()); 
        }

        private void ManageRoom_Click(object sender, RoutedEventArgs e)
        {
            HighlightMenuItem(MenuRoom);
           ContentFrame.Navigate(new AdminManageRoom()); 
        }

        private void ManageBooking_Click(object sender, RoutedEventArgs e)
        {
            HighlightMenuItem(MenuBooking);
            ContentFrame.Navigate(new AdminManageBooking()); 
        }

        private void ManageReport_Click(object sender, RoutedEventArgs e)
        {
            HighlightMenuItem(MenuReport);
            ContentFrame.Navigate(new AdminReportPage()); 
        }

        private void MenuItem_OnClickLogout(object sender, RoutedEventArgs e)
        {
            GetMainWindow().IsAdmin = false;
            GetMainWindow().MainFrame.Navigate(new Login());
        }

    }
}
