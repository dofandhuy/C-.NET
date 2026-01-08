using System;
using System.Collections.Generic;
using System.Linq;
using Assignment.Model.Models;
using Microsoft.EntityFrameworkCore;

namespace Assignment.Repositories.Repository
{
    public class RoomRepository : IRoomRepository
    {
        private static string _connectionString = string.Empty; 
        private readonly FuminiHotelManagementContext _context;
        private static RoomRepository _instance = null;

        private RoomRepository()
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

        public static RoomRepository Instance
        {
            get => _instance ??= new RoomRepository();
        }

        public List<RoomInformation> GetAllRooms()
        {
            return _context.RoomInformations.Include(r => r.RoomType).ToList();
        }

        public RoomInformation GetRoomById(int id)
        {
            return _context.RoomInformations
                .Include(r => r.RoomType)
                .FirstOrDefault(r => r.RoomId == id);
        }

        public List<RoomInformation> GetAvailableRooms(DateTime startDate, DateTime endDate, int? roomTypeId = null)
        {
            DateOnly inputStartDate = DateOnly.FromDateTime(startDate.Date);
            DateOnly inputEndDate = DateOnly.FromDateTime(endDate.Date);

            var bookedRoomIds = _context.BookingDetails
                .Where(bd =>
                    (bd.StartDate < inputEndDate) && (bd.EndDate > inputStartDate)
                )
                .Select(bd => bd.RoomId)
                .ToList();

            var query = _context.RoomInformations
                .Include(r => r.RoomType)
                .Where(r =>
                    r.RoomStatus == 1 &&
                    !bookedRoomIds.Contains(r.RoomId)
                );

            if (roomTypeId.HasValue && roomTypeId.Value > 0)
            {
                query = query.Where(r => r.RoomTypeId == roomTypeId.Value);
            }

            return query.OrderBy(r => r.RoomPricePerDay).ToList();
        }

        public void AddRoom(RoomInformation room)
        {
            _context.RoomInformations.Add(room);
            _context.SaveChanges();
        }

        public void UpdateRoom(RoomInformation room)
        {
            var existingRoom = _context.RoomInformations.Find(room.RoomId);
            if (existingRoom != null)
            {
                _context.RoomInformations.Update(room);
                _context.SaveChanges();
            }
            else
            {
                throw new InvalidOperationException($"Không tìm thấy phòng với ID: {room.RoomId}");
            }
        }

        public void ChangeRoomStatus(int roomId, byte status)
        {
            var room = _context.RoomInformations.Find(roomId);
            if (room != null)
            {
                room.RoomStatus = status;
                _context.RoomInformations.Update(room);
                _context.SaveChanges();
            }
            else
            {
                throw new InvalidOperationException($"Không tìm thấy phòng với ID: {roomId}");
            }
        }
        public List<RoomType> GetAllRoomType()
        {
            return _context.RoomTypes.ToList();
        }
    }
}