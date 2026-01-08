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
    /// Interaction logic for CustomerProfilePage.xaml
    /// </summary>
    public partial class CustomerProfilePage : Page
    {
        private readonly CustomerRepository customerRepository = CustomerRepository.Instance;
        private readonly Input input = new Input(); 

        private MainWindow GetMainWindow() => (MainWindow)Application.Current.MainWindow;

        public CustomerProfilePage()
        {
            InitializeComponent();
            Load();
        }
        private void Load()
        {
            var customer = customerRepository.GetCustomerById(GetMainWindow().CurrentCustomer.CustomerId);

            if (customer == null)
            {
                MessageBox.Show("Không thể tải hồ sơ khách hàng.", "Lỗi DB", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            txtId.Text = customer.CustomerId.ToString();
            txtName.Text = customer.CustomerFullName;
            txtTelephone.Text = customer.Telephone;
            txtEmail.Text = customer.EmailAddress;

            dpDob.SelectedDate = customer.CustomerBirthday?.ToDateTime(new TimeOnly(0, 0));

            if (customer.CustomerStatus == 1)
            {
                active.IsChecked = true;
                deactive.IsChecked = false;
            }
            else
            {
                active.IsChecked = false;
                deactive.IsChecked = true;
            }
        }

        private void ButtonBase_OnClickAdd(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text) || string.IsNullOrWhiteSpace(txtTelephone.Text) || dpDob.SelectedDate == null)
            {
                MessageBox.Show("Vui lòng điền đầy đủ Tên, Điện thoại và Ngày sinh.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!input.isValidPhoneNumberFormat(txtTelephone.Text))
            {
                MessageBox.Show("Số điện thoại không hợp lệ.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            DateOnly dob = DateOnly.FromDateTime(dpDob.SelectedDate.Value);
            if (dob >= DateOnly.FromDateTime(DateTime.Now))
            {
                MessageBox.Show("Ngày sinh phải là ngày trong quá khứ.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var updatedCustomer = customerRepository.GetCustomerById(int.Parse(txtId.Text));
            if (updatedCustomer == null) return;

            updatedCustomer.CustomerFullName = txtName.Text;
            updatedCustomer.Telephone = txtTelephone.Text;
            updatedCustomer.CustomerBirthday = dob;

            var result = MessageBox.Show("Bạn có chắc chắn muốn lưu các thay đổi này không?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    customerRepository.UpdateCustomer(updatedCustomer);
                    MessageBox.Show("Cập nhật thông tin hồ sơ thành công.", "Thành công");

                    GetMainWindow().CurrentCustomer = updatedCustomer;

                    Load();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi lưu hồ sơ: {ex.Message}", "Lỗi DB", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            Load();
        }

        private void ButtonBase_OnClickPass(object sender, RoutedEventArgs e)
        {
            Window popupWindow = new Window()
            {
                Title = "Đổi Mật khẩu",
                Content = new ChangePasswordPage(int.Parse(txtId.Text)),
                SizeToContent = SizeToContent.WidthAndHeight,
                ResizeMode = ResizeMode.NoResize
            };
            popupWindow.ShowDialog(); 

            
        }
    }
}
