using ACUnified.Data.DTOs;


public interface IEducationalRecordRepository
{
    Task<IEnumerable<EducationalRecordDto>> GetAllEducationalRecord();
    Task<IEnumerable<EducationalRecordDto>> GetEducationalRecordsByUserId(string userId);
    Task<EducationalRecordDto> GetEducationalRecord(string userid);
    Task DeleteEducationalRecord(long Id);
    Task<EducationalRecordDto> CreateEducationalRecord(EducationalRecordDto educationalrecordDto);
    Task<EducationalRecordDto> UpdateEducationalRecord(EducationalRecordDto educationalrecordDto);
     Task<IEnumerable<EducationalRecordDto>> GetEducationalRecordsByApplicationFormId(long applicationFormId);
}
