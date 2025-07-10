using ACUnified.Business.Repository.IRepository;
using ACUnified.Data.DTOs;
using ACUnified.Data.Enum;
using ACUnified.Data.Models;

using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACUnified.Business.Repository
{
    public class HostelAllocationRepository : IHostelAllocationRepository
    {
        private readonly DbContextOptions<ApplicationDbContext> dbOptions;
        private readonly IMapper _mapper;
        public HostelAllocationRepository(DbContextOptions<ApplicationDbContext> options, IMapper mapper)
        {
            _mapper = mapper;
            dbOptions = options;
        }

        public async Task<HostelAllocationDto> CreateHostelAllocation(HostelAllocationDto HostelAllocationDto)
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                HostelAllocation HostelAllocation = _mapper.Map<HostelAllocationDto, HostelAllocation>(HostelAllocationDto);
                var addedHostelAllocation = db.HostelAllocation.Add(HostelAllocation);
                await db.SaveChangesAsync();
                return _mapper.Map<HostelAllocation, HostelAllocationDto>(addedHostelAllocation.Entity);
            }
        }
public async Task<HostelAllocationDto> GetHostelAllocationByApplicationFormId(long applicationFormId)
{
    try
    {
        using (var db = new ApplicationDbContext(dbOptions))
        {
            var hostelAllocation = await db.HostelAllocation
                .Include(x => x.HostelRoom)
                    .ThenInclude(hr => hr.HostelBuilding)
                .Include(x => x.Student)
                .Include(x => x.ApplicationForm)
                .FirstOrDefaultAsync(x => x.ApplicationFormId == applicationFormId);

            if (hostelAllocation == null)
            {
                return null;
            }

            return _mapper.Map<HostelAllocation, HostelAllocationDto>(hostelAllocation);
        }
    }
    catch (Exception ex)
    {
        // Log the exception
        Console.WriteLine($"Error in GetHostelAllocationByApplicationFormId: {ex.Message}");
        return null;
    }
}
        public async Task<IEnumerable<HostelAllocationDto>> GetAllHostelAllocation()
        {
            try
            {
                using (var db = new ApplicationDbContext(dbOptions))
                {
                    IEnumerable<HostelAllocationDto> HostelAllocationDtos =
                        _mapper.Map<IEnumerable<HostelAllocation>, IEnumerable<HostelAllocationDto>>(db.HostelAllocation .Include(x => x.HostelRoom)
                        .ThenInclude(x => x.HostelBuilding)
                        .Include(x=> x.ApplicationForm)
                        .ThenInclude(x => x.BioData));
                       
                    return HostelAllocationDtos;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<HostelAllocationDto> GetHostelAllocation(long Id)
        {
            try
            {
                using (var db = new ApplicationDbContext(dbOptions))
                {
                    HostelAllocationDto HostelAllocationDto =
                        _mapper.Map<HostelAllocation, HostelAllocationDto>(await db.HostelAllocation.FirstOrDefaultAsync(x => x.Id == Id));
                    return HostelAllocationDto;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task DeleteHostelAllocation(long Id)
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                var HostelAllocation = db.HostelAllocation.FirstOrDefault(x => x.Id == Id);
                if (HostelAllocation != null)
                {
                    db.HostelAllocation.Remove(HostelAllocation);
                    db.SaveChanges();
                }


            }
        }
        public async Task<HostelAllocationDto> CreateHostelAllocationAndUpdateStage(HostelAllocationDto hostelAllocationDto)
    {
        using (var db = new ApplicationDbContext(dbOptions))
        {
            using (var transaction = await db.Database.BeginTransactionAsync())
            {
                try
                {
                    HostelAllocation hostelAllocation = _mapper.Map<HostelAllocationDto, HostelAllocation>(hostelAllocationDto);
                    var addedHostelAllocation = db.HostelAllocation.Add(hostelAllocation);

                    // Update the ApplicationForm stage to Stage7
                    var applicationForm = await db.ApplicationForm.FirstOrDefaultAsync(af => af.Id == hostelAllocationDto.ApplicationFormId);
                    if (applicationForm != null)
                    {
                        applicationForm.ApplicantStage = ApplicationStage.Stage9;
                        db.ApplicationForm.Update(applicationForm);
                    }

                    await db.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return _mapper.Map<HostelAllocation, HostelAllocationDto>(addedHostelAllocation.Entity);
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }
    }

public async Task<IEnumerable<HostelAllocationDto>> GetPendingAllocationsOlderThan(DateTime cutoffTime)
{
    using (var db = new ApplicationDbContext(dbOptions))
    {
        return await db.HostelAllocation
            .Where(a => a.CurrentStatus == AllocationStatus.Pending && 
                        a.DateAllocated < cutoffTime)
            .Select(a => _mapper.Map<HostelAllocationDto>(a))
            .ToListAsync();
    }
}



    public async Task<IEnumerable<HostelAllocationDto>> GetAllocationsByApplicationFormId(long applicationFormId)
{
    try
    {
        using (var db = new ApplicationDbContext(dbOptions))
        {
            var hostelAllocations = await db.HostelAllocation
                .Where(x => x.ApplicationFormId == applicationFormId)
                .Include(x => x.HostelRoom)
                    .ThenInclude(hr => hr.HostelBuilding)
                .Include(x => x.Student)
                .Include(x => x.ApplicationForm)
                .ToListAsync();

            return _mapper.Map<IEnumerable<HostelAllocation>, IEnumerable<HostelAllocationDto>>(hostelAllocations);
        }
    }
    catch (Exception ex)
    {
        // Log the exception
        Console.WriteLine($"Error in GetAllocationsByApplicationFormId: {ex.Message}");
        return Enumerable.Empty<HostelAllocationDto>();
    }
}

        public async Task<HostelAllocationDto> UpdateHostelAllocation(HostelAllocationDto HostelAllocationDto)
        {
            try
            {
                using (var db = new ApplicationDbContext(dbOptions))
                {
                    {

                        HostelAllocation HostelAllocation = db.HostelAllocation.FirstOrDefault(x => x.Id == HostelAllocationDto.Id);
                        if (HostelAllocation == null)
                        {
                            return null;
                        }
                        else
                        {
                            HostelAllocation HostelAllocationUpdate = _mapper.Map<HostelAllocationDto, HostelAllocation>(HostelAllocationDto, HostelAllocation);
                            var currentupdate = db.HostelAllocation.Update(HostelAllocationUpdate);
                            await db.SaveChangesAsync();
                            return _mapper.Map<HostelAllocation, HostelAllocationDto>(currentupdate.Entity);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}