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
    /// Interaction logic for AdminManageRoom.xaml
    /// </summary>
    public partial class AdminManageRoom : Page
    {
        private MainWindow GetMainWindow() => (MainWindow)Application.Current.MainWindow;

        private readonly RoomRepository roomRepository = RoomRepository.Instance;
       
        public AdminManageRoom()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                Rooms.ItemsSource = roomRepository.GetAllRooms();
                cbRoomType.ItemsSource = roomRepository.GetAllRoomType();
                cbRoomType.DisplayMemberPath = nameof(RoomType.RoomTypeName);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu: {ex.Message}", "Lỗi DB", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Rooms_OnPreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var room = (sender as DataGrid).SelectedItem as RoomInformation;
            if (room != null)
            {
                if (room.RoomStatus == 1)
                {
                    active.IsChecked = true;
                    deactive.IsChecked = false;
                }
                else
                {
                    active.IsChecked = false;
                    deactive.IsChecked = true;
                }

                txtRoomType.Text = room.RoomType?.TypeDescription;
                if (cbRoomType.ItemsSource is IEnumerable<RoomType> roomTypes)
                {
                    cbRoomType.SelectedItem = roomTypes.FirstOrDefault(rt => rt.RoomTypeId == room.RoomTypeId);
                }
            }
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            txtId.Text = string.Empty;
            txtPrice.Text = string.Empty;
            txtNumber.Text = string.Empty;
            txtDescription.Text = string.Empty;
            txtMaxCapacity.Text = string.Empty;
            txtRoomType.Text = string.Empty;
            deactive.IsChecked = false;
            active.IsChecked = false;
            cbRoomType.SelectedValue = null;
            LoadData(); 
        }

        private void CbRoomType_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbRoomType.SelectedItem is RoomType selectedItem)
            {
                txtRoomType.Text = selectedItem.TypeDescription;
            }
            else
            {
                txtRoomType.Text = string.Empty;
            }
        }

        private void ButtonBase_OnClickAdd(object sender, RoutedEventArgs e)
        {
            if (!ValidateInput()) return;
            decimal price = decimal.Parse(txtPrice.Text);
            int maxCapacity = int.Parse(txtMaxCapacity.Text);
            var roomType = cbRoomType.SelectedItem as RoomType;

            try
            {
                var newRoomData = new RoomInformation
                {
                    RoomNumber = txtNumber.Text,
                    RoomDetailDescription = txtDescription.Text,
                    RoomMaxCapacity = maxCapacity,
                    RoomTypeId = roomType.RoomTypeId,
                    RoomStatus = active.IsChecked == true ? (byte)1 : (byte)0,
                    RoomPricePerDay = price
                };

                int id = 0;
                bool isUpdate = !string.IsNullOrWhiteSpace(txtId.Text) && int.TryParse(txtId.Text, out id);

                if (isUpdate)
                {
                    newRoomData.RoomId = id;
                    if (roomRepository.GetAllRooms().Any(r => r.RoomNumber == newRoomData.RoomNumber && r.RoomId != newRoomData.RoomId))
                    {
                        MessageBox.Show("Số phòng đã tồn tại cho phòng khác.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    var result = MessageBox.Show("Bạn có muốn cập nhật phòng này không?", "Cập nhật Phòng", MessageBoxButton.YesNo);
                    if (result == MessageBoxResult.Yes)
                    {
                        roomRepository.UpdateRoom(newRoomData);
                        MessageBox.Show("Cập nhật phòng thành công!", "Thành công");
                    }
                }
                else
                {
                    if (roomRepository.GetAllRooms().Any(r => r.RoomNumber == newRoomData.RoomNumber))
                    {
                        MessageBox.Show("Số phòng đã tồn tại.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    roomRepository.AddRoom(newRoomData);
                    MessageBox.Show("Thêm phòng thành công!", "Thành công");
                }

                LoadData();
                ButtonBase_OnClick(null, null); 
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "Lỗi Nghiệp vụ", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi thao tác DB: {ex.Message}", "Lỗi Hệ thống", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ButtonBase_OnClickDelete(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtId.Text) && int.TryParse(txtId.Text, out int id))
            {
                var result = MessageBox.Show($"Bạn có muốn khóa/xóa mềm phòng ID {id} không?", "Xóa Phòng", MessageBoxButton.YesNo);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        roomRepository.ChangeRoomStatus(id, 0);
                        MessageBox.Show($"Đã khóa phòng ID {id}!", "Thành công");
                        LoadData();
                        ButtonBase_OnClick(null, null); 
                    }
                    catch (InvalidOperationException ex)
                    {
                        MessageBox.Show(ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Lỗi khi xóa: {ex.Message}", "Lỗi DB");
                    }
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn hoặc nhập ID phòng để xóa.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtPrice.Text) || string.IsNullOrWhiteSpace(txtNumber.Text) ||
                string.IsNullOrWhiteSpace(txtMaxCapacity.Text) || cbRoomType.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng điền đầy đủ tất cả các trường bắt buộc.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (!int.TryParse(txtMaxCapacity.Text, out _) || int.Parse(txtMaxCapacity.Text) <= 0)
            {
                MessageBox.Show("Sức chứa tối đa phải là số nguyên dương hợp lệ.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (!decimal.TryParse(txtPrice.Text, out _) || decimal.Parse(txtPrice.Text) <= 0)
            {
                MessageBox.Show("Giá mỗi ngày phải là số dương hợp lệ.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return true;
        }

      

    }
}
