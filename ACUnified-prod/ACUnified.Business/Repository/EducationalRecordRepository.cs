using ACUnified.Data.Models;
using ACUnified.Data.DTOs;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ACUnified.Business.Repository
{
    public class EducationalRecordRepository : IEducationalRecordRepository
    {
        private readonly IMapper _mapper;
        private readonly DbContextOptions<ApplicationDbContext> dbOptions;
               private readonly ILogger<EducationalRecordRepository> _logger;

        public EducationalRecordRepository(DbContextOptions<ApplicationDbContext> options, IMapper mapper)
        {
            _mapper = mapper;
            dbOptions = options;
        }

        public async Task<IEnumerable<EducationalRecordDto>> GetAllEducationalRecord()
        {
            try
            {
                using (var db = new ApplicationDbContext(dbOptions))
                {
                    var records = await db.EducationalRecord.ToListAsync();
                    return _mapper.Map<IEnumerable<EducationalRecord>, IEnumerable<EducationalRecordDto>>(records);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in GetAllEducationalRecord: {ex.Message}");
                return Enumerable.Empty<EducationalRecordDto>();
            }
        }

        public async Task<IEnumerable<EducationalRecordDto>> GetEducationalRecordsByUserId(string userId)
        {
            try
            {
                using (var db = new ApplicationDbContext(dbOptions))
                {
                    var records = await db.EducationalRecord
                        .Where(x => x.UserId == userId)
                        .ToListAsync();
                    return _mapper.Map<IEnumerable<EducationalRecord>, IEnumerable<EducationalRecordDto>>(records);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in GetEducationalRecordsByUserId: {ex.Message}");
                return Enumerable.Empty<EducationalRecordDto>();
            }
        }

        public async Task<EducationalRecordDto> GetEducationalRecord(string userid)
        {
            try
            {
                using (var db = new ApplicationDbContext(dbOptions))
                {
                    EducationalRecordDto educationalrecordDto =
                        _mapper.Map<EducationalRecord, EducationalRecordDto>(await db.EducationalRecord.FirstOrDefaultAsync(x => x.UserId == userid));
                    return educationalrecordDto;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in GetEducationalRecord: {ex.Message}");
                return null;
            }
        }

        public async Task DeleteEducationalRecord(long Id)
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                var educationalrecord = await db.EducationalRecord.FirstOrDefaultAsync(x => x.Id == Id);
                if (educationalrecord != null)
                {
                    db.EducationalRecord.Remove(educationalrecord);
                    await db.SaveChangesAsync();
                }
            }
        }

        public async Task<EducationalRecordDto> CreateEducationalRecord(EducationalRecordDto educationalrecordDto)
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                try
                {
                    EducationalRecord educationalrecord = _mapper.Map<EducationalRecordDto, EducationalRecord>(educationalrecordDto);
                    var addedEducationalRecord = await db.EducationalRecord.AddAsync(educationalrecord);
                    await db.SaveChangesAsync();
                    return _mapper.Map<EducationalRecord, EducationalRecordDto>(addedEducationalRecord.Entity);
                }
                catch (DbUpdateException dbEx)
                {
                    Console.WriteLine($"DbUpdateException in CreateEducationalRecord: {dbEx.Message}");
                    Console.WriteLine($"Inner Exception: {dbEx.InnerException?.Message}");
                    throw;
                }
            }
        }
        
        public async Task<IEnumerable<EducationalRecordDto>> GetEducationalRecordsByApplicationFormId(long applicationFormId)
        {
            try
            {
                using (var db = new ApplicationDbContext(dbOptions))
                {
                    var educationalRecords = await db.EducationalRecord
                        .Where(x => x.ApplicationFormId == applicationFormId)
                        .Select(er => new EducationalRecordDto
                        {
                            Id = er.Id,
                            Name = er.Name ?? string.Empty,
                            Qualification = er.Qualification ?? string.Empty,
                            Grade = er.Grade ?? string.Empty,
                            StartDate = er.StartDate,
                            EndDate = er.EndDate,
                            DocumentPath = er.DocumentPath ?? string.Empty,
                            UserId = er.UserId ?? string.Empty,
                            ApplicationFormId = er.ApplicationFormId
                        })
                        .AsNoTracking()
                        .ToListAsync();

                    _logger.LogInformation($"Retrieved {educationalRecords.Count} educational records for application form ID {applicationFormId}");
                    return educationalRecords;
                }
            }
            catch (InvalidCastException ex)
            {
                _logger.LogError(ex, $"Invalid cast exception when retrieving educational records for application form ID {applicationFormId}. This might be due to NULL values in the database.");
                return Enumerable.Empty<EducationalRecordDto>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving educational records for application form ID {applicationFormId}");
                return Enumerable.Empty<EducationalRecordDto>();
            }
        }


        public async Task<EducationalRecordDto> UpdateEducationalRecord(EducationalRecordDto educationalrecordDto)
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                try
                {
                    var educationalrecord = await db.EducationalRecord.FirstOrDefaultAsync(x => x.Id == educationalrecordDto.Id);
                    if (educationalrecord == null)
                    {
                        return await CreateEducationalRecord(educationalrecordDto);
                    }
                    else
                    {
                        _mapper.Map(educationalrecordDto, educationalrecord);
                        db.EducationalRecord.Update(educationalrecord);
                        await db.SaveChangesAsync();
                        return _mapper.Map<EducationalRecordDto>(educationalrecord);
                    }
                }
                catch (DbUpdateException dbEx)
                {
                    Console.WriteLine($"DbUpdateException in UpdateEducationalRecord: {dbEx.Message}");
                    Console.WriteLine($"Inner Exception: {dbEx.InnerException?.Message}");
                    throw;
                }
            }
        }
    }
}