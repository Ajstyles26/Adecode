using AutoMapper;

using Microsoft.EntityFrameworkCore;
using System.Linq;
using ACUnified.Data.Enum;
using ACUnified.Data.Models;
using ACUnified.Data.DTOs;


public class PayCategoryRepository : IPayCategoryRepository
{
    private readonly DbContextOptions<ApplicationDbContext> dbOptions;
    private readonly IMapper _mapper;
    public PayCategoryRepository(DbContextOptions<ApplicationDbContext> options, IMapper mapper)
    {
        _mapper = mapper;
        dbOptions = options;
    }

    public async Task<PayCategoryDto> CreatePayCategory(PayCategoryDto PayCategoryDto)
    {
        using (var db=new ApplicationDbContext(dbOptions))
        {
        
            PayCategory pay = _mapper.Map<PayCategoryDto, PayCategory>(PayCategoryDto);
                    var addedCategory = db.PayCategory.Add(pay);
                    await db.SaveChangesAsync();
                    return _mapper.Map<PayCategory, PayCategoryDto>(addedCategory.Entity);
        }
        
    }

    public async Task CreatePayCategory(IEnumerable<PayCategoryDto> PayCategoryDto)
    {
        using (var db = new ApplicationDbContext(dbOptions))
        {

            IEnumerable<PayCategory> pay = _mapper.Map<IEnumerable<PayCategoryDto>, IEnumerable<PayCategory>>(PayCategoryDto);
            db.PayCategory.AddRange(pay);
            await db.SaveChangesAsync();
            
        }

    }

    public async Task<IEnumerable<PayCategoryDto>> GetAllPayCategory()
    {
        try
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                var currentPayCategory = db.PayCategory
                    .Include(x => x.StudentLevel)
                   .Include(x => x.Session)
                    .Include(x => x.ProgramSetup)
                    .Include(x => x.ProgramSetup.Department);
                if (currentPayCategory == null)
                {
                    return null;
                }

                IEnumerable<PayCategoryDto> PayCategoryDtos =
                    _mapper.Map<IEnumerable<PayCategory>, IEnumerable<PayCategoryDto>>(currentPayCategory);
                var categoryNames = PayCategoryDtos.Select(x => x.Name).ToList();
                db.PayCategory.Where(x => categoryNames.Contains(x.Name));
                return PayCategoryDtos;
            }
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    public async Task<bool> PayCategoryExists(PayCategoryDto PayCategoryDto)
    {
        
        if (PayCategoryDto == null) return false;
        try
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                return await db.PayCategory.AnyAsync(f => f.Name == PayCategoryDto.Name);
            }
        }
        catch (Exception ex)
        {
           
            return false;
        }
    }

    public async Task<PayCategoryDto> GetPayCategory(long Id)
    {
        try
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                PayCategoryDto PayCategoryDto =
                    _mapper.Map<PayCategory, PayCategoryDto>(
                        await db.PayCategory.FirstOrDefaultAsync(x => x.Id == Id));
                return PayCategoryDto;
            }
        }
        catch (Exception ex)
        {
            return null;
        }
    }
    
    public async Task<IEnumerable<PayCategoryDto>> GetStudentPayCategory(long levelId, long departmentId, long sessionId, long degreeId)
    {
        try
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                var payCategories = await db.PayCategory
                    .Where(x => x.StudentLevelId == levelId && x.ProgramSetup.DepartmentId == departmentId )
                    .ToListAsync();

                if (!payCategories.Any())
                {
                    // Optionally handle the case where no matching pay categories are found
                    return Enumerable.Empty<PayCategoryDto>();
                }

                var payCategoryDtos = _mapper.Map<IEnumerable<PayCategory>, IEnumerable<PayCategoryDto>>(payCategories);
                return payCategoryDtos;
            }
        }
        catch (Exception ex)
        {
            // Log the exception here for debugging purposes
            return Enumerable.Empty<PayCategoryDto>();
        }
    }

    public async Task<IEnumerable<PayCategoryDto>> GetApplicantPayCategory(long levelId, long departmentId, long sessionId, long degreeId)
    {
        try
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                var payCategories = await db.PayCategory
                    .Where(x => x.StudentLevelId == levelId && x.ProgramSetup.DepartmentId == departmentId )
                    .ToListAsync();

                if (!payCategories.Any())
                {
                    // Optionally handle the case where no matching pay categories are found
                    return Enumerable.Empty<PayCategoryDto>();
                }

                var payCategoryDtos = _mapper.Map<IEnumerable<PayCategory>, IEnumerable<PayCategoryDto>>(payCategories);
                return payCategoryDtos;
            }
        }
        catch (Exception ex)
        {
            // Log the exception here for debugging purposes
            return Enumerable.Empty<PayCategoryDto>();
        }
    }

    
  

    public async Task DeletePayCategory(long Id)
    {
        using (var db = new ApplicationDbContext(dbOptions))
        {
            var faculty = db.Faculty.FirstOrDefault(x => x.Id == Id);
            if (faculty != null)
            {
                db.Faculty.Remove(faculty);
                db.SaveChanges();
            }
        }
    }

    public async Task<PayCategoryDto> UpdatePayCategory(PayCategoryDto PayCategoryDto)
    {
        try
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                PayCategory payCategory = db.PayCategory.FirstOrDefault(x => x.Id == PayCategoryDto.Id);
                if (payCategory == null)
                {
                    return null;
                }
                else
                {
                    PayCategory payCategoryUpdate = _mapper.Map<PayCategoryDto, PayCategory>(PayCategoryDto, payCategory);
                    var currentupdate = db.PayCategory.Update(payCategoryUpdate);
                    await db.SaveChangesAsync();
                    return _mapper.Map<PayCategory, PayCategoryDto>(currentupdate.Entity);
                }
            }
        }
        catch (Exception ex)
        {
            return null;
        }
    }


}
