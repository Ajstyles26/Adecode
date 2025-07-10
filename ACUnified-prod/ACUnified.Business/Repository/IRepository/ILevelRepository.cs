using ACUnified.Data.DTOs;
public interface ILevelRepository
{
    Task<LevelDto> CreateLevel(LevelDto levelDto);
    Task<IEnumerable<LevelDto>> GetAllLevel();
    Task<LevelDto> GetLevel(long Id);
    Task DeleteLevel(long Id);
    Task<LevelDto> UpdateLevel(LevelDto levelDto);
}
