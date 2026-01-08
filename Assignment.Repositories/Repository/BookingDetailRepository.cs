using Assignment.Model.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment.Repositories.Repository
{
    public class BookingDetailRepository : IBookingDetailRepository
    {
        private static string _connectionString = string.Empty;
        private readonly FuminiHotelManagementContext _context;
        private static BookingDetailRepository _instance = null;

        private BookingDetailRepository()
        {
            if (string.IsNullOrEmpty(_connectionString))
            {
                throw new InvalidOperationException("Repository chưa được khởi tạo với Connection String. Vui lòng gọi Initialize(connectionString).");
            }

            var optionsBuilder = new DbContextOptionsBuilder<FuminiHotelManagementContext>();
            optionsBuilder.UseSqlServer(_connectionString);

            _context = new FuminiHotelManagementContext(optionsBuilder.Options);
        }

        public static void Initialize(string connectionString)
        {
            if (string.IsNullOrEmpty(_connectionString))
            {
                _connectionString = connectionString;
            }
        }

        public static BookingDetailRepository Instance
        {
            get => _instance ??= new BookingDetailRepository();
        }


        public List<BookingDetail> GetDetailsByReservationId(int reservationId)
        {
            return _context.BookingDetails
                .Where(bd => bd.BookingReservationId == reservationId)
                .Include(bd => bd.Room)
                .ToList();
        }

        public BookingDetail? GetDetailByKey(int reservationId, int roomId)
        {
            return _context.BookingDetails.Find(reservationId, roomId);
        }

        public bool IsRoomAvailable(int roomId, DateOnly startDate, DateOnly endDate, int? excludeReservationId = null)
        {
          
            var query = _context.BookingDetails
                .Where(bd => bd.RoomId == roomId && bd.BookingReservation.BookingStatus == 1) 
                .Where(bd => (bd.StartDate < endDate) && (bd.EndDate > startDate));

            if (excludeReservationId.HasValue)
            {
                query = query.Where(bd => bd.BookingReservationId != excludeReservationId.Value);
            }

            return !query.Any();
        }

        public void AddBookingDetail(BookingDetail bookingDetail)
        {

            if (!IsRoomAvailable(bookingDetail.RoomId, bookingDetail.StartDate, bookingDetail.EndDate, bookingDetail.BookingReservationId))
            {
                throw new InvalidOperationException("Phòng đã bị đặt cho khoảng thời gian này.");
            }

            _context.BookingDetails.Add(bookingDetail);
            _context.SaveChanges();


            UpdateReservationTotalPrice(bookingDetail.BookingReservationId);
        }

        public void UpdateBookingDetail(BookingDetail bookingDetail)
        {
            var existingDetail = GetDetailByKey(bookingDetail.BookingReservationId, bookingDetail.RoomId);
            if (existingDetail == null)
            {
                throw new InvalidOperationException("Không tìm thấy chi tiết đặt phòng để cập nhật.");
            }

            if (!IsRoomAvailable(bookingDetail.RoomId, bookingDetail.StartDate, bookingDetail.EndDate, bookingDetail.BookingReservationId))
            {
                throw new InvalidOperationException("Phòng đã bị đặt cho khoảng thời gian này sau khi cập nhật.");
            }

            _context.Entry(existingDetail).CurrentValues.SetValues(bookingDetail);
            _context.SaveChanges();

            UpdateReservationTotalPrice(bookingDetail.BookingReservationId);
        }

        public void DeleteBookingDetail(int reservationId, int roomId)
        {
            var detailToDelete = GetDetailByKey(reservationId, roomId);
            if (detailToDelete == null)
            {
                throw new InvalidOperationException("Không tìm thấy chi tiết đặt phòng để xóa.");
            }

            _context.BookingDetails.Remove(detailToDelete);
            _context.SaveChanges();

            UpdateReservationTotalPrice(reservationId);
        }


        private void UpdateReservationTotalPrice(int reservationId)
        {
            var reservation = _context.BookingReservations
                .Include(r => r.BookingDetails)
                .FirstOrDefault(r => r.BookingReservationId == reservationId);

            if (reservation != null)
            {

                decimal newTotalPrice = reservation.BookingDetails.Sum(bd => bd.ActualPrice ?? 0);

                reservation.TotalPrice = newTotalPrice;
                _context.BookingReservations.Update(reservation);
                _context.SaveChanges();
            }
        }
        public List<BookingDetail> GetBookingDetailsByDateRange(DateOnly startDate, DateOnly endDate)
        {
    
            return _context.BookingDetails
                .Include(x => x.Room)
                .Include(x => x.BookingReservation)
                .Include(x => x.BookingReservation.Customer)
                .Where(x => x.StartDate >= startDate && x.EndDate <= endDate) 
                .Where(x => x.BookingReservation.BookingStatus == 1) 
                .OrderByDescending(x => x.ActualPrice) 
                .ToList();
        }

        public List<BookingDetail> GetAllBookingDetails()
        {
            return _context.BookingDetails
                .Include(x => x.Room)
                .Include(x => x.BookingReservation)
                .Include(x => x.BookingReservation.Customer)
                .OrderByDescending(x => x.ActualPrice) 
                .ToList();
        }
    }
}
