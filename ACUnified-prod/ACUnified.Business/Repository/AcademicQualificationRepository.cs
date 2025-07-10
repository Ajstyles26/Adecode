using ACUnified.Data.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ACUnified.Data.DTOs;


namespace ACUnified.Business.Repository
{
    public class AcademicQualificationRepository : IAcademicQualificationRepository
    {
      
        private readonly IMapper _mapper;
        private readonly DbContextOptions<ApplicationDbContext> dbOptions;
        public AcademicQualificationRepository(DbContextOptions<ApplicationDbContext> options, IMapper mapper)
        {
            _mapper = mapper;
            dbOptions = options;
        }
    
        public async Task<AcademicQualificationDto> CreateAcademicQualification(AcademicQualificationDto academicqualificationDto)
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                AcademicQualification academicqualification = _mapper.Map<AcademicQualificationDto, AcademicQualification>(academicqualificationDto);
                var addedAcademicQualification = db.AcademicQualification.Add(academicqualification);
                await db.SaveChangesAsync();
                return _mapper.Map<AcademicQualification, AcademicQualificationDto>(addedAcademicQualification.Entity);
            }
        }
    
        public async Task<IEnumerable<AcademicQualificationDto>> GetAllAcademicQualification()
        {
            try
            {
                using (var db = new ApplicationDbContext(dbOptions))
                {
                    IEnumerable<AcademicQualificationDto> academicqualificationDtos =
                        _mapper.Map<IEnumerable<AcademicQualification>, IEnumerable<AcademicQualificationDto>>(db.AcademicQualification);
                    return academicqualificationDtos;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<AcademicQualificationDto> GetAcademicQualification(string userid)
        {
            try
            {
                using (var db = new ApplicationDbContext(dbOptions))
                {
                    AcademicQualificationDto academicqualificationDto =
                        _mapper.Map<AcademicQualification, AcademicQualificationDto>(await db.AcademicQualification.FirstOrDefaultAsync(x => x.UserId == userid));
                    //if (academicqualificationDto == null) { return null; }
                    return academicqualificationDto;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task DeleteAcademicQualification(long Id)
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                var academicqualification = db.AcademicQualification.FirstOrDefault(x => x.Id == Id);
                if (academicqualification != null)
                {
                    db.AcademicQualification.Remove(academicqualification);
                    db.SaveChanges();
                }
            }
        }

        public async Task<AcademicQualificationDto> UpdateAcademicQualification(AcademicQualificationDto academicqualificationDto)
        {
            try
            {
                using (var db = new ApplicationDbContext(dbOptions))
                {
                    var academicqualification = await db.AcademicQualification.FirstOrDefaultAsync(x => x.UserId == academicqualificationDto.UserId);
                    if (academicqualification == null)
                    {
                        return await CreateAcademicQualification(academicqualificationDto);
                    }
                    else
                    {
                        _mapper.Map(academicqualificationDto, academicqualification);
                        db.AcademicQualification.Update(academicqualification);
                        await db.SaveChangesAsync();
                        return _mapper.Map<AcademicQualificationDto>(academicqualification);
                    }
                }
            }
            catch (Exception ex)
            {
                 Console.WriteLine($"Error updating biodata for user {academicqualificationDto.UserId}: {ex.Message}");
                return null;
            }
        }

       

       

      

       
    }

}
