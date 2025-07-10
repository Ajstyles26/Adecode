using ACUnified.Business.Repository.IRepository;
using ACUnified.Data.DTOs;
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
    public class HostelBuildingRepository : IHostelBuildingRepository
    {
        private readonly DbContextOptions<ApplicationDbContext> dbOptions;
        private readonly IMapper _mapper;
        public HostelBuildingRepository(DbContextOptions<ApplicationDbContext> options, IMapper mapper)
        {
            _mapper = mapper;
            dbOptions = options;
        }

        public async Task<HostelBuildingDto> CreateHostelBuilding(HostelBuildingDto HostelBuildingDto)
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                HostelBuilding HostelBuilding = _mapper.Map<HostelBuildingDto, HostelBuilding>(HostelBuildingDto);
                var addedHostelBuilding = db.HostelBuilding.Add(HostelBuilding);
                await db.SaveChangesAsync();
                return _mapper.Map<HostelBuilding, HostelBuildingDto>(addedHostelBuilding.Entity);
            }
        }

        public async Task<IEnumerable<HostelBuildingDto>> GetAllHostelBuilding()
        {
            try
            {
                using (var db = new ApplicationDbContext(dbOptions))
                {
                    IEnumerable<HostelBuildingDto> HostelBuildingDtos =
                        _mapper.Map<IEnumerable<HostelBuilding>, IEnumerable<HostelBuildingDto>>(db.HostelBuilding);
                    return HostelBuildingDtos;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<HostelBuildingDto> GetHostelBuilding(long Id)
        {
            try
            {
                using (var db = new ApplicationDbContext(dbOptions))
                {
                    HostelBuildingDto HostelBuildingDto =
                        _mapper.Map<HostelBuilding, HostelBuildingDto>(await db.HostelBuilding.FirstOrDefaultAsync(x => x.Id == Id));
                    return HostelBuildingDto;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
public async Task<IEnumerable<HostelBuildingDto>> GetHostelsByGender(string gender)
{
    try
    {
        using (var db = new ApplicationDbContext(dbOptions))
        {
            if (string.IsNullOrEmpty(gender) || (gender.ToLower() != "male" && gender.ToLower() != "female"))
            {
                throw new ArgumentException("Gender must be either 'male' or 'female'");
            }

            IEnumerable<HostelBuildingDto> hostelBuildingDtos =
                _mapper.Map<IEnumerable<HostelBuilding>, IEnumerable<HostelBuildingDto>>(
                    await db.HostelBuilding.Where(h => h.Gender.ToLower() == gender.ToLower()).ToListAsync());

            return hostelBuildingDtos;
        }
    }
    catch (Exception ex)
    {
        // Log the exception
        return null;
    }
}
        public async Task DeleteHostelBuilding(long Id)
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                var HostelBuilding = db.HostelBuilding.FirstOrDefault(x => x.Id == Id);
                if (HostelBuilding != null)
                {
                    db.HostelBuilding.Remove(HostelBuilding);
                    db.SaveChanges();
                }


            }
        }

        public async Task<HostelBuildingDto> UpdateHostelBuilding(HostelBuildingDto HostelBuildingDto)
        {
            try
            {
                using (var db = new ApplicationDbContext(dbOptions))
                {
                    {

                        HostelBuilding HostelBuilding = db.HostelBuilding.FirstOrDefault(x => x.Id == HostelBuildingDto.Id);
                        if (HostelBuilding == null)
                        {
                            return null;
                        }
                        else
                        {
                            HostelBuilding HostelBuildingUpdate = _mapper.Map<HostelBuildingDto, HostelBuilding>(HostelBuildingDto, HostelBuilding);
                            var currentupdate = db.HostelBuilding.Update(HostelBuildingUpdate);
                            await db.SaveChangesAsync();
                            return _mapper.Map<HostelBuilding, HostelBuildingDto>(currentupdate.Entity);
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