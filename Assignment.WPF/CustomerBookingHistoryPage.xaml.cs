using Assignment.Model.Models;
using Assignment.Repositories.Repository;
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
    /// Interaction logic for CustomerBookingHistoryPage.xaml
    /// </summary>
    public partial class CustomerBookingHistoryPage : Page
    {
        private readonly BookingReservationRepository reservationRepository = BookingReservationRepository.Instance;

        private MainWindow GetMainWindow() => (MainWindow)Application.Current.MainWindow;

        public CustomerBookingHistoryPage()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            var customer = GetMainWindow().CurrentCustomer;

            if (customer == null)
            {
                MessageBox.Show("Không tìm thấy thông tin khách hàng.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                var list = reservationRepository.GetReservationsByCustomerId(customer.CustomerId);
                Reservation.ItemsSource = list;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải lịch sử đặt phòng: {ex.Message}", "Lỗi DB", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

     
        private void Reservation_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var dataGrid = sender as DataGrid;
            var reservation = dataGrid?.SelectedItem as BookingReservation;

            if (reservation != null)
            {
                Window popupWindow = new Window()
                {
                    Title = $"Chi tiết Đặt phòng ID: {reservation.BookingReservationId}",
                    Content = new BookingDetailPage(reservation),
                    SizeToContent = SizeToContent.WidthAndHeight,
                    ResizeMode = ResizeMode.NoResize
                };
                popupWindow.Show();

                popupWindow.Closed += (s, args) => LoadData();
            }
        }
    }
    
}

