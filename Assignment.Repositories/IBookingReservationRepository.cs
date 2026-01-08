using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assignment.Model.Models;
namespace Assignment.Repositories
{
    public interface IBookingReservationRepository
    {
        List<BookingReservation> GetReservationsByCustomerId(int customerId);
        List<BookingReservation> GetAllReservations();
        BookingReservation GetReservationById(int id);
        void AddReservation(BookingReservation reservation, List<BookingDetail> details);
        void UpdateReservation(BookingReservation reservation);
        void DeleteReservation(int id);
        void UpdateReservationStatus(int id, byte newStatus);
    }
}
