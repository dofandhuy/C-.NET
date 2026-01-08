using Assignment.Model.Models;
using Assignment.Repositories.Repository;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;

namespace Assignment.WPF
{
    public partial class AdminReportPage : Page
    {

        private readonly BookingDetailRepository detailRepository;
        private readonly BookingReservationRepository reservationRepository;

        private MainWindow GetMainWindow() => (MainWindow)Application.Current.MainWindow;

        public AdminReportPage()
        {
            InitializeComponent();
            try
            {
                detailRepository = BookingDetailRepository.Instance;
                reservationRepository = BookingReservationRepository.Instance;
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show($"Lỗi Khởi tạo DB: {ex.Message}\nVui lòng kiểm tra lại MainWindow.Initialize(ConnectionString).", "Lỗi Hệ thống", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            LoadData();
        }

        private void LoadData()
        {
            try
            {
                if (detailRepository != null)
                {
                    Reservation.ItemsSource = detailRepository.GetAllBookingDetails();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu báo cáo: {ex.Message}", "Lỗi DB", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DpStart_OnCalendarClosed(object sender, RoutedEventArgs e)
        {
            var startDate = dpStart.SelectedDate;
            var endDate = dpEnd.SelectedDate;

            if (startDate != null && endDate != null)
            {
                if (startDate > endDate)
                {
                    MessageBox.Show("Ngày bắt đầu không được lớn hơn ngày kết thúc.", "Lỗi Lọc", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                try
                {
                    DateOnly start = DateOnly.FromDateTime(startDate.GetValueOrDefault());
                    DateOnly end = DateOnly.FromDateTime(endDate.GetValueOrDefault());

                    var filteredList = detailRepository.GetBookingDetailsByDateRange(start, end);

                    Reservation.ItemsSource = filteredList;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi lọc dữ liệu: {ex.Message}", "Lỗi DB", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void ButtonBase_OnClickReset(object sender, RoutedEventArgs e)
        {
            dpEnd.SelectedDate = null;
            dpStart.SelectedDate = null;
            LoadData();
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            var hyperlink = sender as Hyperlink;
            if (hyperlink == null) return;

            var dataItem = hyperlink.DataContext as BookingDetail;
            if (dataItem == null) return;

            if (reservationRepository == null) return;

            try
            {
                var reservation = reservationRepository.GetReservationById(dataItem.BookingReservationId);

                if (reservation == null)
                {
                    MessageBox.Show("Không tìm thấy Đặt phòng tương ứng.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                Window popupWindow = new Window()
                {
                    Title = $"Chi tiết Đặt phòng ID: {reservation.BookingReservationId}",
                    Content = new BookingDetailPage(reservation),
                    SizeToContent = SizeToContent.WidthAndHeight,
                    ResizeMode = ResizeMode.NoResize
                };
                popupWindow.Show();

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi truy cập chi tiết: {ex.Message}", "Lỗi DB", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            e.Handled = true;
        }


        private void Reservation_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            SaveDataGridToCsvDialog(Reservation);
        }


        public static void SaveDataGridToCsv(DataGrid dataGrid, string filePath)
        {
            var sb = new StringBuilder();

            var headers = dataGrid.Columns
                .Select(column => column.Header.ToString())
                .ToArray();
            sb.AppendLine(string.Join(",", headers));

            foreach (var item in dataGrid.Items)
            {
                var cells = dataGrid.Columns.Select(column =>
                {
                    var binding = (column as DataGridBoundColumn)?.Binding as System.Windows.Data.Binding;

                    var value = binding?.Path.Path.Split('.').Aggregate((object)item, (obj, property) =>
                    {
                        if (obj == null) return null;

                        PropertyInfo propInfo = obj.GetType().GetProperty(property);
                        return propInfo?.GetValue(obj, null);
                    });

                    return value?.ToString()?.Replace(",", ".");
                }).ToArray();

                sb.AppendLine(string.Join(",", cells));
            }
            File.WriteAllText(filePath, sb.ToString(), Encoding.UTF8);
        }

        public static void SaveDataGridToCsvDialog(DataGrid dataGrid)
        {
            var dialog = new SaveFileDialog
            {
                Filter = "CSV files (*.csv)|*.csv",
                DefaultExt = ".csv",
                FileName = $"BookingReport_{DateTime.Now:yyyyMMdd_HHmmss}"
            };

            if (dialog.ShowDialog() == true)
            {
                try
                {
                    SaveDataGridToCsv(dataGrid, dialog.FileName);
                    MessageBox.Show($"Đã xuất báo cáo thành công ra file: {dialog.FileName}", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (IOException ex)
                {
                    MessageBox.Show($"Lỗi ghi file: {ex.Message}", "Lỗi Xuất CSV", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}