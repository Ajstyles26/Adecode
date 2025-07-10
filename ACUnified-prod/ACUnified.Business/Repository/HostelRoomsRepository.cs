using ACUnified.Business.Repository.IRepository;
using ACUnified.Data.DTOs;
using ACUnified.Data.Enum;
using ACUnified.Data.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace ACUnified.Business.Repository
{
    public class HostelRoomsRepository : IHostelRoomsRepository
    {
        private readonly DbContextOptions<ApplicationDbContext> _dbOptions;
        private readonly IMapper _mapper;

        public HostelRoomsRepository(DbContextOptions<ApplicationDbContext> options, IMapper mapper)
        {
            _dbOptions = options;
            _mapper = mapper;
        }

        public async Task<HostelRoomsDto> CreateHostelRooms(HostelRoomsDto hostelRoomsDto)
        {
            using (var db = new ApplicationDbContext(_dbOptions))
            {
                HostelRooms hostelRooms = _mapper.Map<HostelRoomsDto, HostelRooms>(hostelRoomsDto);
                var addedHostelRooms = db.HostelRooms.Add(hostelRooms);
                await db.SaveChangesAsync();
                return _mapper.Map<HostelRooms, HostelRoomsDto>(addedHostelRooms.Entity);
            }
        }

        public async Task<IEnumerable<HostelRoomsDto>> GetAllHostelRooms()
        {
            try
            {
                using (var db = new ApplicationDbContext(_dbOptions))
                {
                    IEnumerable<HostelRoomsDto> hostelRoomsDtos =
                        _mapper.Map<IEnumerable<HostelRooms>, IEnumerable<HostelRoomsDto>>(db.HostelRooms.Include(x =>x.HostelBuilding));
                    return hostelRoomsDtos;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetAllHostelRooms: {ex.Message}");
                return null;
            }
        }

    public async Task<IEnumerable<HostelRoomsDto>> GetRoomsByHostelId(long hostelId)
{
    try
    {
        using (var db = new ApplicationDbContext(_dbOptions))
        {
            var rooms = await db.HostelRooms
                .Where(r => r.HostelBuildingId == hostelId)
                .Include(r => r.HostelBuilding)
                .ToListAsync();

            return _mapper.Map<IEnumerable<HostelRooms>, IEnumerable<HostelRoomsDto>>(rooms);
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error in GetRoomsByHostelId: {ex.Message}");
        return null;
    }
}


        public async Task<bool> UpdateRoomOccupancy(long roomId, OccupancyStatus newStatus)
        {
            try
            {
                using (var db = new ApplicationDbContext(_dbOptions))
                {
                    var room = await db.HostelRooms.FindAsync(roomId);
                    if (room == null)
                        return false;

                    room.OccupancyStatus = newStatus;
                    await db.SaveChangesAsync();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in UpdateRoomOccupancy: {ex.Message}");
                return false;
            }
        }

        public async Task<HostelRoomsDto> GetHostelRooms(long id)
        {
            try
            {
                using (var db = new ApplicationDbContext(_dbOptions))
                {
                    HostelRoomsDto hostelRoomsDto =
                        _mapper.Map<HostelRooms, HostelRoomsDto>(await db.HostelRooms.FirstOrDefaultAsync(x => x.Id == id));
                    return hostelRoomsDto;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetHostelRooms: {ex.Message}");
                return null;
            }
        }

        public async Task DeleteHostelRooms(long id)
        {
            using (var db = new ApplicationDbContext(_dbOptions))
            {
                var hostelRooms = db.HostelRooms.FirstOrDefault(x => x.Id == id);
                if (hostelRooms != null)
                {
                    db.HostelRooms.Remove(hostelRooms);
                    await db.SaveChangesAsync();
                }
            }
        }
public async Task<HostelRoomsDto> GetRoomById(long roomId)
{
    using (var db = new ApplicationDbContext(_dbOptions))
    {
        var room = await db.HostelRooms
            .Where(r => r.Id == roomId)
            .Select(r => new HostelRoomsDto
            {
                Id = r.Id,
                RoomNumber = r.RoomNumber,
                Capacity = r.Capacity,
                OccupancyStatus = r.OccupancyStatus,
                // Map other properties as needed
            })
            .FirstOrDefaultAsync();

        return room;
    }
}
public async Task<List<HostelRoomsDto>> GetRoomOccupants(long roomId)
{
    using (var db = new ApplicationDbContext(_dbOptions))
    {
        var occupants = await db.HostelAllocation
            .Where(ha => ha.HostelRoomId == roomId && ha.CurrentStatus == AllocationStatus.Active)
            .Select(ha => new HostelRoomsDto
            {
                Id = ha.HostelRoom.Id,
                HostelBuildingId = ha.HostelRoom.HostelBuildingId,
                HostelBuilding = ha.HostelRoom.HostelBuilding,
                RoomNumber = ha.HostelRoom.RoomNumber,
                Capacity = ha.HostelRoom.Capacity,
                OccupancyStatus = ha.HostelRoom.OccupancyStatus,
                Occupants = new List<ApplicationFormDto>
                {
                    new ApplicationFormDto
                    {
                        Id = ha.ApplicationForm.Id,
                        LevelId = ha.ApplicationForm.LevelId,
                        FormRefNo = ha.ApplicationForm.FormRefNo,
                        BioData = new BioDataDto
                        {
                            FirstName = ha.ApplicationForm.BioData.FirstName,
                            LastName = ha.ApplicationForm.BioData.LastName,
                            OtherName = ha.ApplicationForm.BioData.OtherName
                        }
                    }
                }
            })
            .ToListAsync();
        
        return occupants;
    }
}
public async Task<bool> AssignRoomToStudent(long roomId, long applicationFormId)
{
    using (var db = new ApplicationDbContext(_dbOptions))
    {
        using var transaction = await db.Database.BeginTransactionAsync();

        try
        {
            var room = await db.HostelRooms.FindAsync(roomId);
            if (room == null)
            {
                return false;
            }

            var applicationForm = await db.ApplicationForm.FindAsync(applicationFormId);
            if (applicationForm == null)
            {
                return false;
            }

            // Check if the room is already full
            var currentOccupants = await db.HostelAllocation.CountAsync(ha => ha.HostelRoomId == roomId && ha.CurrentStatus == AllocationStatus.Active);
            if (currentOccupants >= room.Capacity)
            {
                return false;
            }

            // Create a new HostelAllocation
            var hostelAllocation = new HostelAllocation
            {
                ApplicationFormId = applicationFormId,
                HostelRoomId = roomId,
                DateAllocated = DateTime.UtcNow,
                HostelDuration = AllocationDuration.Regular, // You may want to make this configurable
                CurrentStatus = AllocationStatus.Active
            };

            // Add the new allocation
            db.HostelAllocation.Add(hostelAllocation);
           // Decrease room capacity by 1
            room.Capacity = Math.Max(0, room.Capacity - 1);
            
            // Update room occupancy status
            room.OccupancyStatus = OccupancyStatus.Occupied;
            
            await db.SaveChangesAsync();
            
            await transaction.CommitAsync();
            return true;
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            return false;
        }
    }
}
    
public async Task<HostelRoomsDto> UpdateHostelRooms(HostelRoomsDto hostelRoomsDto)
{
    try
    {
        using (var db = new ApplicationDbContext(_dbOptions))
        {
            HostelRooms hostelRooms = await db.HostelRooms.FirstOrDefaultAsync(x => x.Id == hostelRoomsDto.Id);
            if (hostelRooms == null)
            {
                Console.WriteLine($"HostelRooms with Id {hostelRoomsDto.Id} not found.");
                return null;
            }
            else
            {
                // Update only specific fields
                hostelRooms.OccupancyStatus = hostelRoomsDto.OccupancyStatus;
                hostelRooms.Capacity = hostelRoomsDto.Capacity;

                // If the room is now full, update the status
                if (hostelRooms.Capacity == 0)
                {
                    hostelRooms.OccupancyStatus = OccupancyStatus.Occupied;
                }

                var currentUpdate = db.HostelRooms.Update(hostelRooms);
                await db.SaveChangesAsync();
                return _mapper.Map<HostelRooms, HostelRoomsDto>(currentUpdate.Entity);
            }
        }
    }
    catch (DbUpdateException dbEx)
    {
        Console.WriteLine($"Database update error in UpdateHostelRooms: {dbEx.Message}");
        Console.WriteLine($"Inner exception: {dbEx.InnerException?.Message}");
        return null;
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error in UpdateHostelRooms: {ex.Message}");
        Console.WriteLine($"Stack trace: {ex.StackTrace}");
        return null;
            }
        }
    }
}