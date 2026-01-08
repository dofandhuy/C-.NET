using Assignment.Model.Models;
using Assignment.Repositories.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Assignment.WPF
{
    public partial class BookingDetailPage : Page
    {
        private readonly BookingReservation bookingReservation;

        private readonly BookingDetailRepository detailRepository = BookingDetailRepository.Instance;
        private readonly RoomRepository roomRepository = RoomRepository.Instance;
        private readonly Input input = new Input();
        
        private MainWindow GetMainWindow() => (MainWindow)Application.Current.MainWindow;

        public BookingDetailPage(BookingReservation bookingReservation)
        {
            InitializeComponent();
            this.bookingReservation = bookingReservation;
            
            CheckUserPermission();
            
            LoadData();
        }
        
        private void CheckUserPermission()
        {
            var mainWindow = GetMainWindow();

            if (!mainWindow.IsAdmin)
            {
                AdminControlsPanel.Visibility = Visibility.Collapsed;
                if (Parent is Grid parentGrid)
                {
                    parentGrid.ColumnDefinitions[1].Width = new GridLength(0); 
                }
            }
            else
            {
                AdminControlsPanel.Visibility = Visibility.Visible;
            }
        }

        private void LoadData()
        {
            try
            {
                ContentControl.Content = $"Chi tiết Đặt phòng ID: {bookingReservation.BookingReservationId}";
                var details = detailRepository.GetDetailsByReservationId(bookingReservation.BookingReservationId);
                BookingDetails.ItemsSource = details;
                
                if (GetMainWindow().IsAdmin)
                {
                    txtId.ItemsSource = roomRepository.GetAllRooms();
                    txtId.DisplayMemberPath = nameof(RoomInformation.RoomNumber);
                    ButtonBase_OnClick(null, null); 
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải chi tiết đặt phòng: {ex.Message}", "Lỗi DB", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Rooms_OnPreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (!GetMainWindow().IsAdmin) return;
            
            var detail = (sender as DataGrid).SelectedItem as BookingDetail;
            if (detail != null)
            {
                if (txtId.ItemsSource is IEnumerable<RoomInformation> rooms)
                {
                    txtId.SelectedItem = rooms.FirstOrDefault(r => r.RoomId == detail.RoomId);
                }

                txtPrice.Text = detail.ActualPrice?.ToString("N0"); // Định dạng giá trị
                dpStart.SelectedDate = detail.StartDate.ToDateTime(new TimeOnly(0, 0));
                dpEnd.SelectedDate = detail.EndDate.ToDateTime(new TimeOnly(0, 0));
            }
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            if (!GetMainWindow().IsAdmin) return;
            
            txtId.SelectedItem = null;
            txtPrice.Text = string.Empty;
            dpStart.SelectedDate = null;
            dpEnd.SelectedDate = null;
        }

        private void ButtonBase_OnClickAdd(object sender, RoutedEventArgs e)
        {
            if (!GetMainWindow().IsAdmin) return; 
            
            if (txtId.SelectedItem == null || dpStart.SelectedDate == null || dpEnd.SelectedDate == null)
            {
                MessageBox.Show("Vui lòng điền đầy đủ Room, Start Date, và End Date.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            DateOnly startDate = DateOnly.FromDateTime(dpStart.SelectedDate.GetValueOrDefault());
            DateOnly endDate = DateOnly.FromDateTime(dpEnd.SelectedDate.GetValueOrDefault());
            var room = txtId.SelectedItem as RoomInformation;

            // Validation 
            if (startDate >= endDate)
            {
                MessageBox.Show("Ngày bắt đầu phải trước ngày kết thúc.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (startDate < DateOnly.FromDateTime(DateTime.Now).AddDays(-1)) 
            {
          
                MessageBox.Show("Ngày bắt đầu không thể là ngày trong quá khứ.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!detailRepository.IsRoomAvailable(room.RoomId, startDate, endDate, bookingReservation.BookingReservationId))
            {
                MessageBox.Show("Phòng đã bị đặt trong khoảng thời gian này.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                DateTime startDateTime = startDate.ToDateTime(new TimeOnly(0, 0));
                DateTime endDateTime = endDate.ToDateTime(new TimeOnly(0, 0));
                int totalDays = (endDateTime - startDateTime).Days;
                if (totalDays <= 0) totalDays = 1; 

                decimal actualPrice = totalDays * room.RoomPricePerDay.GetValueOrDefault();

                var bookingDetail = new BookingDetail()
                {
                    StartDate = startDate,
                    RoomId = room.RoomId,
                    EndDate = endDate,
                    ActualPrice = actualPrice,
                    BookingReservationId = bookingReservation.BookingReservationId,
                };

                detailRepository.AddBookingDetail(bookingDetail);

                txtPrice.Text = actualPrice.ToString("N0");
                MessageBox.Show("Thêm chi tiết đặt phòng thành công.", "Thành công");
                LoadData();
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "Lỗi Nghiệp vụ", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thêm chi tiết: {ex.Message}", "Lỗi DB", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Delete(object sender, RoutedEventArgs e)
        {
            if (!GetMainWindow().IsAdmin) return;
    
            var selectedDetail = BookingDetails.SelectedItem as BookingDetail;
            if (selectedDetail == null)
            {
                MessageBox.Show("Vui lòng chọn một chi tiết đặt phòng để xóa.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var result = MessageBox.Show("Bạn có muốn xóa chi tiết đặt phòng này không?", "Xác nhận Xóa", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    detailRepository.DeleteBookingDetail(selectedDetail.BookingReservationId, selectedDetail.RoomId);

                    MessageBox.Show("Xóa chi tiết đặt phòng thành công.", "Thành công");
                    LoadData();
                }
                catch (InvalidOperationException ex)
                {
                    MessageBox.Show(ex.Message, "Lỗi Nghiệp vụ", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi xóa chi tiết: {ex.Message}", "Lỗi DB", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}