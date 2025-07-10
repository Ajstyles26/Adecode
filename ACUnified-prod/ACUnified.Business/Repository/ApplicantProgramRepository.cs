using ACUnified.Data.DTOs;
using ACUnified.Data.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;



namespace ACUnified.Business.Repository
{
    public class ApplicationProgramRepository : IApplicationProgramRepository
    {
        private readonly DbContextOptions<ApplicationDbContext> dbOptions;
        private readonly IMapper _mapper;
        public ApplicationProgramRepository(DbContextOptions<ApplicationDbContext> options, IMapper mapper)
        {
            _mapper = mapper;
            dbOptions = options;
        }

        public async Task<ApplicationProgramDto> CreateApplicationProgram(ApplicationProgramDto ApplicationProgramDto)
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                ApplicationProgram ApplicationProgram = _mapper.Map<ApplicationProgramDto, ApplicationProgram>(ApplicationProgramDto);
                var addedApplicationProgram = db.ApplicationProgram.Add(ApplicationProgram);
                await db.SaveChangesAsync();
                return _mapper.Map<ApplicationProgram, ApplicationProgramDto>(addedApplicationProgram.Entity);
            }
        }

        public async Task<IEnumerable<ApplicationProgramDto>> GetAllApplicationProgram()
        {
            try
            {
                using (var db = new ApplicationDbContext(dbOptions))
                {
                    IEnumerable<ApplicationProgramDto> ApplicationProgramDtos =
                        _mapper.Map<IEnumerable<ApplicationProgram>, IEnumerable<ApplicationProgramDto>>(db.ApplicationProgram);
                    return ApplicationProgramDtos;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<ApplicationProgramDto> GetApplicationProgram(long Id)
        {
            try
            {
                using (var db = new ApplicationDbContext(dbOptions))
                {
                    ApplicationProgramDto ApplicationProgramDto =
                        _mapper.Map<ApplicationProgram, ApplicationProgramDto>(await db.ApplicationProgram.FirstOrDefaultAsync(x => x.Id == Id));
                    return ApplicationProgramDto;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task DeleteApplicationProgram(long Id)
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                var ApplicationProgram = db.ApplicationProgram.FirstOrDefault(x => x.Id == Id);
                if (ApplicationProgram != null)
                {
                    db.ApplicationProgram.Remove(ApplicationProgram);
                    db.SaveChanges();
                }


            }
        }

        public async Task<ApplicationProgramDto> UpdateApplicationProgram(ApplicationProgramDto ApplicationProgramDto)
        {
            try
            {
                using (var db = new ApplicationDbContext(dbOptions))
                {
                    {

                        ApplicationProgram ApplicationProgram = db.ApplicationProgram.FirstOrDefault(x => x.Id == ApplicationProgramDto.Id);
                        if (ApplicationProgram == null)
                        {
                            return null;
                        }
                        else
                        {
                            ApplicationProgram ApplicationProgramUpdate = _mapper.Map<ApplicationProgramDto, ApplicationProgram>(ApplicationProgramDto, ApplicationProgram);
                            var currentupdate = db.ApplicationProgram.Update(ApplicationProgramUpdate);
                            await db.SaveChangesAsync();
                            return _mapper.Map<ApplicationProgram, ApplicationProgramDto>(currentupdate.Entity);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public Task DeleteProgram(long Id)
        {
            throw new NotImplementedException();
        }
    }
}
