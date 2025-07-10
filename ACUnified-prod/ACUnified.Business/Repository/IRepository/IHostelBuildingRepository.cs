using ACUnified.Data.DTOs;

namespace ACUnified.Business.Repository.IRepository
{
    public interface IHostelBuildingRepository
    {
        Task<HostelBuildingDto> CreateHostelBuilding(HostelBuildingDto HostelBuildingDto);
        Task<IEnumerable<HostelBuildingDto>> GetAllHostelBuilding();
         Task<IEnumerable<HostelBuildingDto>> GetHostelsByGender(string gender);
        Task<HostelBuildingDto> GetHostelBuilding(long Id);
        Task DeleteHostelBuilding(long Id);
        Task<HostelBuildingDto> UpdateHostelBuilding(HostelBuildingDto HostelBuildingDto);
    }

}