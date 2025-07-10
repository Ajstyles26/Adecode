using ACUnified.Data.DTOs;

public interface ICountryRepository
{
    Task<CountryDto> CreateCountry(CountryDto CountryDto);
    Task<IEnumerable<CountryDto>> GetAllCountry();
    Task<CountryDto> GetCountry(long Id);
    Task DeleteCountry(long Id);
    Task<CountryDto> UpdateCountry(CountryDto CountryDto);
 
}
