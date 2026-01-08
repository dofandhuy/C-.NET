
using Assignment.Repositories.Repository;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Text;

namespace ConsoleApp
{
    internal class Program
    {
        public static IConfigurationRoot Configuration { get; private set; }

        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.Title = "FUMini Hotel Management Console";

 

            LoadAppSettings();

            if (InitializeRepositories())
            {
                Console.WriteLine("Hệ thống Repository đã khởi tạo thành công.");
                MenuService.ShowMainMenu();
            }
            else
            {
                Console.WriteLine("Lỗi nghiêm trọng: Không thể khởi tạo hệ thống Repository. Ứng dụng sẽ thoát.");
                Console.ReadKey();
            }
        }

        private static void LoadAppSettings()
        {
            try
            {
                Configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsetting.json", optional: false, reloadOnChange: true)
                    .Build();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi tải cấu hình (appsetting.json): {ex.Message}");
                Configuration = null;
            }
        }

        private static bool InitializeRepositories()
        {
            if (Configuration == null) return false;

            try
            {
                string connectionString = Configuration.GetConnectionString("FUMiniHotelDB");

                if (string.IsNullOrEmpty(connectionString))
                {
                    Console.WriteLine("Lỗi: Không tìm thấy Connection String 'FUMiniHotelDB' trong cấu hình.");
                    return false;
                }

                CustomerRepository.Initialize(connectionString);
                RoomRepository.Initialize(connectionString);
                BookingReservationRepository.Initialize(connectionString);
                BookingDetailRepository.Initialize(connectionString);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khởi tạo Repository (Có thể do Connection String sai): {ex.Message}");
                return false;
            }
        }
    }
}