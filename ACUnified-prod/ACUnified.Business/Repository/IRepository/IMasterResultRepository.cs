using ACUnified.Data.DTOs;

namespace ACUnified.Business.Repository.IRepository;

public interface IMasterResultRepository
{
    Task<MasterResultDto> CreateMasterResultDto(MasterResultDto nextofkinDto);
    Task<IEnumerable<MasterResultDto>> GetAllMasterResultDto();
    Task<MasterResultDto> GetMasterResultDto(long Id);
    Task DeleteMasterResultDto(long Id);
    Task<MasterResultDto> UpdateMasterResultDto(MasterResultDto nextofkinDto);
}