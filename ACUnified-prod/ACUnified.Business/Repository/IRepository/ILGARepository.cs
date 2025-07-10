using ACUnified.Data.DTOs;



public interface ILGARepository
{
     Task<LGADto> CreateLGA(LGADto lgaDto);
    Task<IEnumerable<LGADto>> GetAllLGA();
    Task<LGADto> GetLGA(long Id);
    Task DeleteLGA(long Id);
    Task<LGADto> UpdateLGA(LGADto lgaDto);
}