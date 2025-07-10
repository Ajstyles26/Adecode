using ACUnified.Data.DTOs;
using ACUnified.Data.Enum;
using ACUnified.Data.Models;

using AutoMapper;

using Microsoft.EntityFrameworkCore;
public class CountryRepository : ICountryRepository
{
    private readonly DbContextOptions<ApplicationDbContext> dbOptions;
    private readonly IMapper _mapper;
    public CountryRepository(DbContextOptions<ApplicationDbContext> options, IMapper mapper)
    {
        _mapper = mapper;
        dbOptions = options;
    }

    public async Task<CountryDto> CreateCountry(CountryDto CountryDto)
    {
        using (var db = new ApplicationDbContext(dbOptions))
        {
            Country Country = _mapper.Map<CountryDto, Country>(CountryDto);
            var addedCountry = db.Country.Add(Country);
            await db.SaveChangesAsync();
            return _mapper.Map<Country, CountryDto>(addedCountry.Entity);
        }

    }

    public async Task<IEnumerable<CountryDto>> GetAllCountry()
    {
        try
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                IEnumerable<CountryDto> CountryDtos =
                    _mapper.Map<IEnumerable<Country>, IEnumerable<CountryDto>>(db.Country);
                return CountryDtos;
            }
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    public async Task<CountryDto> GetCountry(long Id)
    {
        try
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                CountryDto CountryDto =
                    _mapper.Map<Country, CountryDto>(await db.Country.FirstOrDefaultAsync(x => x.Id == Id));
                return CountryDto;
            }
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    

    

    public async Task DeleteCountry(long Id)
    {
        using (var db = new ApplicationDbContext(dbOptions))
        {
            var Country = db.Country.FirstOrDefault(x => x.Id == Id);
            if (Country != null)
            {
                db.Country.Remove(Country);
                db.SaveChanges();
            }
        }
    }

    public async Task<CountryDto> UpdateCountry(CountryDto CountryDto)
    {
        try
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                Country Country = db.Country.FirstOrDefault(x => x.Id == CountryDto.Id);
                if (Country == null)
                {
                    return null;
                }
                else
                {
                    Country CountryUpdate = _mapper.Map<CountryDto, Country>(CountryDto, Country);
                    var currentupdate = db.Country.Update(CountryUpdate);
                    await db.SaveChangesAsync();
                    return _mapper.Map<Country, CountryDto>(currentupdate.Entity);
                }
            }
        }
        catch (Exception ex)
        {
            return null;
        }
    }
}
