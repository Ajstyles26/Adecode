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
    public class ApplicantPayUploadCategoryBatchRepository : IApplicantPayUploadCategoryBatchRepository
    {
        private readonly IMapper _mapper;
        private readonly DbContextOptions<ApplicationDbContext> dbOptions;


        public ApplicantPayUploadCategoryBatchRepository(IMapper mapper, DbContextOptions<ApplicationDbContext> options)
        {
            _mapper = mapper;

            dbOptions = options;
        }
        public async Task<ApplicantPayUploadCategoryBatchDto> CreatePayUploadCategoryBatch(ApplicantPayUploadCategoryBatchDto ApplicantPayUploadCategoryBatchDto)
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                ApplicantPayUploadCategoryBatch ApplicantPayUploadCategoryBatch = _mapper.Map<ApplicantPayUploadCategoryBatchDto, ApplicantPayUploadCategoryBatch>(ApplicantPayUploadCategoryBatchDto);
                var addedPayUploadCategoryBatch = db.ApplicantPayUploadCategoryBatch.Add(ApplicantPayUploadCategoryBatch);
                await db.SaveChangesAsync();
                return _mapper.Map<ApplicantPayUploadCategoryBatch, ApplicantPayUploadCategoryBatchDto>(addedPayUploadCategoryBatch.Entity);
            }
        }
       
        public async Task CreatePayUploadCategoryBatch(IEnumerable<ApplicantPayUploadCategoryBatchDto> ApplicantPayUploadCategoryBatchDto)
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                IEnumerable<ApplicantPayUploadCategoryBatch> ApplicantPayUploadCategoryBatch = _mapper.Map<IEnumerable<ApplicantPayUploadCategoryBatchDto>, IEnumerable<ApplicantPayUploadCategoryBatch>>(ApplicantPayUploadCategoryBatchDto);
                 db.ApplicantPayUploadCategoryBatch.AddRange(ApplicantPayUploadCategoryBatch);
                await db.SaveChangesAsync();
                //return _mapper.Map<IEnumerable<ApplicantPayUploadCategoryBatch>, IEnumerable<ApplicantPayUploadCategoryBatchDto>>(addedPayUploadCategoryBatch.Entity);
            }
        }


        public async Task<IEnumerable<ApplicantPayUploadCategoryBatchDto>> GetAllPayUploadCategoryBatch()
        {
            try
            {
                using (var db = new ApplicationDbContext(dbOptions))
                {
                    var data = db.ApplicantPayUploadCategoryBatch.ToList(); // Fetch data from the database

                    if (data != null && data.Any())
                    {
                        // Use AutoMapper to map the retrieved data to DTOs
                        var payUploadCategoryBatchDtos = _mapper.Map<IEnumerable<ApplicantPayUploadCategoryBatch>, IEnumerable<ApplicantPayUploadCategoryBatchDto>>(data);
                        return payUploadCategoryBatchDtos;
                    }
                    else
                    {
                        // Return an empty collection if no data is found
                        return Enumerable.Empty<ApplicantPayUploadCategoryBatchDto>();
                    }



                }


            }
            catch (Exception ex)
            {
                return Enumerable.Empty<ApplicantPayUploadCategoryBatchDto>();
            }
        }

        public async Task<bool> PayUploadCategoryBatchExists(ApplicantPayUploadCategoryBatchDto ApplicantPayUploadCategoryBatchDto)
        {
            if (ApplicantPayUploadCategoryBatchDto == null) return false;
            try
            {
                using (var db = new ApplicationDbContext(dbOptions))
                {
                    return await db.ApplicantPayUploadCategoryBatch.AnyAsync(f => f.Name == ApplicantPayUploadCategoryBatchDto.Name);
                }
            }
            catch (Exception ex)
            {

                return false;
            }
        }

        public async Task<ApplicantPayUploadCategoryBatchDto> GetPayUploadCategoryBatch(long Id)
        {
            try
            {
                using (var db = new ApplicationDbContext(dbOptions))
                {
                    ApplicantPayUploadCategoryBatchDto ApplicantPayUploadCategoryBatchDto =
                        _mapper.Map<ApplicantPayUploadCategoryBatch, ApplicantPayUploadCategoryBatchDto>(await db.ApplicantPayUploadCategoryBatch.FirstOrDefaultAsync(x => x.Id == Id));
                    return ApplicantPayUploadCategoryBatchDto;
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
                var ApplicantPayUploadCategoryBatch = db.ApplicantPayUploadCategoryBatch.FirstOrDefault(x => x.Id == Id);
                if (ApplicantPayUploadCategoryBatch != null)
                {
                    db.ApplicantPayUploadCategoryBatch.Remove(ApplicantPayUploadCategoryBatch);
                    db.SaveChanges();
                }
            }
        }

        public async Task<ApplicantPayUploadCategoryBatchDto> UpdatePayUploadCategoryBatch(ApplicantPayUploadCategoryBatchDto ApplicantPayUploadCategoryBatchDto)
        {
            try
            {
                using (var db = new ApplicationDbContext(dbOptions))
                {
                    ApplicantPayUploadCategoryBatch ApplicantPayUploadCategoryBatch = db.ApplicantPayUploadCategoryBatch.FirstOrDefault(x => x.Id == ApplicantPayUploadCategoryBatchDto.Id);
                    if (ApplicantPayUploadCategoryBatch == null)
                    {
                        return null;
                    }
                    else
                    {
                        ApplicantPayUploadCategoryBatch PayUploadCategoryBatchUpdate = _mapper.Map<ApplicantPayUploadCategoryBatchDto, ApplicantPayUploadCategoryBatch>(ApplicantPayUploadCategoryBatchDto, ApplicantPayUploadCategoryBatch);
                        var currentupdate = db.ApplicantPayUploadCategoryBatch.Update(PayUploadCategoryBatchUpdate);
                        await db.SaveChangesAsync();
                        return _mapper.Map<ApplicantPayUploadCategoryBatch, ApplicantPayUploadCategoryBatchDto>(currentupdate.Entity);
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
