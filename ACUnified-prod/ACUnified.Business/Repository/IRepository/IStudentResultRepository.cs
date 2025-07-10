using ACUnified.Data.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public interface IStudentResultRepository
{
    Task<IEnumerable<StudentResultDto>> GetResultByMatric(string matric, long semesterId, long sessionId);
    Task<IEnumerable<StudentResultListDto>> GetResultListingByMatric(string matric);
    Task<IEnumerable<StudentResultListDto>> GetResultListingByStudentId(long studentId);
    Task<IEnumerable<StudentResultDto>> GetResultByStudentId(long studentId, long semesterId, long sessionId);

}

