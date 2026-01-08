using Assignment.Model.Models;
using Assignment.Repositories.Repository;
using Azure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Assignment.WPF
{
    public partial class AdminManageBooking : Page
    {
        private readonly BookingReservationRepository bookingRepository = BookingReservationRepository.Instance;
        private readonly CustomerRepository customerRepository = CustomerRepository.Instance;

        private MainWindow GetMainWindow() => (MainWindow)Application.Current.MainWindow;

        public AdminManageBooking()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                var reservations = bookingRepository.GetAllReservations();
                Reservation.ItemsSource = reservations;

                cbCustomer.ItemsSource = customerRepository.GetAllCustomers();
                cbCustomer.DisplayMemberPath = nameof(Customer.CustomerFullName);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu đặt phòng: {ex.Message}", "Lỗi DB", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

  
        private void Rooms_OnPreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var reservation = (sender as DataGrid).SelectedItem as BookingReservation;
            if (reservation != null)
            {
     
                if (reservation.BookingStatus == 1)
                {
                    active.IsChecked = true;
                    deactive.IsChecked = false;
                }
                else
                {
                    active.IsChecked = false;
                    deactive.IsChecked = true;
                }


                if (cbCustomer.ItemsSource is IEnumerable<Customer> customers)
                {
                    cbCustomer.SelectedItem = customers.FirstOrDefault(c => c.CustomerId == reservation.CustomerId);
                }
            }
        }


        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            txtPrice.Text = string.Empty;
            txtId.Text = string.Empty;
            dpBooking.SelectedDate = null;
            cbCustomer.SelectedItem = null;
            deactive.IsChecked = false;
            active.IsChecked = false;
            LoadData();
        }


        private void ButtonBase_OnClickAdd(object sender, RoutedEventArgs e)
        {

            if (!int.TryParse(txtId.Text, out int id))
            {
                MessageBox.Show("Vui lòng chọn một Đặt phòng để cập nhật trạng thái.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                var reservationToUpdate = bookingRepository.GetReservationById(id);
                if (reservationToUpdate == null) throw new InvalidOperationException("Không tìm thấy Đặt phòng.");


                reservationToUpdate.BookingStatus = active.IsChecked == true ? (byte)1 : (byte)0;

                bookingRepository.UpdateReservation(reservationToUpdate);

                MessageBox.Show("Cập nhật trạng thái thành công!", "Thành công");
                LoadData();
                ButtonBase_OnClick(null, null); 
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi cập nhật trạng thái: {ex.Message}", "Lỗi DB", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ButtonBase_OnClickDelete(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(txtId.Text, out int id))
            {
                MessageBox.Show("Vui lòng chọn một Đặt phòng để hủy.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var result = MessageBox.Show($"Bạn có muốn HỦY Đặt phòng ID {id} không?", "Xác nhận Hủy", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                
                    bookingRepository.DeleteReservation(id);

                    MessageBox.Show($"Đã hủy Đặt phòng ID {id}!", "Thành công");
                    LoadData();
                    ButtonBase_OnClick(null, null); 
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi hủy: {ex.Message}", "Lỗi DB");
                }
            }
        }

 
        private void Edit(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(txtId.Text, out int id))
            {
                MessageBox.Show("Vui lòng chọn một Đặt phòng để xem chi tiết.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                var reservation = bookingRepository.GetReservationById(id);
                if (reservation != null)
                {
                   
                    Window popupWindow = new Window()
                    {
                        Title = $"Chi tiết Đặt phòng ID: {id}",
                        Content = new BookingDetailPage(reservation), 
                        SizeToContent = SizeToContent.WidthAndHeight,
                        ResizeMode = ResizeMode.NoResize
                    };
                    popupWindow.ShowDialog();

                    LoadData();
                }
                else
                {
                    MessageBox.Show("Booking reservation not found", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi truy xuất chi tiết: {ex.Message}", "Lỗi DB", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}