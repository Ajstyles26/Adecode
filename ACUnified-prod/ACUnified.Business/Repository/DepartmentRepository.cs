using ACUnified.Data.DTOs;
using ACUnified.Data.Models;

using AutoMapper;

using Microsoft.EntityFrameworkCore;
public class DepartmentRepository : IDepartmentRepository
{
    private readonly DbContextOptions<ApplicationDbContext> dbOptions;
    private readonly IMapper _mapper;
    public DepartmentRepository(DbContextOptions<ApplicationDbContext> options, IMapper mapper)
    {
        _mapper = mapper;
        dbOptions = options;
    }

    public async Task<DepartmentDto> CreateDepartment(DepartmentDto departmentDto)
    {
        using (var db = new ApplicationDbContext(dbOptions))
        {
            Department department = _mapper.Map<DepartmentDto, Department>(departmentDto);
            var addedDepartment = db.Department.Add(department);
            await db.SaveChangesAsync();
            return _mapper.Map<Department, DepartmentDto>(addedDepartment.Entity);
        }
    }

    public async Task<IEnumerable<DepartmentDto>> GetAllDepartment()
    {
        try
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                IEnumerable<DepartmentDto> departmentDtos =
                    _mapper.Map<IEnumerable<Department>, IEnumerable<DepartmentDto>>(db.Department);
                return departmentDtos;
            }
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    public async Task<DepartmentDto> GetDepartment(long Id)
    {
        try
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                DepartmentDto departmentDto =
                    _mapper.Map<Department, DepartmentDto>(await db.Department.FirstOrDefaultAsync(x => x.Id == Id));
                return departmentDto;
            }
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    public async Task DeleteDepartment(long Id)
    {
        using (var db = new ApplicationDbContext(dbOptions))
        {
            var department = db.Department.FirstOrDefault(x => x.Id == Id);
            if (department != null)
            {
                db.Department.Remove(department);
                db.SaveChanges();
            }

            
        }
    }

    public async Task<DepartmentDto> UpdateDepartment(DepartmentDto departmentDto)
    {
        try
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                {

                    Department department = db.Department.FirstOrDefault(x => x.Id == departmentDto.Id);
                    if (department == null)
                    {
                        return null;
                    }
                    else
                    {
                        Department departmentUpdate = _mapper.Map<DepartmentDto, Department>(departmentDto, department);
                        var currentupdate = db.Department.Update(departmentUpdate);
                        await db.SaveChangesAsync();
                        return _mapper.Map<Department, DepartmentDto>(currentupdate.Entity);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            return null;
        }
    }
}
