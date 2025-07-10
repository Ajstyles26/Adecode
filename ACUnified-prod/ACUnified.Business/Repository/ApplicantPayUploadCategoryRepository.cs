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
    public class ApplicantPayUploadCategoryRepository : IApplicantPayUploadCategoryRepository
    {
        private readonly IMapper _mapper;
        private readonly DbContextOptions<ApplicationDbContext> dbOptions;

                
        public ApplicantPayUploadCategoryRepository(IMapper mapper, DbContextOptions<ApplicationDbContext> options)
        {
            _mapper = mapper;

            dbOptions = options;
        }
        public async Task<ApplicantPayUploadCategoryDto> CreatePayUploadCategory(ApplicantPayUploadCategoryDto ApplicantPayUploadCategoryDto)
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                ApplicantPayUploadCategory ApplicantPayUploadCategory = _mapper.Map<ApplicantPayUploadCategoryDto, ApplicantPayUploadCategory>(ApplicantPayUploadCategoryDto);
                var addedPayUploadCategory = db.ApplicantPayUploadCategory.Add(ApplicantPayUploadCategory);
                await db.SaveChangesAsync();
                return _mapper.Map<ApplicantPayUploadCategory, ApplicantPayUploadCategoryDto>(addedPayUploadCategory.Entity);
            }
        }

        public async Task CreatePayUploadCategory(IEnumerable<ApplicantPayUploadCategoryDto> ApplicantPayUploadCategoryDto)
        {
            try
            {
                using (var db = new ApplicationDbContext(dbOptions))
                {
                    IEnumerable<ApplicantPayUploadCategory> ApplicantPayUploadCategory = _mapper.Map<IEnumerable<ApplicantPayUploadCategoryDto>, IEnumerable<ApplicantPayUploadCategory>>(ApplicantPayUploadCategoryDto);
                    db.ApplicantPayUploadCategory.AddRange(ApplicantPayUploadCategory);
                    await db.SaveChangesAsync();

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                
            }
            
        }


        public async Task<IEnumerable<ApplicantPayUploadCategoryDto>> GetAllPayUploadCategory()
        {
            try
            {
                using (var db = new ApplicationDbContext(dbOptions))
                {
                    IEnumerable<ApplicantPayUploadCategoryDto> PayUploadCategoryDtos = _mapper.Map<IEnumerable<ApplicantPayUploadCategory>, IEnumerable<ApplicantPayUploadCategoryDto>>(db.ApplicantPayUploadCategory);
                    //var PayUploadCategoryNames = PayUploadCategoryDtos.Select(x => x.Name).ToList();
                    //db.ApplicantPayUploadCategory.Where(x => PayUploadCategoryNames.Contains(x.Name));
                    return PayUploadCategoryDtos;
                }


            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<IEnumerable<ApplicantPayUploadCategoryDto>> GetPayUploadCategoryByBatchId(long id)
        {
            try
            {
                using (var db = new ApplicationDbContext(dbOptions))
                {
                    IEnumerable<ApplicantPayUploadCategoryDto> PayUploadCategoryDtos = _mapper.Map<IEnumerable<ApplicantPayUploadCategory>, IEnumerable<ApplicantPayUploadCategoryDto>>(db.ApplicantPayUploadCategory.Where(x=>x.ApplicantPayUploadCategoryBatchId==id));

                    return PayUploadCategoryDtos;
                }


            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<IEnumerable<ApplicantPayUploadCategoryDto>> GetPayUploadCategoryByBDPL(long batchid,long degreeid, long programid,long levelid)
        {
            try
            {
                using (var db = new ApplicationDbContext(dbOptions))
                {
                    IEnumerable<ApplicantPayUploadCategoryDto> PayUploadCategoryDtos = _mapper.Map<IEnumerable<ApplicantPayUploadCategory>, IEnumerable<ApplicantPayUploadCategoryDto>>(db.ApplicantPayUploadCategory.Where(x => x.ApplicantPayUploadCategoryBatchId == batchid&&
                    x.DegreeId==degreeid&&x.ProgramSetupId==programid&& x.StudentLevelId==levelid));

                    return PayUploadCategoryDtos;
                }


            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<bool> PayUploadCategoryExists(ApplicantPayUploadCategoryDto ApplicantPayUploadCategoryDto)
        {
            if (ApplicantPayUploadCategoryDto == null) return false;
            try
            {
                using (var db = new ApplicationDbContext(dbOptions))
                {
                    return await db.ApplicantPayUploadCategory.AnyAsync(f => f.Name == ApplicantPayUploadCategoryDto.Name);
                }
            }
            catch (Exception ex)
            {

                return false;
            }
        }

        public async Task<ApplicantPayUploadCategoryDto> GetPayUploadCategory(long Id)
        {
            try
            {
                using (var db = new ApplicationDbContext(dbOptions))
                {
                    ApplicantPayUploadCategoryDto ApplicantPayUploadCategoryDto =
                        _mapper.Map<ApplicantPayUploadCategory, ApplicantPayUploadCategoryDto>(await db.ApplicantPayUploadCategory.FirstOrDefaultAsync(x => x.Id == Id));
                    return ApplicantPayUploadCategoryDto;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<IEnumerable<ApplicantPayUploadCategoryDto>> GetPayUploadCategoryBatch(long Id)
        {
            try
            {
                using (var db = new ApplicationDbContext(dbOptions))
                {
                    IEnumerable<ApplicantPayUploadCategoryDto> ApplicantPayUploadCategoryDto = await db.ApplicantPayUploadCategory
                    .Where(x => x.ApplicantPayUploadCategoryBatchId == Id)
                    .Select(x => _mapper.Map<ApplicantPayUploadCategoryDto>(x))
                    .ToListAsync();

                    return ApplicantPayUploadCategoryDto;
                }
            }
            catch (Exception ex)
            {
               return Enumerable.Empty<ApplicantPayUploadCategoryDto>();
            }
        }

        public async Task DeletePayUploadCategory(long Id)
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                var ApplicantPayUploadCategory = db.ApplicantPayUploadCategory.FirstOrDefault(x => x.Id == Id);
                if (ApplicantPayUploadCategory != null)
                {
                    db.ApplicantPayUploadCategory.Remove(ApplicantPayUploadCategory);
                    db.SaveChanges();
                }
            }
        }

        public async Task<ApplicantPayUploadCategoryDto> UpdatePayUploadCategory(ApplicantPayUploadCategoryDto ApplicantPayUploadCategoryDto)
        {
            try
            {
                using (var db = new ApplicationDbContext(dbOptions))
                {
                    ApplicantPayUploadCategory ApplicantPayUploadCategory = db.ApplicantPayUploadCategory.FirstOrDefault(x => x.Id == ApplicantPayUploadCategoryDto.Id);
                    if (ApplicantPayUploadCategory == null)
                    {
                        return null;
                    }
                    else
                    {
                        ApplicantPayUploadCategory PayUploadCategoryUpdate = _mapper.Map<ApplicantPayUploadCategoryDto, ApplicantPayUploadCategory>(ApplicantPayUploadCategoryDto, ApplicantPayUploadCategory);
                        var currentupdate = db.ApplicantPayUploadCategory.Update(PayUploadCategoryUpdate);
                        await db.SaveChangesAsync();
                        return _mapper.Map<ApplicantPayUploadCategory, ApplicantPayUploadCategoryDto>(currentupdate.Entity);
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
