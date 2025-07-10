using ACUnified.Data.DTOs;

namespace ACUnified.Business.Repository.IRepository;

public interface IHealthAppointmentRepository
{
    Task<HealthAppointmentDto> CreateHealthAppointment(HealthAppointmentDto healthDto);
    Task<IEnumerable<HealthAppointmentDto>> GetAllHealthAppointment();
    Task<IEnumerable<HealthAppointmentDto>> GetAllHealthAppointmentByUserId(string userid);
    Task<HealthAppointmentDto> GetHealthAppointment(long Id);
    Task DeleteHealthAppointment(long Id);
    Task<HealthAppointmentDto> UpdateHealthAppointment(HealthAppointmentDto healthDto);
}