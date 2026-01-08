using System;
using System.Collections.Generic;
using System.Linq;
using Assignment.Model.Models;
using Microsoft.EntityFrameworkCore;

namespace Assignment.Repositories.Repository
{
    public class BookingReservationRepository : IBookingReservationRepository
    {
        private static string _connectionString = string.Empty; 

        private readonly FuminiHotelManagementContext _context;
        private static BookingReservationRepository _instance = null;

        private BookingReservationRepository()
        {
            if (string.IsNullOrEmpty(_connectionString))
            {
                throw new InvalidOperationException("Repository chưa được khởi tạo với Connection String.");
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

        public static BookingReservationRepository Instance => _instance ??= new BookingReservationRepository();

        public List<BookingReservation> GetReservationsByCustomerId(int customerId)
        {
            return _context.BookingReservations
                    .Include(br => br.BookingDetails)
                        .ThenInclude(bd => bd.Room)
                    .Where(br => br.CustomerId == customerId)
                    .OrderByDescending(br => br.BookingDate)
                    .ToList();
        }
        public void UpdateReservationStatus(int id, byte newStatus)
        {
            var reservation = _context.BookingReservations.Find(id);
            if (reservation == null)
            {
                throw new InvalidOperationException($"Không tìm thấy đơn đặt phòng có ID: {id}");
            }

            if (reservation.BookingStatus == newStatus)
            {
                return;
            }

            reservation.BookingStatus = newStatus;
            _context.BookingReservations.Update(reservation);
            _context.SaveChanges();
        }

       
        public void AddReservation(BookingReservation reservation, List<BookingDetail> details)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    _context.BookingReservations.Add(reservation);
                    _context.SaveChanges();

                    foreach (var detail in details)
                    {
                        detail.BookingReservationId = reservation.BookingReservationId;
                        _context.BookingDetails.Add(detail);
                    }
                    _context.SaveChanges();

                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }


        public List<BookingReservation> GetAllReservations()
        {
            return _context.BookingReservations
                .Include(br => br.Customer)
                .Include(br => br.BookingDetails)
                .ToList();
        }

        public BookingReservation GetReservationById(int id)
        {
            return _context.BookingReservations
                .Include(br => br.BookingDetails)
                    .ThenInclude(bd => bd.Room)
                .FirstOrDefault(br => br.BookingReservationId == id);
        }

        public void UpdateReservation(BookingReservation reservation)
        {
            _context.BookingReservations.Update(reservation);
            _context.SaveChanges();
        }

        public void DeleteReservation(int id)
        {
            var reservation = _context.BookingReservations.Find(id);
            if (reservation != null)
            {
                reservation.BookingStatus = 0;
                _context.BookingReservations.Update(reservation);
                _context.SaveChanges();
            }
        }
    }
}