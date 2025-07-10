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
    public class PayUploadCategoryBatchRepository : IPayUploadCategoryBatchRepository
    {
        private readonly IMapper _mapper;
        private readonly DbContextOptions<ApplicationDbContext> dbOptions;


        public PayUploadCategoryBatchRepository(IMapper mapper, DbContextOptions<ApplicationDbContext> options)
        {
            _mapper = mapper;

            dbOptions = options;
        }
        public async Task<PayUploadCategoryBatchDto> CreatePayUploadCategoryBatch(PayUploadCategoryBatchDto PayUploadCategoryBatchDto)
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                PayUploadCategoryBatch PayUploadCategoryBatch = _mapper.Map<PayUploadCategoryBatchDto, PayUploadCategoryBatch>(PayUploadCategoryBatchDto);
                var addedPayUploadCategoryBatch = db.PayUploadCategoryBatch.Add(PayUploadCategoryBatch);
                await db.SaveChangesAsync();
                return _mapper.Map<PayUploadCategoryBatch, PayUploadCategoryBatchDto>(addedPayUploadCategoryBatch.Entity);
            }
        }
       
        public async Task CreatePayUploadCategoryBatch(IEnumerable<PayUploadCategoryBatchDto> PayUploadCategoryBatchDto)
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                IEnumerable<PayUploadCategoryBatch> PayUploadCategoryBatch = _mapper.Map<IEnumerable<PayUploadCategoryBatchDto>, IEnumerable<PayUploadCategoryBatch>>(PayUploadCategoryBatchDto);
                 db.PayUploadCategoryBatch.AddRange(PayUploadCategoryBatch);
                await db.SaveChangesAsync();
                //return _mapper.Map<IEnumerable<PayUploadCategoryBatch>, IEnumerable<PayUploadCategoryBatchDto>>(addedPayUploadCategoryBatch.Entity);
            }
        }


        public async Task<IEnumerable<PayUploadCategoryBatchDto>> GetAllPayUploadCategoryBatch()
        {
            try
            {
                using (var db = new ApplicationDbContext(dbOptions))
                {
                    var data = db.PayUploadCategoryBatch.ToList(); // Fetch data from the database

                    if (data != null && data.Any())
                    {
                        // Use AutoMapper to map the retrieved data to DTOs
                        var payUploadCategoryBatchDtos = _mapper.Map<IEnumerable<PayUploadCategoryBatch>, IEnumerable<PayUploadCategoryBatchDto>>(data);
                        return payUploadCategoryBatchDtos;
                    }
                    else
                    {
                        // Return an empty collection if no data is found
                        return Enumerable.Empty<PayUploadCategoryBatchDto>();
                    }



                }


            }
            catch (Exception ex)
            {
                return Enumerable.Empty<PayUploadCategoryBatchDto>();
            }
        }

        public async Task<bool> PayUploadCategoryBatchExists(PayUploadCategoryBatchDto PayUploadCategoryBatchDto)
        {
            if (PayUploadCategoryBatchDto == null) return false;
            try
            {
                using (var db = new ApplicationDbContext(dbOptions))
                {
                    return await db.PayUploadCategoryBatch.AnyAsync(f => f.Name == PayUploadCategoryBatchDto.Name);
                }
            }
            catch (Exception ex)
            {

                return false;
            }
        }

        public async Task<PayUploadCategoryBatchDto> GetPayUploadCategoryBatch(long Id)
        {
            try
            {
                using (var db = new ApplicationDbContext(dbOptions))
                {
                    PayUploadCategoryBatchDto PayUploadCategoryBatchDto =
                        _mapper.Map<PayUploadCategoryBatch, PayUploadCategoryBatchDto>(await db.PayUploadCategoryBatch.FirstOrDefaultAsync(x => x.Id == Id));
                    return PayUploadCategoryBatchDto;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task DeletePayUploadCategoryBatch(long Id)
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                var PayUploadCategoryBatch = db.PayUploadCategoryBatch.FirstOrDefault(x => x.Id == Id);
                if (PayUploadCategoryBatch != null)
                {
                    db.PayUploadCategoryBatch.Remove(PayUploadCategoryBatch);
                    db.SaveChanges();
                }
            }
        }

        public async Task<PayUploadCategoryBatchDto> UpdatePayUploadCategoryBatch(PayUploadCategoryBatchDto PayUploadCategoryBatchDto)
        {
            try
            {
                using (var db = new ApplicationDbContext(dbOptions))
                {
                    PayUploadCategoryBatch PayUploadCategoryBatch = db.PayUploadCategoryBatch.FirstOrDefault(x => x.Id == PayUploadCategoryBatchDto.Id);
                    if (PayUploadCategoryBatch == null)
                    {
                        return null;
                    }
                    else
                    {
                        PayUploadCategoryBatch PayUploadCategoryBatchUpdate = _mapper.Map<PayUploadCategoryBatchDto, PayUploadCategoryBatch>(PayUploadCategoryBatchDto, PayUploadCategoryBatch);
                        var currentupdate = db.PayUploadCategoryBatch.Update(PayUploadCategoryBatchUpdate);
                        await db.SaveChangesAsync();
                        return _mapper.Map<PayUploadCategoryBatch, PayUploadCategoryBatchDto>(currentupdate.Entity);
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
