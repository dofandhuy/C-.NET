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
    /// Interaction logic for Register.xaml
    /// </summary>
    public partial class Register : Page
    {
        private readonly Input input = new Input();
    

        
        private MainWindow GetMainWindow() => (MainWindow)Application.Current.MainWindow;

        public Register()
        {
            InitializeComponent();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {

            if (string.IsNullOrWhiteSpace(txtEmail.Text) || string.IsNullOrWhiteSpace(txtName.Text) ||
                string.IsNullOrEmpty(txtRetypePass.Password) || string.IsNullOrEmpty(txtPass.Password))
            {
                MessageBox.Show("Vui lòng điền đầy đủ tất cả các trường.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (txtPass.Password != txtRetypePass.Password)
            {
                MessageBox.Show("Mật khẩu không khớp.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!input.isEmailFormatValid(txtEmail.Text))
            {
                MessageBox.Show("Định dạng email không hợp lệ.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }


            if (!input.isValidPhoneNumberFormat(txtTelephone.Text))
            {
                MessageBox.Show("Định dạng số điện thoại không hợp lệ (phải là 10 số, bắt đầu bằng 0).", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }


            if (!input.isDate18YearsAgo(DateOnly.Parse(dpDob.Text)))
            {
                
                return;
            }

            DateOnly? dob = dpDob.SelectedDate.HasValue
                            ? DateOnly.FromDateTime(dpDob.SelectedDate.Value)
                            : (DateOnly?)null;

            var customer = new Customer()
            {
                CustomerFullName = txtName.Text,
                EmailAddress = txtEmail.Text,
                Password = txtPass.Password,
                Telephone = txtTelephone.Text,
                CustomerBirthday = dob,
                CustomerStatus = 1 
            };

            try
            {
                CustomerRepository.Instance.AddCustomer(customer);

                MessageBox.Show("Đăng ký thành công! Bạn có thể đăng nhập.", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                CloseParentWindow();
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "Lỗi Đăng ký", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi hệ thống: {ex.Message}", "Lỗi DB", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CloseParentWindow()
        {
            Window parentWindow = Window.GetWindow(this);
            parentWindow?.Close();
        }
    }
}
