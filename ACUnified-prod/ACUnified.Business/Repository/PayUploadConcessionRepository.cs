using AutoMapper;

using Microsoft.EntityFrameworkCore;
using System.Linq;
using ACUnified.Data.Models;
using ACUnified.Data.DTOs;


public class PayUploadConcessionRepository : IPayUploadConcessionRepository
{
    
    private readonly IMapper _mapper;
    private readonly DbContextOptions<ApplicationDbContext> dbOptions;
    public PayUploadConcessionRepository( IMapper mapper,DbContextOptions<ApplicationDbContext> options)
    {
        _mapper = mapper;
        
        dbOptions = options;
    }

    public async Task<PayUploadConcessionDto> CreatePayUploadConcession(PayUploadConcessionDto PayUploadConcessionDto)
    {
        using (var db=new ApplicationDbContext(dbOptions))
        {
            PayUploadConcession PayUploadConcession = _mapper.Map<PayUploadConcessionDto, PayUploadConcession>(PayUploadConcessionDto);
            var addedPayUploadConcession = db.PayUploadConcession.Add(PayUploadConcession);
            await db.SaveChangesAsync();
            return _mapper.Map<PayUploadConcession, PayUploadConcessionDto>(addedPayUploadConcession.Entity);
        }
        
    }

    public async Task<IEnumerable<PayUploadConcessionDto>> GetAllPayUploadConcession()
    {
        try
        {
            using (var db=new ApplicationDbContext(dbOptions))
            {
                IEnumerable<PayUploadConcessionDto> PayUploadConcessionDtos = _mapper.Map<IEnumerable<PayUploadConcession>, IEnumerable<PayUploadConcessionDto>>(db.PayUploadConcession);
                var PayUploadConcessionNames = PayUploadConcessionDtos.Select(x => x.Name).ToList();
                db.PayUploadConcession.Where(x => PayUploadConcessionNames.Contains(x.Name));
                return PayUploadConcessionDtos;
            }
            
           
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    public async Task<bool> PayUploadConcessionExists(PayUploadConcessionDto PayUploadConcessionDto)
    {
        if (PayUploadConcessionDto == null) return false;
        try
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                return await db.PayConcession.AnyAsync(f => f.Name == PayUploadConcessionDto.Name);
            }
        }
        catch (Exception ex)
        {
           
            return false;
        }
    }

    public async Task<PayUploadConcessionDto> GetPayUploadConcession(long Id)
    {
        try
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                PayUploadConcessionDto PayUploadConcessionDto =
                    _mapper.Map<PayUploadConcession, PayUploadConcessionDto>(await db.PayUploadConcession.FirstOrDefaultAsync(x => x.Id == Id));
                return PayUploadConcessionDto;
            }
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    public async Task<IEnumerable<PayUploadConcessionDto>> GetPayUploadConcessionBatch(long Id)
    {
        try
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                var info = db.PayUploadConcession.Where(x => x.PayUploadCategoryBatchId == Id).AsEnumerable();

                IEnumerable<PayUploadConcessionDto> PayUploadConcessionDto =
                    _mapper.Map<IEnumerable<PayUploadConcession>,IEnumerable<PayUploadConcessionDto>>(info);
                return PayUploadConcessionDto;
            }
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    public async Task DeletePayUploadConcession(long Id)
    {
        using (var db = new ApplicationDbContext(dbOptions))
        {
            var PayUploadConcession = db.PayUploadConcession.FirstOrDefault(x => x.Id == Id);
            if (PayUploadConcession != null)
            {
                db.PayUploadConcession.Remove(PayUploadConcession);
                db.SaveChanges();
            }
        }
    }

    public async Task<PayUploadConcessionDto> UpdatePayUploadConcession(PayUploadConcessionDto PayUploadConcessionDto)
    {
        try
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                PayUploadConcession PayUploadConcession = db.PayUploadConcession.FirstOrDefault(x => x.Id == PayUploadConcessionDto.Id);
                if (PayUploadConcession == null)
                {
                    return null;
                }
                else
                {
                    PayUploadConcession PayUploadConcessionUpdate = _mapper.Map<PayUploadConcessionDto, PayUploadConcession>(PayUploadConcessionDto, PayUploadConcession);
                    var currentupdate = db.PayUploadConcession.Update(PayUploadConcessionUpdate);
                    await db.SaveChangesAsync();
                    return _mapper.Map<PayUploadConcession, PayUploadConcessionDto>(currentupdate.Entity);
                }
            }
        }
        catch (Exception ex)
        {
            return null;
        }
    }

   
}
