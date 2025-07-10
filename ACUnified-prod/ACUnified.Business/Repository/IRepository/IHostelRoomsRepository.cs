using ACUnified.Data.DTOs;
using ACUnified.Data.Enum;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ACUnified.Business.Repository.IRepository
{
    public interface IHostelRoomsRepository
    {
        Task<HostelRoomsDto> CreateHostelRooms(HostelRoomsDto hostelRoomsDto);
        Task<IEnumerable<HostelRoomsDto>> GetAllHostelRooms();
        Task<IEnumerable<HostelRoomsDto>> GetRoomsByHostelId(long hostelId);
        Task<HostelRoomsDto> GetRoomById(long roomId);
    // Task<List<ApplicationFormDto>> GetRoomOccupants(long roomId);
   Task<List<HostelRoomsDto>> GetRoomOccupants(long roomId);

    Task<bool> AssignRoomToStudent(long roomId, long applicationFormId);
        Task<bool> UpdateRoomOccupancy(long roomId, OccupancyStatus newStatus);
        Task<HostelRoomsDto> GetHostelRooms(long id);
        Task DeleteHostelRooms(long id);
        Task<HostelRoomsDto> UpdateHostelRooms(HostelRoomsDto hostelRoomsDto);
    }

    public class RoomOccupantInfoDto
    {
    }
}