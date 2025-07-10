using ACUnified.Data.DTOs;
public interface IDesignationRepository
{
    Task<DesignationDto> CreateDesignation(DesignationDto designationDto);
    Task<IEnumerable<DesignationDto>> GetAllDesignation();
    Task<DesignationDto> GetDesignation(long Id);
    Task DeleteDesignation(long Id);
    Task<DesignationDto> UpdateDesignation(DesignationDto designationDto);
}
