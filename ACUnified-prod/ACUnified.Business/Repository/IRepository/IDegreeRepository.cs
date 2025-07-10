using ACUnified.Data.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


    public interface IDegreeRepository
    {
        Task<DegreeDto> CreateDegree(DegreeDto degreeDto);
        Task<IEnumerable<DegreeDto>> GetAllDegree();
        Task<DegreeDto> GetDegree(long Id);
        Task DeleteDegree(long Id);
        Task<DegreeDto> UpdateDegree(DegreeDto degreeDto);
    }

