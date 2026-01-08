using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assignment.Model.Models;
using Assignment.Repositories.Repository;
namespace ConsoleApp
{
    public static class MenuService
    {
        public static Customer CurrentCustomer { get; set; }
        public static bool IsAdmin { get; set; } = false;

        public static void ShowMainMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("===== HỆ THỐNG QUẢN LÝ KHÁCH SẠN (CONSOLE) =====");
                Console.WriteLine("------------------------------------------------");
                Console.WriteLine("1. Đăng nhập");
                Console.WriteLine("0. Thoát ứng dụng");
                Console.Write("\nChọn chức năng: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ShowLogin();
                        break;
                    case "0":
                        Console.WriteLine("Đã thoát ứng dụng.");
                        return;
                    default:
                        Console.WriteLine("Lựa chọn không hợp lệ. Nhấn phím bất kỳ để thử lại.");
                        Console.ReadKey();
                        break;
                }
            }
        }

        private static void ShowLogin()
        {
            Console.Clear();
            Console.WriteLine("===== ĐĂNG NHẬP =====");
            Console.Write("Email: ");
            string email = Console.ReadLine()?.Trim() ?? string.Empty;
            Console.Write("Mật khẩu: ");
            string password = Console.ReadLine() ?? string.Empty;

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                Console.WriteLine("Vui lòng nhập đầy đủ Email và Mật khẩu.");
                Console.ReadKey();
                return;
            }

            var config = Program.Configuration;
            if (config == null) return; 

            var adminEmail = config["Admin:Email"];
            var adminPassword = config["Admin:Password"];

            if (email.Equals(adminEmail, StringComparison.OrdinalIgnoreCase) && password.Equals(adminPassword))
            {
                IsAdmin = true;
                CurrentCustomer = null;
                Console.WriteLine("\nĐăng nhập Admin thành công!");
                Console.ReadKey();
                AdminService.ShowAdminDashboard();
                IsAdmin = false; 
                return;
            }

     
            try
            {
                
                Customer customer = CustomerRepository.Instance.GetCustomerByEmailAndPassword(email, password);

                if (customer != null && customer.CustomerStatus == 1) 
                {
                    IsAdmin = false;
                    CurrentCustomer = customer;
                    Console.WriteLine($"\nChào mừng, {customer.CustomerFullName}!");
                    Console.ReadKey();
                    CustomerService.ShowCustomerDashboard();
                    CurrentCustomer = null; 
                }
                else if (customer != null && customer.CustomerStatus == 0)
                {
                    Console.WriteLine("\nTài khoản của bạn đã bị khóa. Vui lòng liên hệ quản trị viên.");
                    Console.ReadKey();
                }
                else
                {
                    Console.WriteLine("\nEmail hoặc Mật khẩu không hợp lệ.");
                    Console.ReadKey();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi đăng nhập: {ex.Message}");
                Console.ReadKey();
            }
        }
    }
}
