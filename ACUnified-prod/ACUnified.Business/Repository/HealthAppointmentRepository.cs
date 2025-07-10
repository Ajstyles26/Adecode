using ACUnified.Business.Repository.IRepository;
using ACUnified.Data.DTOs;
using ACUnified.Data.Models;

using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace ACUnified.Business.Repository;

public class HealthAppointmentRepository:IHealthAppointmentRepository
{
    private readonly DbContextOptions<ApplicationDbContext> dbOptions;
    private readonly IMapper _mapper;

    public HealthAppointmentRepository(DbContextOptions<ApplicationDbContext> options, IMapper mapper)
    {
        _mapper = mapper;
        dbOptions = options;
    }
    public async Task<HealthAppointmentDto> CreateHealthAppointment(HealthAppointmentDto healthDto)
    {
        using (var db=new ApplicationDbContext(dbOptions))
        {
            HealthAppointment healthAppointment = _mapper.Map<HealthAppointmentDto, HealthAppointment>(healthDto);
                             var addedHealthAppointment = db.HealthAppointment.Add(healthAppointment);
                             await db.SaveChangesAsync();
                             return _mapper.Map<HealthAppointment, HealthAppointmentDto>(addedHealthAppointment.Entity);
        }
        
    }

    public async Task<IEnumerable<HealthAppointmentDto>> GetAllHealthAppointment()
    {
        try
        {
            using (var db=new ApplicationDbContext(dbOptions))
            {
                   IEnumerable<HealthAppointmentDto> healthAppointmentDtos = _mapper.Map<IEnumerable<HealthAppointment>, IEnumerable<HealthAppointmentDto>>(db.HealthAppointment);
                                         return healthAppointmentDtos;
            }
         
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    public async Task<IEnumerable<HealthAppointmentDto>> GetAllHealthAppointmentByUserId(string UserId)
    {
        try
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                IEnumerable<HealthAppointmentDto> healthAppointmentDtos = _mapper.Map<IEnumerable<HealthAppointment>, IEnumerable<HealthAppointmentDto>>(db.HealthAppointment.Where(x=>x.UserId==UserId));
                return healthAppointmentDtos;
            }

        }
        catch (Exception ex)
        {
            return null;
        }
    }

    public async Task<HealthAppointmentDto> GetHealthAppointment(long Id)
    {
        try
        {
            using (var db=new ApplicationDbContext(dbOptions))
            {
                  HealthAppointmentDto healthAppointmentDto = _mapper.Map<HealthAppointment, HealthAppointmentDto>(await db.HealthAppointment.FirstOrDefaultAsync(x => x.Id == Id));
                            return healthAppointmentDto;
            }
          
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    public Task DeleteHealthAppointment(long Id)
    {
        throw new NotImplementedException();
    }

    public async Task<HealthAppointmentDto> UpdateHealthAppointment(HealthAppointmentDto healthDto)
    {
        try
        {
            using (var db=new ApplicationDbContext(dbOptions))
            {
                HealthAppointment healthAppointment = db.HealthAppointment.FirstOrDefault(x => x.Id == healthDto.Id);
                            if (healthAppointment == null)
                            {
                                return null;
                            }
                            else
                            {
                                HealthAppointment healthAppointmentUpdate = _mapper.Map<HealthAppointmentDto, HealthAppointment>(healthDto, healthAppointment);
                                var currentupdate = db.HealthAppointment.Update(healthAppointmentUpdate);
                                await db.SaveChangesAsync();
                                return _mapper.Map<HealthAppointment, HealthAppointmentDto>(currentupdate.Entity);
                            }
            }
            
        }
        catch (Exception ex)
        {
            return null;
        }
    }
}