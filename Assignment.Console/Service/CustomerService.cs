using Assignment.Repositories.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    public static class CustomerService
    {
        private static readonly BookingReservationRepository reservationRepository = BookingReservationRepository.Instance;
        private static readonly CustomerRepository customerRepository = CustomerRepository.Instance;

        private static readonly Input input = new Input();

        public static void ShowCustomerDashboard()
        {
            while (MenuService.CurrentCustomer != null)
            {
                Console.Clear();
                Console.WriteLine($"===== KHÁCH HÀNG: {MenuService.CurrentCustomer.CustomerFullName.ToUpper()} =====");
                Console.WriteLine("---------------------------------------------");
                Console.WriteLine("1. Xem Lịch sử Đặt phòng");
                Console.WriteLine("2. Quản lý Hồ sơ Cá nhân");
                Console.WriteLine("0. Đăng xuất");
                Console.Write("\nChọn chức năng: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ViewBookingHistory();
                        break;
                    case "2":
                        ManageProfile();
                        break;
                    case "0":
                        Console.WriteLine("Đang đăng xuất...");
                        return; // Quay lại Main Menu
                    default:
                        Console.WriteLine("Lựa chọn không hợp lệ.");
                        Console.ReadKey();
                        break;
                }
            }
        }

        private static void ViewBookingHistory()
        {
            Console.Clear();
            Console.WriteLine("===== LỊCH SỬ ĐẶT PHÒNG =====");
            try
            {
                var customerId = MenuService.CurrentCustomer.CustomerId;
                var reservations = reservationRepository.GetReservationsByCustomerId(customerId);

                if (!reservations.Any())
                {
                    Console.WriteLine("Bạn chưa có đơn đặt phòng nào.");
                }
                else
                {
                    Console.WriteLine(new string('=', 70));
                    Console.WriteLine($"{"ID",-5}{"Ngày Đặt",-15}{"Tổng Giá",-20}{"Trạng Thái",-15}");
                    Console.WriteLine(new string('-', 70));

                    foreach (var r in reservations)
                    {
                        string status = r.BookingStatus == 1 ? "Active" : "Cancelled/Finished";
                        string price = r.TotalPrice.HasValue ? $"{r.TotalPrice.Value:N0} VNĐ" : "N/A";

                        Console.WriteLine($"{r.BookingReservationId,-5}{r.BookingDate:dd/MM/yyyy,-15}{price,-20}{status,-15}");
                    }
                    Console.WriteLine(new string('=', 70));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi tải dữ liệu: {ex.Message}");
            }

            Console.WriteLine("\nNhấn phím bất kỳ để tiếp tục...");
            Console.ReadKey();
        }

        private static void ManageProfile()
        {
            Console.Clear();
            Console.WriteLine("===== QUẢN LÝ HỒ SƠ CÁ NHÂN =====");

            var customer = customerRepository.GetCustomerById(MenuService.CurrentCustomer.CustomerId);
            if (customer == null)
            {
                Console.WriteLine("Lỗi: Không tìm thấy hồ sơ của bạn.");
                Console.ReadKey();
                return;
            }

            Console.WriteLine($"ID: {customer.CustomerId}");
            Console.WriteLine($"Email: {customer.EmailAddress} (Không thể thay đổi)");
            Console.WriteLine($"Tên hiện tại: {customer.CustomerFullName}");
            Console.WriteLine($"SĐT hiện tại: {customer.Telephone}");
            Console.WriteLine($"Ngày sinh hiện tại: {customer.CustomerBirthday:dd/MM/yyyy}");
            Console.WriteLine($"Trạng thái: {(customer.CustomerStatus == 1 ? "Active" : "Deactive")}");
            Console.WriteLine(new string('-', 30));

            Console.WriteLine("\nNhập thông tin mới (Để trống nếu không muốn thay đổi):");

            Console.Write("Họ và Tên mới: ");
            string newName = Console.ReadLine()?.Trim();

            Console.Write("Số điện thoại mới: ");
            string newPhone = Console.ReadLine()?.Trim();

            Console.Write("Ngày sinh mới (dd/MM/yyyy): ");
            string newDobStr = Console.ReadLine()?.Trim();

            try
            {
                if (!string.IsNullOrEmpty(newName))
                {
                    customer.CustomerFullName = newName;
                }

                if (!string.IsNullOrEmpty(newPhone))
                {
                    if (!input.isValidPhoneNumberFormat(newPhone))
                    {
                        Console.WriteLine("Lỗi: Số điện thoại không hợp lệ.");
                        Console.ReadKey();
                        return;
                    }
                    customer.Telephone = newPhone;
                }

                if (!string.IsNullOrEmpty(newDobStr))
                {
                    if (DateOnly.TryParseExact(newDobStr, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateOnly newDob))
                    {
                        if (newDob >= DateOnly.FromDateTime(DateTime.Now))
                        {
                            Console.WriteLine("Lỗi: Ngày sinh phải là ngày trong quá khứ.");
                            Console.ReadKey();
                            return;
                        }
                        customer.CustomerBirthday = newDob;
                    }
                    else
                    {
                        Console.WriteLine("Lỗi: Định dạng ngày sinh không hợp lệ (Phải là dd/MM/yyyy).");
                        Console.ReadKey();
                        return;
                    }
                }

                customerRepository.UpdateCustomer(customer);

                MenuService.CurrentCustomer = customer;

                Console.WriteLine("\nCập nhật hồ sơ thành công!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi cập nhật hồ sơ: {ex.Message}");
            }

            Console.ReadKey();
        }
    }
}
