using Assignment.Model.Models;
using Assignment.Repositories.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp
{
    public static class AdminService
    {
    
        private static readonly CustomerRepository customerRepository = CustomerRepository.Instance;
        private static readonly RoomRepository roomRepository = RoomRepository.Instance;
        private static readonly BookingReservationRepository reservationRepository = BookingReservationRepository.Instance;
        private static readonly BookingDetailRepository detailRepository = BookingDetailRepository.Instance;


        private static readonly Input input = new Input();

        public static void ShowAdminDashboard()
        {
            while (MenuService.IsAdmin)
            {
                Console.Clear();
                Console.WriteLine("===== ADMIN DASHBOARD =====");
                Console.WriteLine("---------------------------");
                Console.WriteLine("1. Quản lý Khách hàng (CRUD)");
                Console.WriteLine("2. Quản lý Phòng (CRUD)");
                Console.WriteLine("3. Xem Báo cáo Thống kê");
                Console.WriteLine("0. Đăng xuất");
                Console.Write("\nChọn chức năng: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ManageCustomers();
                        break;
                    case "2":
                        ManageRooms();
                        break;
                    
                    case "3":
                        ViewReports();
                        break;
                    case "0":
                        Console.WriteLine("Đang đăng xuất Admin...");
                        return;
                    default:
                        Console.WriteLine("Lựa chọn không hợp lệ.");
                        Console.ReadKey();
                        break;
                }
            }
        }



        private static void ManageCustomers()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("===== QUẢN LÝ KHÁCH HÀNG =====");

                ListCustomers();

                Console.WriteLine("\n--- CHỨC NĂNG ---");
                Console.WriteLine("1. Thêm khách hàng mới (Add)");
                Console.WriteLine("2. Cập nhật thông tin (Update)");
                Console.WriteLine("3. Vô hiệu hóa (Delete/Deactive)");
                Console.WriteLine("0. Quay lại");
                Console.Write("Chọn: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddCustomer();
                        break;
                    case "2":
                        UpdateCustomer();
                        break;
                    case "3":
                        DeleteCustomer();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Lựa chọn không hợp lệ.");
                        Console.ReadKey();
                        break;
                }
            }
        }

        private static void ListCustomers()
        {
            try
            {
                var customers = customerRepository.GetAllCustomers();

                Console.WriteLine(new string('=', 105));
                Console.WriteLine($"{"ID",-5}{"Tên KH",-25}{"Email",-30}{"SĐT",-15}{"Ngày sinh",-15}{"Trạng thái",-10}");
                Console.WriteLine(new string('-', 105));

                foreach (var c in customers)
                {
                    string status = c.CustomerStatus == 1 ? "Active" : "Deactive";
                    Console.WriteLine($"{c.CustomerId,-5}{c.CustomerFullName,-25}{c.EmailAddress,-30}{c.Telephone,-15}{c.CustomerBirthday:dd/MM/yyyy,-15}{status,-10}");
                }
                Console.WriteLine(new string('=', 105));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi tải dữ liệu khách hàng: {ex.Message}");
                Console.ReadKey();
            }
        }

        private static void AddCustomer()
        {
            Console.Clear();
            Console.WriteLine("===== THÊM KHÁCH HÀNG MỚI =====");
            try
            {
                Console.Write("Họ và Tên: ");
                string fullName = Console.ReadLine()?.Trim();
                Console.Write("Email: ");
                string email = Console.ReadLine()?.Trim();
                Console.Write("Mật khẩu: ");
                string password = Console.ReadLine() ?? string.Empty;
                Console.Write("Số điện thoại: ");
                string phone = Console.ReadLine()?.Trim();
                Console.Write("Ngày sinh (dd/MM/yyyy): ");
                string dobStr = Console.ReadLine()?.Trim();

                if (string.IsNullOrWhiteSpace(fullName) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                    throw new InvalidOperationException("Vui lòng điền đầy đủ các trường bắt buộc.");
                if (!input.isValidPhoneNumberFormat(phone))
                    throw new InvalidOperationException("Số điện thoại không hợp lệ.");
                if (!DateOnly.TryParseExact(dobStr, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateOnly dob) || !input.isDate18YearsAgo(dob))
                    throw new InvalidOperationException("Ngày sinh không hợp lệ hoặc không đúng định dạng (dd/MM/yyyy).");

                var newCustomer = new Assignment.Model.Models.Customer
                {
                    CustomerFullName = fullName,
                    EmailAddress = email,
                    Password = password,
                    Telephone = phone,
                    CustomerBirthday = dob,
                    CustomerStatus = 1
                };

                customerRepository.AddCustomer(newCustomer);
                Console.WriteLine("\nThêm khách hàng mới thành công!");

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi: {ex.Message}");
            }
            Console.ReadKey();
        }

        private static void UpdateCustomer()
        {
            Console.Clear();
            Console.WriteLine("===== CẬP NHẬT THÔNG TIN KHÁCH HÀNG =====");
            Console.Write("Nhập ID khách hàng cần cập nhật: ");

            if (!int.TryParse(Console.ReadLine(), out int customerId))
            {
                Console.WriteLine("ID không hợp lệ.");
                Console.ReadKey();
                return;
            }

            var customer = customerRepository.GetCustomerById(customerId);
            if (customer == null)
            {
                Console.WriteLine($"Không tìm thấy khách hàng với ID: {customerId}");
                Console.ReadKey();
                return;
            }

            Console.WriteLine($"Tên cũ: {customer.CustomerFullName}");
            Console.Write("Tên mới (Enter để giữ cũ): ");
            string newName = Console.ReadLine()?.Trim();

            Console.WriteLine($"SĐT cũ: {customer.Telephone}");
            Console.Write("SĐT mới (Enter để giữ cũ): ");
            string newPhone = Console.ReadLine()?.Trim();

            Console.WriteLine($"Trạng thái cũ: {customer.CustomerStatus}");
            Console.Write("Trạng thái mới (1=Active, 0=Deactive) (Enter để giữ cũ): ");
            string newStatusStr = Console.ReadLine()?.Trim();

            try
            {
                if (!string.IsNullOrWhiteSpace(newName)) customer.CustomerFullName = newName;
                if (!string.IsNullOrWhiteSpace(newPhone))
                {
                    if (!input.isValidPhoneNumberFormat(newPhone)) throw new InvalidOperationException("Số điện thoại không hợp lệ.");
                    customer.Telephone = newPhone;
                }

                if (!string.IsNullOrWhiteSpace(newStatusStr))
                {
                    if (byte.TryParse(newStatusStr, out byte newStatus) && (newStatus == 0 || newStatus == 1))
                    {
                        customer.CustomerStatus = newStatus;
                    }
                    else
                    {
                        throw new InvalidOperationException("Trạng thái không hợp lệ (Chỉ chấp nhận 0 hoặc 1).");
                    }
                }

                customerRepository.UpdateCustomer(customer);
                Console.WriteLine("\nCập nhật thông tin khách hàng thành công!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi: {ex.Message}");
            }
            Console.ReadKey();
        }

        private static void DeleteCustomer()
        {
            Console.Clear();
            Console.WriteLine("===== VÔ HIỆU HÓA KHÁCH HÀNG =====");
            Console.Write("Nhập ID khách hàng cần vô hiệu hóa: ");
            if (int.TryParse(Console.ReadLine(), out int customerId))
            {
                Console.Write($"Bạn có chắc chắn muốn vô hiệu hóa khách hàng ID {customerId}? (Y/N): ");
                if (Console.ReadLine()?.Trim().ToUpper() == "Y")
                {
                    try
                    {
                        customerRepository.ChangeCustomerStatus(customerId,0);
                        Console.WriteLine($"Khách hàng ID {customerId} đã được vô hiệu hóa thành công.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Lỗi DB: {ex.Message}");
                    }
                }
            }
            else
            {
                Console.WriteLine("ID không hợp lệ.");
            }
            Console.ReadKey();
        }

        private static void ManageRooms()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("===== QUẢN LÝ PHÒNG =====");

                ListRooms();

                Console.WriteLine("\n--- CHỨC NĂNG ---");
                Console.WriteLine("1. Thêm phòng mới (Add)");
                Console.WriteLine("2. Cập nhật thông tin (Update)");
                Console.WriteLine("3. Vô hiệu hóa (Delete/Inactive)");
                Console.WriteLine("0. Quay lại");
                Console.Write("Chọn: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddRoom();
                        break;
                    case "2":
                        UpdateRoom();
                        break;
                    case "3":
                        DeleteRoom();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Lựa chọn không hợp lệ.");
                        Console.ReadKey();
                        break;
                }
            }
        }

        private static void ListRooms()
        {
            try
            {
                var rooms = roomRepository.GetAllRooms();

                Console.WriteLine(new string('=', 95));
                Console.WriteLine($"{"ID",-5}{"Số phòng",-15}{"Tên phòng",-35}{"Giá/ngày",-15}{"Trạng thái",-10}");
                Console.WriteLine(new string('-', 95));

                foreach (var r in rooms)
                {
                    string status = r.RoomStatus == 1 ? "Active" : "Inactive";
                    string price = r.RoomPricePerDay.HasValue ? $"{r.RoomPricePerDay.Value:N0}" : "N/A";
                    Console.WriteLine($"{r.RoomId,-5}{r.RoomNumber,-15}{r.RoomDetailDescription,-35}{price,-15}{status,-10}");
                }
                Console.WriteLine(new string('=', 95));

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi tải dữ liệu phòng: {ex.Message}");
                Console.ReadKey();
            }
        }

        private static void AddRoom()
        {
            Console.Clear();
            Console.WriteLine("===== THÊM PHÒNG MỚI =====");
            try
            {
                Console.Write("Số phòng (ví dụ: 101): ");
                string roomNumber = Console.ReadLine()?.Trim();

                Console.Write("Mô tả/Tên phòng: "); 
                string description = Console.ReadLine()?.Trim();

                Console.Write("Sức chứa tối đa (Max Capacity): ");
                if (!int.TryParse(Console.ReadLine(), out int maxCapacity) || maxCapacity <= 0)
                    throw new InvalidOperationException("Sức chứa không hợp lệ.");

                Console.Write("ID Loại phòng : ");
                if (!int.TryParse(Console.ReadLine(), out int roomTypeId) || roomTypeId <= 0)
                    roomTypeId = 1; 

                Console.Write("Giá mỗi ngày: ");
                if (!decimal.TryParse(Console.ReadLine(), out decimal price) || price <= 0)
                    throw new InvalidOperationException("Giá không hợp lệ hoặc nhỏ hơn 0.");

                if (string.IsNullOrWhiteSpace(roomNumber))
                    throw new InvalidOperationException("Vui lòng nhập Số phòng.");


                var newRoom = new Assignment.Model.Models.RoomInformation
                {
                    RoomNumber = roomNumber,
                    RoomDetailDescription = description,
                    RoomMaxCapacity = maxCapacity, 
                    RoomTypeId = roomTypeId,      
                    RoomPricePerDay = price,
                    RoomStatus = 1,
                };

                roomRepository.AddRoom(newRoom);
                Console.WriteLine("\nThêm phòng mới thành công!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi: {ex.Message}");
            }
            Console.ReadKey();
        }
        private static void UpdateRoom()
        {
            Console.Clear();
            Console.WriteLine("===== CẬP NHẬT PHÒNG =====");
            Console.Write("Nhập ID phòng cần cập nhật: ");
            if (!int.TryParse(Console.ReadLine(), out int roomId))
            {
                Console.WriteLine("ID không hợp lệ.");
                Console.ReadKey();
                return;
            }

            var room = roomRepository.GetRoomById(roomId);
            if (room == null)
            {
                Console.WriteLine($"Không tìm thấy phòng với ID: {roomId}");
                Console.ReadKey();
                return;
            }

            Console.WriteLine($"Tên phòng cũ: {room.RoomDetailDescription}");
            Console.Write("Tên phòng mới (Enter để giữ cũ): ");
            string newName = Console.ReadLine()?.Trim();

            Console.WriteLine($"Giá cũ: {room.RoomPricePerDay:N0}");
            Console.Write("Giá mới (Enter để giữ cũ): ");
            string newPriceStr = Console.ReadLine()?.Trim();

            Console.WriteLine($"Trạng thái cũ: {room.RoomStatus}");
            Console.Write("Trạng thái mới (1=Active, 0=Inactive) (Enter để giữ cũ): ");
            string newStatusStr = Console.ReadLine()?.Trim();

            try
            {
                if (!string.IsNullOrWhiteSpace(newName)) room.RoomDetailDescription = newName;

                if (!string.IsNullOrWhiteSpace(newPriceStr))
                {
                    if (decimal.TryParse(newPriceStr, out decimal newPrice) && newPrice > 0)
                    {
                        room.RoomPricePerDay = newPrice;
                    }
                    else
                    {
                        throw new InvalidOperationException("Giá không hợp lệ.");
                    }
                }

                if (!string.IsNullOrWhiteSpace(newStatusStr))
                {
                    if (byte.TryParse(newStatusStr, out byte newStatus) && (newStatus == 0 || newStatus == 1))
                    {
                        room.RoomStatus = newStatus;
                    }
                    else
                    {
                        throw new InvalidOperationException("Trạng thái không hợp lệ.");
                    }
                }

                roomRepository.UpdateRoom(room);
                Console.WriteLine("\nCập nhật thông tin phòng thành công!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi: {ex.Message}");
            }
            Console.ReadKey();
        }

        private static void DeleteRoom()
        {
            Console.Clear();
            Console.WriteLine("===== VÔ HIỆU HÓA PHÒNG =====");
            Console.Write("Nhập ID phòng cần vô hiệu hóa: ");
            if (int.TryParse(Console.ReadLine(), out int roomId))
            {
                Console.Write($"Bạn có chắc chắn muốn vô hiệu hóa phòng ID {roomId}? (Y/N): ");
                if (Console.ReadLine()?.Trim().ToUpper() == "Y")
                {
                    try
                    {
                        roomRepository.ChangeRoomStatus(roomId,0);
                        Console.WriteLine($"Phòng ID {roomId} đã được vô hiệu hóa thành công.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Lỗi DB: {ex.Message}");
                    }
                }
            }
            else
            {
                Console.WriteLine("ID không hợp lệ.");
            }
            Console.ReadKey();
        }

       
        private static void ViewReports()
        {
            Console.Clear();
            Console.WriteLine("===== BÁO CÁO THỐNG KÊ DOANH THU =====");
            Console.Write("Nhập Ngày Bắt đầu (dd/MM/yyyy): ");
            string startStr = Console.ReadLine();
            Console.Write("Nhập Ngày Kết thúc (dd/MM/yyyy): ");
            string endStr = Console.ReadLine();

            if (!DateOnly.TryParseExact(startStr, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateOnly startDate) ||
                !DateOnly.TryParseExact(endStr, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateOnly endDate) ||
                startDate > endDate)
            {
                Console.WriteLine("Lỗi: Định dạng hoặc khoảng ngày không hợp lệ.");
                Console.ReadKey();
                return;
            }

            try
            {
                var reportDetails = detailRepository.GetBookingDetailsByDateRange(startDate, endDate);

                if (!reportDetails.Any())
                {
                    Console.WriteLine("\nKhông có dữ liệu đặt phòng nào trong khoảng thời gian này.");
                    Console.ReadKey();
                    return;
                }

                decimal totalRevenue = reportDetails.Sum(d => d.ActualPrice ?? 0);

                Console.WriteLine("\n--- KẾT QUẢ THỐNG KÊ ---");
                Console.WriteLine($"Tổng Doanh thu: {totalRevenue:N0} VNĐ");

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi lấy báo cáo: {ex.Message}");
            }

            Console.WriteLine("\nNhấn phím bất kỳ để tiếp tục...");
            Console.ReadKey();
        }
    }
}