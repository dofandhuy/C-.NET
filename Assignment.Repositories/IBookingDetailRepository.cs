using Assignment.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment.Repositories
{
    public interface IBookingDetailRepository
    {
        List<BookingDetail> GetDetailsByReservationId(int reservationId);
        BookingDetail? GetDetailByKey(int reservationId, int roomId);
        bool IsRoomAvailable(int roomId, DateOnly startDate, DateOnly endDate, int? excludeReservationId = null);

        void AddBookingDetail(BookingDetail bookingDetail);
        void UpdateBookingDetail(BookingDetail bookingDetail);
        void DeleteBookingDetail(int reservationId, int roomId);
 
        List<BookingDetail> GetBookingDetailsByDateRange(DateOnly startDate, DateOnly endDate);
    }
}
