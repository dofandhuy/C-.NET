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
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Page
    {
        private MainWindow GetMainWindow() => (MainWindow)Application.Current.MainWindow;
        public Login()
        {
            InitializeComponent();
            GetMainWindow().IsAdmin = false;
        }
        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var mainWindow = GetMainWindow();
            string email = this.email.Text;
            string password = this.password.Password;

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Vui lòng nhập cả email và mật khẩu.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var adminEmail = mainWindow.Configuration["Admin:Email"];
            var adminPassword = mainWindow.Configuration["Admin:Password"];

            if (email == adminEmail && password == adminPassword)
            {
                mainWindow.IsAdmin = true;
                NavigationService.Navigate(new AdminDashBoard());
                return;
            }


            Customer customer = CustomerRepository.Instance.GetCustomerByEmailAndPassword(email, password);

            if (customer != null)
            {
                mainWindow.IsAdmin = false;
                mainWindow.CurrentCustomer = customer;
                NavigationService.Navigate(new CustomerDashBoard());
            }
            else
            {
                MessageBox.Show("Email hoặc Mật khẩu không hợp lệ, hoặc tài khoản đã bị khóa.", "Lỗi Đăng nhập", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ButtonBase_OnClickRegister(object sender, RoutedEventArgs e)
        {
            
            Window popupWindow = new Window()
            {
                Title = "Đăng ký Khách hàng",
                Content = new Register(),
                SizeToContent = SizeToContent.WidthAndHeight,
                ResizeMode = ResizeMode.NoResize 
            };
            popupWindow.ShowDialog(); 
        }

 
    }
}
