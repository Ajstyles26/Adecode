using AutoMapper;

using Microsoft.EntityFrameworkCore;
using System.Linq;
using ACUnified.Data.Models;
using ACUnified.Data.DTOs;
using Microsoft.EntityFrameworkCore.Internal;


public class PayConcessionRepository : IPayConcessionRepository
{
    
    private readonly IMapper _mapper;
    private readonly DbContextOptions<ApplicationDbContext> dbOptions;
    public PayConcessionRepository( IMapper mapper,DbContextOptions<ApplicationDbContext> options)
    {
        _mapper = mapper;
        
        dbOptions = options;
    }

    public async Task<PayConcessionDto> CreatePayConcession(PayConcessionDto PayConcessionDto)
    {
        using (var db=new ApplicationDbContext(dbOptions))
        {
            PayConcession PayConcession = _mapper.Map<PayConcessionDto, PayConcession>(PayConcessionDto);
            var addedPayConcession = db.PayConcession.Add(PayConcession);
            await db.SaveChangesAsync();
            return _mapper.Map<PayConcession, PayConcessionDto>(addedPayConcession.Entity);
        }
        
    }

    public async Task CreatePayConcession(IEnumerable<PayConcessionDto> PayConcessionDto)
    {
        using (var db = new ApplicationDbContext(dbOptions))
        {
            IEnumerable<PayConcession> PayConcession = _mapper.Map<IEnumerable<PayConcessionDto>, IEnumerable<PayConcession>>(PayConcessionDto);
             db.PayConcession.AddRange(PayConcession);
            await db.SaveChangesAsync();
        }

    }



    public async Task<IEnumerable<PayConcessionDto>> GetAllPayConcession()
    {
        try
        {
            using (var db=new ApplicationDbContext(dbOptions))
            {
                //get current semester
                //var concessionResult =  db.PayConcession;
                //IEnumerable<PayConcessionDto> PayConcessionDtos = _mapper.Map<IEnumerable<PayConcession>, IEnumerable<PayConcessionDto>>(concessionResult);

                //TODO: ADD CURRENT SEMESTER TO ENABLE EFFECTIVE SEARCH
                var result = db.Student
                .GroupJoin(db.PayConcession,
                  student => student.Id,
                  concession => concession.StudentId,
                  (student, concessions) => new PayConcessionDto
                    {
                      StudentId= student.Id,
                      Student=student,
                      Amount = concessions.DefaultIfEmpty().Select(c => c.Amount??0M).FirstOrDefault()
                  }).ToList();
                return  result;
            }
            
           
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    public async Task<IEnumerable<PayConcessionDto>> GetAllPayByUserIdConcession(string Userid)
    {
        try
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                //get current semester
                //var concessionResult =  db.PayConcession;
                //IEnumerable<PayConcessionDto> PayConcessionDtos = _mapper.Map<IEnumerable<PayConcession>, IEnumerable<PayConcessionDto>>(concessionResult);

                //TODO: ADD CURRENT SEMESTER TO ENABLE EFFECTIVE SEARCH
                var result = db.Student
                    .Where(x=>x.UserId==Userid)
                .GroupJoin(db.PayConcession,
                  student => student.Id,
                  concession => concession.StudentId,
                  (student, concessions) => new PayConcessionDto
                  {
                      StudentId = student.Id,
                      Student = student,
                      Amount = concessions.DefaultIfEmpty().Select(c => c.Amount ?? 0M).FirstOrDefault()
                  }).ToList();
                return result;
            }


        }
        catch (Exception ex)
        {
            return null;
        }
    }

    public async Task<bool> PayConcessionExists(PayConcessionDto PayConcessionDto)
    {
        if (PayConcessionDto == null) return false;
        try
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                return await db.PayConcession.AnyAsync(f => f.Name == PayConcessionDto.Name);
            }
        }
        catch (Exception ex)
        {
           
            return false;
        }
    }

    public async Task<PayConcessionDto> GetPayConcession(long Id)
    {
        try
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                PayConcessionDto PayConcessionDto =
                    _mapper.Map<PayConcession, PayConcessionDto>(await db.PayConcession.FirstOrDefaultAsync(x => x.Id == Id));
                return PayConcessionDto;
            }
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    public async Task DeletePayConcession(long Id)
    {
        using (var db = new ApplicationDbContext(dbOptions))
        {
            var PayConcession = db.PayConcession.FirstOrDefault(x => x.Id == Id);
            if (PayConcession != null)
            {
                db.PayConcession.Remove(PayConcession);
                db.SaveChanges();
            }
        }
    }

    public async Task<PayConcessionDto> UpdatePayConcession(PayConcessionDto PayConcessionDto)
    {
        try
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                PayConcession PayConcession = db.PayConcession.FirstOrDefault(x => x.Id == PayConcessionDto.Id);
                if (PayConcession == null)
                {
                    return null;
                }
                else
                {
                    PayConcession PayConcessionUpdate = _mapper.Map<PayConcessionDto, PayConcession>(PayConcessionDto, PayConcession);
                    var currentupdate = db.PayConcession.Update(PayConcessionUpdate);
                    await db.SaveChangesAsync();
                    return _mapper.Map<PayConcession, PayConcessionDto>(currentupdate.Entity);
                }
            }
        }
        catch (Exception ex)
        {
            return null;
        }
    }
}
