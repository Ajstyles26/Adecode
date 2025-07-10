using ACUnified.Business.Repository.IRepository;
using ACUnified.Data.DTOs;
using ACUnified.Data.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace ACUnified.Business.Repository;

public class DebtorsListRepository : IDebtorsListRepository
{
    private readonly DbContextOptions<ApplicationDbContext> dbOptions;
    private readonly IMapper _mapper;

    public DebtorsListRepository(DbContextOptions<ApplicationDbContext> options, IMapper mapper)
    {
        _mapper = mapper;
        dbOptions = options;
    }

    Task<DebtorsListDto> IDebtorsListRepository.CreateDebtorsList(DebtorsListDto DebtorsListDto)
    {
        throw new NotImplementedException();
    }

    Task IDebtorsListRepository.DeleteDebtorsList(long Id)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<DebtorsListDto>> GetAllDebtorsList()
    {
        using (var db = new ApplicationDbContext(dbOptions))
        {
            var debtors = await db.PayDetails
            .GroupJoin(db.Payment,
                pd => pd.Id,
                p => p.PayDetailsId,
                (pd, payments) => new { PayDetails = pd, Payments = payments })
            .SelectMany(x => x.Payments.DefaultIfEmpty(),
                (pd, payment) => new
                {
                    PayDetails = pd.PayDetails,
                    Payment = payment
                })//only sum when the payment is successful
            //.Where(x=>x.Payment.isSuccessful==true)
            .GroupBy(x => new { x.PayDetails.StudentId, x.PayDetails.SessionId, x.PayDetails.Student.Matric }) // Group by StudentId and SemesterId
            .Select(g => new DebtorsListDto
            {
                StudentId = (long)g.Key.StudentId,
                SemesterId = g.Key.SessionId,
                Matric = g.Key.Matric,
                TotalAmount = g.Sum(x => x.PayDetails.TotalAmount),
                TotalPaid = g.Where(x => x.Payment.isSuccessful == true)
                .Sum(x => (x.Payment.Amount != null ? x.Payment.Amount : 0)),
                OutstandingAmount = (g.Sum(x => x.PayDetails.TotalAmount) - g.
                Where(x => x.Payment.isSuccessful == true)
                .Sum(x => (x.Payment.Amount != null ? x.Payment.Amount : 0)))
            })
            .ToListAsync();

            if (debtors != null)
            {
                return debtors;
            }

            return Enumerable.Empty<DebtorsListDto>();
        }
    }

    Task<DebtorsListDto> IDebtorsListRepository.GetDebtorsList(long Id)
    {
        throw new NotImplementedException();
    }

    Task<DebtorsListDto> IDebtorsListRepository.UpdateDebtorsList(DebtorsListDto DebtorsListDto)
    {
        throw new NotImplementedException();
    }
}