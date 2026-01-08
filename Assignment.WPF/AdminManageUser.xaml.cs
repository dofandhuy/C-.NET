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
    /// Interaction logic for AdminManageUser.xaml
    /// </summary>
    public partial class AdminManageUser : Page
    {
        private MainWindow GetMainWindow() => (MainWindow)Application.Current.MainWindow;
        public AdminManageUser()
        {
            InitializeComponent();
            LoadCustomers();
        }

        private void LoadCustomers()
        {
            try
            {
                var list = CustomerRepository.Instance.GetAllCustomers();
                Customers.ItemsSource = list;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách khách hàng: {ex.Message}", "Lỗi DB", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void Customers_OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var customer = (sender as DataGrid).SelectedItem as Customer;
            if (customer != null)
            {
                txtId.Text = customer.CustomerId.ToString();

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
        }


        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            txtEmail.Text = string.Empty;
            txtId.Text = string.Empty;
            txtName.Text = string.Empty;
            txtTelephone.Text = string.Empty;
            dpDob.Text = string.Empty;
            deactive.IsChecked = false;
            active.IsChecked = false;
            LoadCustomers();
        }

        private void ButtonBase_OnClickAdd(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtEmail.Text) || string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Email và Tên không được để trống.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                int customerId = 0;
                if (!string.IsNullOrWhiteSpace(txtId.Text) && int.TryParse(txtId.Text, out customerId))
                {

                    var existingCustomer = CustomerRepository.Instance.GetCustomerById(customerId);

                    if (existingCustomer != null)
                    {
                        var result = MessageBox.Show($"Khách hàng ID {customerId} đã tồn tại. Bạn muốn Cập nhật?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question);

                        if (result == MessageBoxResult.Yes)
                        {
                            existingCustomer.CustomerFullName = txtName.Text;
                            existingCustomer.Telephone = txtTelephone.Text;
                            existingCustomer.CustomerBirthday = DateOnly.Parse(dpDob.Text);
                            existingCustomer.CustomerStatus = active.IsChecked == true ? (byte)1 : (byte)0;
                            CustomerRepository.Instance.UpdateCustomer(existingCustomer);
                            MessageBox.Show("Cập nhật thành công!", "Thành công");
                        }
                    }
                    else
                    {
                        PerformAddOperation();
                    }
                }
                else
                {
                    PerformAddOperation();
                }

                LoadCustomers();
                ButtonBase_OnClick(null, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thêm/cập nhật: {ex.Message}", "Lỗi DB", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void PerformAddOperation()
        {
            if (CustomerRepository.Instance.GetCustomerByEmail(txtEmail.Text) != null)
            {
                MessageBox.Show("Email đã tồn tại. Vui lòng sử dụng email khác!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var customer = new Customer()
            {
                CustomerFullName = txtName.Text,
                Telephone = txtTelephone.Text,
                EmailAddress = txtEmail.Text,
                CustomerBirthday = DateOnly.Parse(dpDob.Text),
                CustomerStatus = active.IsChecked == true ? (byte)1 : (byte)0,
            };

            CustomerRepository.Instance.AddCustomer(customer);
            MessageBox.Show("Thêm khách hàng thành công!", "Thành công");
        }

        private void ButtonBase_OnClickDel(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtId.Text) && int.TryParse(txtId.Text, out int id))
            {
                try
                {
                    CustomerRepository.Instance.ChangeCustomerStatus(id, 0);
                    MessageBox.Show($"Đã khóa tài khoản khách hàng ID {id}!", "Thành công");
                    LoadCustomers();
                }
                catch (InvalidOperationException ex)
                {
                    MessageBox.Show(ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi xóa: {ex.Message}", "Lỗi DB");
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn hoặc nhập ID khách hàng!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}
