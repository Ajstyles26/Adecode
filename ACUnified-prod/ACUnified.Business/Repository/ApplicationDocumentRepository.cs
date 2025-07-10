using ACUnified.Business.Repository.IRepository;
using ACUnified.Data.DTOs;
using ACUnified.Data.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ACUnified.Data.Enum;
namespace ACUnified.Business.Repository
{
    public class ApplicationDocumentRepository : IApplicationDocumentRepository
    {
        private readonly DbContextOptions<ApplicationDbContext> dbOptions;
        private readonly IMapper _mapper;
        public ApplicationDocumentRepository(DbContextOptions<ApplicationDbContext> options, IMapper mapper)
        {
            _mapper = mapper;
            dbOptions = options;
        }

        public async Task<ApplicationDocumentDto> CreateApplicationDocument(ApplicationDocumentDto ApplicationDocumentDto)
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                ApplicationDocument ApplicationDocument = _mapper.Map<ApplicationDocumentDto, ApplicationDocument>(ApplicationDocumentDto);
                var addedApplicationDocument = db.ApplicationDocument.Add(ApplicationDocument);
                await db.SaveChangesAsync();
                return _mapper.Map<ApplicationDocument, ApplicationDocumentDto>(addedApplicationDocument.Entity);
            }
        }

        public async Task<IEnumerable<ApplicationDocumentDto>> GetAllApplicationDocument()
        {
            try
            {
                using (var db = new ApplicationDbContext(dbOptions))
                {
                    IEnumerable<ApplicationDocumentDto> ApplicationDocumentDtos =
                        _mapper.Map<IEnumerable<ApplicationDocument>, IEnumerable<ApplicationDocumentDto>>(db.ApplicationDocument);
                    return ApplicationDocumentDtos;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<ApplicationDocumentDto> GetApplicationDocument(long Id)
        {
            try
            {
                using (var db = new ApplicationDbContext(dbOptions))
                {
                    ApplicationDocumentDto ApplicationDocumentDto =
                        _mapper.Map<ApplicationDocument, ApplicationDocumentDto>(await db.ApplicationDocument.FirstOrDefaultAsync(x => x.Id == Id));
                    return ApplicationDocumentDto;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<IEnumerable<ApplicationDocumentDto>> GetApplicationDocumentByUserId(string userId)
        {
            try
            {
                using (var db = new ApplicationDbContext(dbOptions))
                {
                    IEnumerable<ApplicationDocumentDto> ApplicationDocumentDto =
                        _mapper.Map<IEnumerable<ApplicationDocument>, IEnumerable<ApplicationDocumentDto>>( db.ApplicationDocument.Where(x => x.UserId == userId));
                    return ApplicationDocumentDto;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
public async Task<ApplicationDocumentDto> Getdocumentbytypeanduserid(DocumentType  type, string userid)
{
    try
    {
        using (var db = new ApplicationDbContext(dbOptions))
        {
            ApplicationDocumentDto applicationDocumentDto =
                _mapper.Map<ApplicationDocument, ApplicationDocumentDto>(
                    await db.ApplicationDocument.FirstOrDefaultAsync(x => 
                        x.ApplicationDocumentType == type && x.UserId == userid));
            return applicationDocumentDto;
        }
    }
    catch (Exception ex)
    {
        // Consider logging the exception here
        // _logger.LogError(ex, "Error in Getdocumentbytypeanduserid");
        return null;
    }
}
        public async Task DeleteApplicationDocument(long Id)
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                var ApplicationDocument = db.ApplicationDocument.FirstOrDefault(x => x.Id == Id);
                if (ApplicationDocument != null)
                {
                    db.ApplicationDocument.Remove(ApplicationDocument);
                    db.SaveChanges();
                }


            }
        }

        public async Task<ApplicationDocumentDto> UpdateApplicationDocument(ApplicationDocumentDto ApplicationDocumentDto)
        {
            try
            {
                using (var db = new ApplicationDbContext(dbOptions))
                {
                    {

                        ApplicationDocument ApplicationDocument = db.ApplicationDocument.FirstOrDefault(x => x.Id == ApplicationDocumentDto.Id);
                        if (ApplicationDocument == null)
                        {
                            return null;
                        }
                        else
                        {
                            ApplicationDocument ApplicationDocumentUpdate = _mapper.Map<ApplicationDocumentDto, ApplicationDocument>(ApplicationDocumentDto, ApplicationDocument);
                            var currentupdate = db.ApplicationDocument.Update(ApplicationDocumentUpdate);
                            await db.SaveChangesAsync();
                            return _mapper.Map<ApplicationDocument, ApplicationDocumentDto>(currentupdate.Entity);
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
