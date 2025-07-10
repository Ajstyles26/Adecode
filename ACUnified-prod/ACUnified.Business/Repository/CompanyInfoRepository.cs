using ACUnified.Data.DTOs;
using ACUnified.Data.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

public class CompanyInfoRepository : ICompanyInfoRepository
{
    private readonly DbContextOptions<ApplicationDbContext> dbOptions;
    private readonly IMapper _mapper;
    public CompanyInfoRepository(DbContextOptions<ApplicationDbContext> options, IMapper mapper)
    {
        _mapper = mapper;
        dbOptions = options;
    }

    public async Task<CompanyInfoDto> CreateCompanyInfo(CompanyInfoDto companyinfoDto)
    {
        using (var db=new ApplicationDbContext(dbOptions))
        {
            CompanyInfo companyinfo = _mapper.Map<CompanyInfoDto, CompanyInfo>(companyinfoDto);
            var addedCompanyInfo = db.CompanyInfo.Add(companyinfo);
            await db.SaveChangesAsync();
            return _mapper.Map<CompanyInfo, CompanyInfoDto>(addedCompanyInfo.Entity);
        }
        
    }

    public async Task<IEnumerable<CompanyInfoDto>> GetAllCompanyInfo()
    {
        try
        {
            using (var db=new ApplicationDbContext(dbOptions))
            {
                IEnumerable<CompanyInfoDto> companyinfoDtos = _mapper.Map<IEnumerable<CompanyInfo>, IEnumerable<CompanyInfoDto>>(db.CompanyInfo);
                return companyinfoDtos;
            }
            
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    public async Task<CompanyInfoDto> GetCompanyInfo(long Id)
    {
        try
        {
            using (var db=new ApplicationDbContext(dbOptions))
            {
                CompanyInfoDto companyinfoDto = _mapper.Map<CompanyInfo, CompanyInfoDto>(await db.CompanyInfo.FirstOrDefaultAsync(x => x.Id == Id));
                return companyinfoDto;
            }
          
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    public async Task DeleteCompanyInfo(long Id)
    {
        using (var db=new ApplicationDbContext(dbOptions))
        {
            var companyInfo = db.CompanyInfo.FirstOrDefault(x => x.Id == Id);
            if(companyInfo!=null)
            {
                db.CompanyInfo.Remove(companyInfo);
                db.SaveChanges();
            }
        }
      
    }

    public async Task<CompanyInfoDto> UpdateCompanyInfo(CompanyInfoDto companyinfoDto)
    {
        try
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                CompanyInfo companyinfo = db.CompanyInfo.FirstOrDefault(x => x.Id == companyinfoDto.Id);
                if (companyinfo == null)
                {
                    return null;
                }
                else
                {
                    CompanyInfo companyinfoUpdate = _mapper.Map<CompanyInfoDto, CompanyInfo>(companyinfoDto, companyinfo);
                    var currentupdate = db.CompanyInfo.Update(companyinfoUpdate);
                    await db.SaveChangesAsync();
                    return _mapper.Map<CompanyInfo, CompanyInfoDto>(currentupdate.Entity);
                }
            }
          
        }
        catch (Exception ex)
        {
            return null;
        }
    }
}
