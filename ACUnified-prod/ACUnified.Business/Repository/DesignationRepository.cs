using ACUnified.Data.Models;
using ACUnified.Data.DTOs;
using AutoMapper;

using Microsoft.EntityFrameworkCore;
public class DesignationRepository : IDesignationRepository
{
    private readonly DbContextOptions<ApplicationDbContext> dbOptions;
    private readonly IMapper _mapper;
    public DesignationRepository(DbContextOptions<ApplicationDbContext> options, IMapper mapper)
    {
        _mapper = mapper;
        dbOptions = options;
    }

    public async Task<DesignationDto> CreateDesignation(DesignationDto designationDto)
    {
        using (var db = new ApplicationDbContext(dbOptions))
        {
            Designation designation = _mapper.Map<DesignationDto, Designation>(designationDto);
            var addedDesignation = db.Designation.Add(designation);
            await db.SaveChangesAsync();
            return _mapper.Map<Designation, DesignationDto>(addedDesignation.Entity);
        }
    }

    public async Task<IEnumerable<DesignationDto>> GetAllDesignation()
    {
        try
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                IEnumerable<DesignationDto> designationDtos =
                    _mapper.Map<IEnumerable<Designation>, IEnumerable<DesignationDto>>(db.Designation);
                return designationDtos;
            }
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    public async Task<DesignationDto> GetDesignation(long Id)
    {
        try
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                DesignationDto designationDto =
                    _mapper.Map<Designation, DesignationDto>(
                        await db.Designation.FirstOrDefaultAsync(x => x.Id == Id));
                return designationDto;
            }
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    public async Task DeleteDesignation(long Id)
    {
        using (var db = new ApplicationDbContext(dbOptions))
        {
            var designation = db.Designation.FirstOrDefault(x => x.Id == Id);
            if (designation != null)
            {
                db.Designation.Remove(designation);
                db.SaveChanges();
            }
        }
    }

    public async Task<DesignationDto> UpdateDesignation(DesignationDto designationDto)
    {
        try
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                Designation designation = db.Designation.FirstOrDefault(x => x.Id == designationDto.Id);
                if (designation == null)
                {
                    return null;
                }
                else
                {
                    Designation designationUpdate =
                        _mapper.Map<DesignationDto, Designation>(designationDto, designation);
                    var currentupdate = db.Designation.Update(designationUpdate);
                    await db.SaveChangesAsync();
                    return _mapper.Map<Designation, DesignationDto>(currentupdate.Entity);
                }
            }
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    Task<DesignationDto> IDesignationRepository.CreateDesignation(DesignationDto designationDto)
    {
        throw new NotImplementedException();
    }

    Task<IEnumerable<DesignationDto>> IDesignationRepository.GetAllDesignation()
    {
        throw new NotImplementedException();
    }

    Task<DesignationDto> IDesignationRepository.GetDesignation(long Id)
    {
        throw new NotImplementedException();
    }

    Task<DesignationDto> IDesignationRepository.UpdateDesignation(DesignationDto designationDto)
    {
        throw new NotImplementedException();
    }
}
