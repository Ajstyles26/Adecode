using ACUnified.Data.DTOs;

public interface IDepartmentRepository
{
    Task<DepartmentDto> CreateDepartment(DepartmentDto departmentDto);
    Task<IEnumerable<DepartmentDto>> GetAllDepartment();
    Task<DepartmentDto> GetDepartment(long Id);
    Task DeleteDepartment(long Id);
    Task<DepartmentDto> UpdateDepartment(DepartmentDto departmentDto);
}
