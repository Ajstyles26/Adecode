using ACUnified.Data.DTOs;



public interface IAcademicQualificationRepository
{
    Task<AcademicQualificationDto> CreateAcademicQualification(AcademicQualificationDto otherdetailsDto);
     
    public  Task<AcademicQualificationDto> GetAcademicQualification(string userid);
    Task<IEnumerable<AcademicQualificationDto>> GetAllAcademicQualification();
    Task DeleteAcademicQualification(long Id);
    Task<AcademicQualificationDto> UpdateAcademicQualification(AcademicQualificationDto otherdetailsDto);
}
