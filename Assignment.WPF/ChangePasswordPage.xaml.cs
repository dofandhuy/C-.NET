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
    /// Interaction logic for ChangePasswordPage.xaml
    /// </summary>
    public partial class ChangePasswordPage : Page
    {
        private readonly CustomerRepository customerRepository = CustomerRepository.Instance;
        private readonly int _customerId;

        public ChangePasswordPage(int customerId)
        {
            InitializeComponent();
            _customerId = customerId;
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            string oldPassword = txtOldPass.Password;
            string newPassword = txtNewPass.Password;

            if (string.IsNullOrWhiteSpace(oldPassword) || string.IsNullOrWhiteSpace(newPassword))
            {
                MessageBox.Show("Vui lòng điền đầy đủ Mật khẩu cũ và Mật khẩu mới.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (oldPassword.Equals(newPassword))
            {
                MessageBox.Show("Mật khẩu mới phải khác mật khẩu cũ.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                var customer = customerRepository.GetCustomerById(_customerId);

                if (customer == null)
                {
                    MessageBox.Show("Không tìm thấy thông tin người dùng.", "Lỗi Hệ thống", MessageBoxButton.OK, MessageBoxImage.Error);
                    CloseParentWindow();
                    return;
                }

                if (customer.Password.Equals(oldPassword))
                {
                    customer.Password = newPassword;
                    customerRepository.UpdateCustomer(customer); 

                    MessageBox.Show("Mật khẩu đã được thay đổi thành công!", "Thành công", MessageBoxButton.OK);
                    CloseParentWindow();
                }
                else
                {
                    MessageBox.Show("Mật khẩu cũ không chính xác!", "Lỗi Xác thực", MessageBoxButton.OK, MessageBoxImage.Hand);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi đổi mật khẩu: {ex.Message}", "Lỗi DB", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CloseParentWindow()
        {
            Window parentWindow = Window.GetWindow(this);
            parentWindow?.Close();
        }
    }
}
