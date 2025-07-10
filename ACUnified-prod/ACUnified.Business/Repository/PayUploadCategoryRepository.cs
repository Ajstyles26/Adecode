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
    public class PayUploadCategoryRepository : IPayUploadCategoryRepository
    {
        private readonly IMapper _mapper;
        private readonly DbContextOptions<ApplicationDbContext> dbOptions;

                
        public PayUploadCategoryRepository(IMapper mapper, DbContextOptions<ApplicationDbContext> options)
        {
            _mapper = mapper;

            dbOptions = options;
        }
        public async Task<PayUploadCategoryDto> CreatePayUploadCategory(PayUploadCategoryDto PayUploadCategoryDto)
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                PayUploadCategory PayUploadCategory = _mapper.Map<PayUploadCategoryDto, PayUploadCategory>(PayUploadCategoryDto);
                var addedPayUploadCategory = db.PayUploadCategory.Add(PayUploadCategory);
                await db.SaveChangesAsync();
                return _mapper.Map<PayUploadCategory, PayUploadCategoryDto>(addedPayUploadCategory.Entity);
            }
        }

        public async Task CreatePayUploadCategory(IEnumerable<PayUploadCategoryDto> PayUploadCategoryDto)
        {
            try
            {
                using (var db = new ApplicationDbContext(dbOptions))
                {
                    IEnumerable<PayUploadCategory> PayUploadCategory = _mapper.Map<IEnumerable<PayUploadCategoryDto>, IEnumerable<PayUploadCategory>>(PayUploadCategoryDto);
                    db.PayUploadCategory.AddRange(PayUploadCategory);
                    await db.SaveChangesAsync();

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                
            }
            
        }


        public async Task<IEnumerable<PayUploadCategoryDto>> GetAllPayUploadCategory()
        {
            try
            {
                using (var db = new ApplicationDbContext(dbOptions))
                {
                    IEnumerable<PayUploadCategoryDto> PayUploadCategoryDtos = _mapper.Map<IEnumerable<PayUploadCategory>, IEnumerable<PayUploadCategoryDto>>(db.PayUploadCategory);
                    //var PayUploadCategoryNames = PayUploadCategoryDtos.Select(x => x.Name).ToList();
                    //db.PayUploadCategory.Where(x => PayUploadCategoryNames.Contains(x.Name));
                    return PayUploadCategoryDtos;
                }


            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<IEnumerable<PayUploadCategoryDto>> GetPayUploadCategoryByBatchId(long id)
        {
            try
            {
                using (var db = new ApplicationDbContext(dbOptions))
                {
                    IEnumerable<PayUploadCategoryDto> PayUploadCategoryDtos = _mapper.Map<IEnumerable<PayUploadCategory>, IEnumerable<PayUploadCategoryDto>>(db.PayUploadCategory.Where(x=>x.PayUploadCategoryBatchId==id));

                    return PayUploadCategoryDtos;
                }


            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<IEnumerable<PayUploadCategoryDto>> GetPayUploadCategoryByBDPL(long batchid,long degreeid, long programid,long levelid)
        {
            try
            {
                using (var db = new ApplicationDbContext(dbOptions))
                {
                    IEnumerable<PayUploadCategoryDto> PayUploadCategoryDtos = _mapper.Map<IEnumerable<PayUploadCategory>, IEnumerable<PayUploadCategoryDto>>(db.PayUploadCategory.Where(x => x.PayUploadCategoryBatchId == batchid&&
                    x.DegreeId==degreeid&&x.ProgramSetupId==programid&& x.StudentLevelId==levelid));

                    return PayUploadCategoryDtos;
                }


            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<bool> PayUploadCategoryExists(PayUploadCategoryDto PayUploadCategoryDto)
        {
            if (PayUploadCategoryDto == null) return false;
            try
            {
                using (var db = new ApplicationDbContext(dbOptions))
                {
                    return await db.PayUploadCategory.AnyAsync(f => f.Name == PayUploadCategoryDto.Name);
                }
            }
            catch (Exception ex)
            {

                return false;
            }
        }

        public async Task<PayUploadCategoryDto> GetPayUploadCategory(long Id)
        {
            try
            {
                using (var db = new ApplicationDbContext(dbOptions))
                {
                    PayUploadCategoryDto PayUploadCategoryDto =
                        _mapper.Map<PayUploadCategory, PayUploadCategoryDto>(await db.PayUploadCategory.FirstOrDefaultAsync(x => x.Id == Id));
                    return PayUploadCategoryDto;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<IEnumerable<PayUploadCategoryDto>> GetPayUploadCategoryBatch(long Id)
        {
            try
            {
                using (var db = new ApplicationDbContext(dbOptions))
                {
                    IEnumerable<PayUploadCategoryDto> payUploadCategoryDto = await db.PayUploadCategory
                    .Where(x => x.PayUploadCategoryBatchId == Id)
                    .Select(x => _mapper.Map<PayUploadCategoryDto>(x))
                    .ToListAsync();

                    return payUploadCategoryDto;
                }
            }
            catch (Exception ex)
            {
               return Enumerable.Empty<PayUploadCategoryDto>();
            }
        }

        public async Task DeletePayUploadCategory(long Id)
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                var PayUploadCategory = db.PayUploadCategory.FirstOrDefault(x => x.Id == Id);
                if (PayUploadCategory != null)
                {
                    db.PayUploadCategory.Remove(PayUploadCategory);
                    db.SaveChanges();
                }
            }
        }

        public async Task<PayUploadCategoryDto> UpdatePayUploadCategory(PayUploadCategoryDto PayUploadCategoryDto)
        {
            try
            {
                using (var db = new ApplicationDbContext(dbOptions))
                {
                    PayUploadCategory PayUploadCategory = db.PayUploadCategory.FirstOrDefault(x => x.Id == PayUploadCategoryDto.Id);
                    if (PayUploadCategory == null)
                    {
                        return null;
                    }
                    else
                    {
                        PayUploadCategory PayUploadCategoryUpdate = _mapper.Map<PayUploadCategoryDto, PayUploadCategory>(PayUploadCategoryDto, PayUploadCategory);
                        var currentupdate = db.PayUploadCategory.Update(PayUploadCategoryUpdate);
                        await db.SaveChangesAsync();
                        return _mapper.Map<PayUploadCategory, PayUploadCategoryDto>(currentupdate.Entity);
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
