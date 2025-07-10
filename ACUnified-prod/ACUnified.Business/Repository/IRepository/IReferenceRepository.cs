using ACUnified.Data.DTOs;


public interface IReferenceRepository
{
    Task<ReferenceDto> CreateReference(ReferenceDto referenceDto);
     
    public  Task<ReferenceDto> GetReference(string userid);
    Task<IEnumerable<ReferenceDto>> GetAllReference();
    Task DeleteReference(long Id);
    Task<ReferenceDto> UpdateReference(ReferenceDto referenceDto);
}
