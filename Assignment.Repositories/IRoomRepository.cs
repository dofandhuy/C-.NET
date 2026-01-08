using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assignment.Model.Models;
namespace Assignment.Repositories
{
    public interface IRoomRepository
    {
        List<RoomInformation> GetAllRooms();
        RoomInformation GetRoomById(int id);
        List<RoomInformation> GetAvailableRooms(DateTime startDate, DateTime endDate, int? roomTypeId = null);
        void AddRoom(RoomInformation room);
        void UpdateRoom(RoomInformation room);
        void ChangeRoomStatus(int roomId, byte status);
    }
}
