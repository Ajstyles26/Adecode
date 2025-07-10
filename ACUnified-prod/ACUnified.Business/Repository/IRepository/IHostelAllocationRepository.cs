using ACUnified.Data.DTOs;

namespace ACUnified.Business.Repository.IRepository
{
    public interface IHostelAllocationRepository
    {
        Task<HostelAllocationDto> CreateHostelAllocation(HostelAllocationDto HostelAllocationDto);
        Task<IEnumerable<HostelAllocationDto>> GetAllHostelAllocation();
        Task<HostelAllocationDto> GetHostelAllocation(long Id);
        Task<IEnumerable<HostelAllocationDto>> GetAllocationsByApplicationFormId(long applicationFormId);
            Task<IEnumerable<HostelAllocationDto>> GetPendingAllocationsOlderThan(DateTime cutoffTime);
            
    
        Task DeleteHostelAllocation(long Id);
        Task<HostelAllocationDto> GetHostelAllocationByApplicationFormId(long applicationFormId);
        Task<HostelAllocationDto> UpdateHostelAllocation(HostelAllocationDto HostelAllocationDto);
         Task<HostelAllocationDto> CreateHostelAllocationAndUpdateStage(HostelAllocationDto hostelAllocationDto);
    }
}