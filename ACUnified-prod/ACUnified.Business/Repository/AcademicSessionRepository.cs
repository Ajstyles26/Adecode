using ACUnified.Data.DTOs;
using ACUnified.Data.Models;
using AutoMapper;

using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Ocsp;

public class AcademicSessionRepository : IAcademicSessionRepository
{
    
    private readonly DbContextOptions<ApplicationDbContext> dbOptions;
    private readonly IMapper _mapper;
    public AcademicSessionRepository(DbContextOptions<ApplicationDbContext> options, IMapper mapper)
    {
        _mapper = mapper;
        dbOptions = options;
    }

    public async Task<SessionDto> CreateSession(SessionDto sessionDto)
    {
        using (var db=new ApplicationDbContext(dbOptions))
        {
            Session session = _mapper.Map<SessionDto, Session>(sessionDto);
            var addedSession = db.Session.Add(session);
            await db.SaveChangesAsync();
            return _mapper.Map<Session, SessionDto>(addedSession.Entity);
        }
    }

   

    public async Task<IEnumerable<SessionDto>> GetAllSession()
    {
        try
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                IEnumerable<SessionDto> sessionDtos =
                    _mapper.Map<IEnumerable<Session>, IEnumerable<SessionDto>>(db.Session);
                return sessionDtos;
            }
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    public async Task<SessionDto> GetSession(long Id)
    {
        try
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                SessionDto sessionDto =
                    _mapper.Map<Session, SessionDto>(await db.Session.FirstOrDefaultAsync(x => x.Id == Id));
                return sessionDto;
            }
        }
        catch (Exception ex)
        {
            return null;
        }
    }
    
    public  async Task<IEnumerable<SessionDto>> GetActiveSession()
    {
        try
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                var result = db.Session.Where(x => x.isActive);
                IEnumerable<SessionDto> sessionDto = _mapper.Map<IEnumerable<Session>, IEnumerable<SessionDto>>(result);
                return sessionDto;
            }
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    public async Task<IEnumerable<SessionDto>> GetActiveApplicantSession()
    {
        try
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                var result = db.Session.Where(x => x.isApplicantActive);
                IEnumerable<SessionDto> sessionDto = _mapper.Map<IEnumerable<Session>, IEnumerable<SessionDto>>(result);
                return sessionDto;
            }
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    public async Task DeleteSession(long Id)
    {
        using (var db = new ApplicationDbContext(dbOptions))
        {
            var acada_session = db.Session.FirstOrDefault(x => x.Id == Id);
            if (acada_session != null)
            {
                db.Session.Remove(acada_session);
                db.SaveChanges();
            }
        }
    }

    public async Task<SessionDto> UpdateSession(SessionDto sessionDto)
    {
        try
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                Session session = db.Session.FirstOrDefault(x => x.Id == sessionDto.Id);
                if (session == null)
                {
                    return null;
                }
                else
                {
                    Session sessionUpdate = _mapper.Map<SessionDto, Session>(sessionDto, session);
                    var currentupdate = db.Session.Update(sessionUpdate);
                    await db.SaveChangesAsync();
                    return _mapper.Map<Session, SessionDto>(currentupdate.Entity);
                }
            }
        }
        catch (Exception ex)
        {
            return null;
        }
    }

   

  
}
