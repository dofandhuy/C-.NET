using Assignment.Model.Models;
using Assignment.Repositories.Repository;
using Azure;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Text;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public IConfigurationRoot Configuration { get; private set; }
        public Customer CurrentCustomer { get; set; }
        public bool IsAdmin { get; set; } = false;

        public MainWindow()
        {
  
            LoadAppSettings();
            InitializeRepositories();
            InitializeComponent();
            MainFrame.Navigate(new Login()); 
        }

        private void LoadAppSettings()
        {
   
            Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsetting.json", optional: false, reloadOnChange: true)
                .Build();
        }

        private void InitializeRepositories()
        {
            string connectionString = Configuration.GetConnectionString("FUMiniHotelDB");

            if (string.IsNullOrEmpty(connectionString))
            {
                MessageBox.Show("Lỗi: Không tìm thấy Connection String.", "Cấu hình Lỗi");
                Application.Current.Shutdown();
                return;
            }

            CustomerRepository.Initialize(connectionString);
            RoomRepository.Initialize(connectionString);
            BookingReservationRepository.Initialize(connectionString);
            BookingDetailRepository.Initialize(connectionString);
        }
    }
}