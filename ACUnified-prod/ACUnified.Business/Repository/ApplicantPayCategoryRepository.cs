using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACUnified.Business.Repository
{
    using AutoMapper;

    using Microsoft.EntityFrameworkCore;
    using System.Linq;
    using ACUnified.Data.Enum;
    using ACUnified.Data.Models;

    using ACUnified.Data.DTOs;

    public class ApplicantPayCategoryRepository : IApplicantPayCategoryRepository
    {
        private readonly DbContextOptions<ApplicationDbContext> dbOptions;
        private readonly IMapper _mapper;
        public ApplicantPayCategoryRepository(DbContextOptions<ApplicationDbContext> options, IMapper mapper)
        {
            _mapper = mapper;
            dbOptions = options;
        }

        public async Task<ApplicantPayCategoryDto> CreateApplicantPayCategory(ApplicantPayCategoryDto ApplicantPayCategoryDto)
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {

                ApplicantPayCategory pay = _mapper.Map<ApplicantPayCategoryDto, ApplicantPayCategory>(ApplicantPayCategoryDto);
                var addedCategory = db.ApplicantPayCategory.Add(pay);
                await db.SaveChangesAsync();
                return _mapper.Map<ApplicantPayCategory, ApplicantPayCategoryDto>(addedCategory.Entity);
            }

        }

        public async Task<IEnumerable<ApplicantPayCategoryDto>> GetAllApplicantPayCategory()
        {
            try
            {
                using (var db = new ApplicationDbContext(dbOptions))
                {
                    IEnumerable<ApplicantPayCategoryDto> ApplicantPayCategoryDtos =
                        _mapper.Map<IEnumerable<ApplicantPayCategory>, IEnumerable<ApplicantPayCategoryDto>>(db.ApplicantPayCategory);
                    //var categoryNames = ApplicantPayCategoryDtos.Select(x => x.Name).ToList();
                    //db.ApplicantPayCategory.Where(x => categoryNames.Contains(x.Name));
                    return ApplicantPayCategoryDtos;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<bool> ApplicantPayCategoryExists(ApplicantPayCategoryDto ApplicantPayCategoryDto)
        {

            if (ApplicantPayCategoryDto == null) return false;
            try
            {
                using (var db = new ApplicationDbContext(dbOptions))
                {
                    return await db.ApplicantPayCategory.AnyAsync(f => f.Name == ApplicantPayCategoryDto.Name);
                }
            }
            catch (Exception ex)
            {

                return false;
            }
        }

        public async Task<ApplicantPayCategoryDto> GetApplicantPayCategory(long Id)
        {
            try
            {
                using (var db = new ApplicationDbContext(dbOptions))
                {
                    ApplicantPayCategoryDto ApplicantPayCategoryDto =
                        _mapper.Map<ApplicantPayCategory, ApplicantPayCategoryDto>(
                            await db.ApplicantPayCategory.FirstOrDefaultAsync(x => x.Id == Id));
                    return ApplicantPayCategoryDto;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<IEnumerable<ApplicantPayCategoryDto>> GetStudentApplicantPayCategory(long levelId, long departmentId, long sessionId, long degreeId)
        {
            try
            {
                using (var db = new ApplicationDbContext(dbOptions))
                {
                    var payCategories = await db.ApplicantPayCategory
                        .Where(x => x.StudentLevelId == levelId && x.DepartmentId == departmentId)
                        .ToListAsync();

                    if (!payCategories.Any())
                    {
                        // Optionally handle the case where no matching pay categories are found
                        return Enumerable.Empty<ApplicantPayCategoryDto>();
                    }

                    var ApplicantPayCategoryDtos = _mapper.Map<IEnumerable<ApplicantPayCategory>, IEnumerable<ApplicantPayCategoryDto>>(payCategories);
                    return ApplicantPayCategoryDtos;
                }
            }
            catch (Exception ex)
            {
                // Log the exception here for debugging purposes
                return Enumerable.Empty<ApplicantPayCategoryDto>();
            }
        }

        public async Task<IEnumerable<ApplicantPayCategoryDto>> GetAllApplicantPayCategory(long levelId, long departmentId, long sessionId, long degreeId)
        {
            try
            {
                using (var db = new ApplicationDbContext(dbOptions))
                {
                    var payCategories = await db.ApplicantPayCategory
                        .Where(x => x.StudentLevelId == levelId && x.DepartmentId == departmentId)
                        .ToListAsync();

                    if (!payCategories.Any())
                    {
                        // Optionally handle the case where no matching pay categories are found
                        return Enumerable.Empty<ApplicantPayCategoryDto>();
                    }

                    var ApplicantPayCategoryDtos = _mapper.Map<IEnumerable<ApplicantPayCategory>, IEnumerable<ApplicantPayCategoryDto>>(payCategories);
                    return ApplicantPayCategoryDtos;
                }
            }
            catch (Exception ex)
            {
                // Log the exception here for debugging purposes
                return Enumerable.Empty<ApplicantPayCategoryDto>();
            }
        }




        public async Task DeleteApplicantPayCategory(long Id)
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                var faculty = db.Faculty.FirstOrDefault(x => x.Id == Id);
                if (faculty != null)
                {
                    db.Faculty.Remove(faculty);
                    db.SaveChanges();
                }
            }
        }

        public async Task<ApplicantPayCategoryDto> UpdateApplicantPayCategory(ApplicantPayCategoryDto ApplicantPayCategoryDto)
        {
            try
            {
                using (var db = new ApplicationDbContext(dbOptions))
                {
                    Faculty faculty = db.Faculty.FirstOrDefault(x => x.Id == ApplicantPayCategoryDto.Id);
                    if (faculty == null)
                    {
                        return null;
                    }
                    else
                    {
                        Faculty facultyUpdate = _mapper.Map<ApplicantPayCategoryDto, Faculty>(ApplicantPayCategoryDto, faculty);
                        var currentupdate = db.Faculty.Update(facultyUpdate);
                        await db.SaveChangesAsync();
                        return _mapper.Map<Faculty, ApplicantPayCategoryDto>(currentupdate.Entity);
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
