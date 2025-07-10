using ACUnified.Business.Repository.IRepository;

namespace ACUnified.Business.Repository;

using AutoMapper;

using Microsoft.EntityFrameworkCore;
using System.Linq;
using ACUnified.Data.Models;
using ACUnified.Data.DTOs;

public class PaymentLogRepository : IPaymentLogRepository
{
    private readonly DbContextOptions<ApplicationDbContext> dbOptions;
    private readonly IMapper _mapper;
    public PaymentLogRepository(DbContextOptions<ApplicationDbContext> options, IMapper mapper)
    {
        _mapper = mapper;
        dbOptions = options;
    }

    public async Task<PaymentLogDto> CreatePayment(PaymentLogDto PaymentLogDto)
    {
        using (var db = new ApplicationDbContext(dbOptions))
        {
            Payment payment = _mapper.Map<PaymentLogDto, Payment>(PaymentLogDto);
            var addedPayment = db.Payment.Add(payment);
            await db.SaveChangesAsync();
            return _mapper.Map<Payment, PaymentLogDto>(addedPayment.Entity);
        }
    }
    
    public async Task<bool> CreatePayments(List<PaymentLogDto> PaymentLogDto)
    {
        using ( var db = new ApplicationDbContext(dbOptions))
        {
            List<Payment> payment = _mapper.Map<List<PaymentLogDto>, List<Payment>>(PaymentLogDto);
            var addedPayment = db.Payment.AddRangeAsync(payment);
            await db.SaveChangesAsync();
            return addedPayment.IsCompletedSuccessfully;
        }
    }

    public async Task<IEnumerable<PaymentLogDto>> GetAllPayment()
    {
        try
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                IEnumerable<PaymentLogDto> PaymentLogDtos =
                    _mapper.Map<IEnumerable<Payment>, IEnumerable<PaymentLogDto>>(
                        db.Payment.Include(x=>x.PayDetails)
                        .Include(x=>x.PayDetails.PayCategory)
                        .Include(x=>x.Semester)
                        .Include(x=>x.Semester.Session)
                        );
                //var paymentNames = PaymentLogDtos.Select(x => x.tran_ref).ToList();
                //db.Payment
                //    .Include(x=>x.PayDetails)
                //    .Where(x => paymentNames.Contains(x.tran_ref));
                return PaymentLogDtos;
            }
        }
        catch (Exception ex)
        {
            return null;
        }
    }
    public async Task<IEnumerable<PaymentLogDto>> GetAllPayment2()
    {
        try
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                IEnumerable<PaymentLogDto> PaymentLogDtos =
                    _mapper.Map<IEnumerable<Payment>, IEnumerable<PaymentLogDto>>(
                        db.Payment.Include(x => x.PayDetails)
                        .Include(x => x.PayDetails.PayCategory)
                        .Include(x => x.Semester)
                        .Include(x => x.Semester.Session).Take(10)
                        );
                //var paymentNames = PaymentLogDtos.Select(x => x.tran_ref).ToList();
                //db.Payment
                //    .Include(x=>x.PayDetails)
                //    .Where(x => paymentNames.Contains(x.tran_ref));
                return PaymentLogDtos;
            }
        }
        catch (Exception ex)
        {
            return null;
        }
    }
    public async Task<IEnumerable<PaymentLogDto>> GetAllPaymentByMatric(string matric)
    {
        try
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                

                var paymentlist=await db.Payment.Where(x =>  x.MatricNo == matric).ToListAsync();
                IEnumerable<PaymentLogDto> PaymentLogDtos =
                    _mapper.Map<IEnumerable<Payment>, IEnumerable<PaymentLogDto>>(paymentlist);

                return PaymentLogDtos;
            }
        }
        catch (Exception ex)
        {
            return null;
        }
    }
    
    public async Task<IEnumerable<PaymentLogDto>> GetAllPaymentByReferenceNo(string referenceNo)
    {
        try
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                IEnumerable<PaymentLogDto> PaymentLogDtos =
                    _mapper.Map<IEnumerable<Payment>, IEnumerable<PaymentLogDto>>
                    (db.Payment.
                    Where(x => x.ReferenceNo == referenceNo).Include(x=>x.PayDetails).
                        Include(x=>x.PayDetails.PayCategory).
                        Include(x=>x.Semester).
                        Include(x=>x.Semester.Session)
                        .ToList()
                    );
                //var paymentNames = PaymentLogDtos.Select(x => x.ReferenceNo).ToList();
                //db.Payment.Where(x => paymentNames.Contains(x.ReferenceNo) && x.ReferenceNo == referenceNo);
                return PaymentLogDtos;
            }
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    public async Task<bool> PaymentExists(PaymentLogDto PaymentLogDto)
    {
        if (PaymentLogDto == null) return false;
        try
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {

                return await db.Payment.AnyAsync(f => f.tran_ref == PaymentLogDto.tran_ref);
            }
        }
        catch (Exception ex)
        {
           
            return false;
        }
    }

    public async Task<PaymentLogDto> GetPayment(long Id)
    {
        try
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                PaymentLogDto PaymentLogDto =
                    _mapper.Map<Payment, PaymentLogDto>(await db.Payment.FirstOrDefaultAsync(x => x.Id == Id));
                return PaymentLogDto;
            }
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    public async Task DeletePayment(long Id)
    {
        using (var db = new ApplicationDbContext(dbOptions))
        {
            var payment = db.Payment.FirstOrDefault(x => x.Id == Id);
            if (payment != null)
            {
                db.Payment.Remove(payment);
                db.SaveChanges();
            }
        }
    }

    public async Task<PaymentLogDto> UpdatePayment(PaymentLogDto PaymentLogDto)
    {
        try
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                Payment payment = db.Payment.FirstOrDefault(x => x.Id == PaymentLogDto.Id);
                if (payment == null)
                {
                    return null;
                }
                else
                {
                    Payment paymentUpdate = _mapper.Map<PaymentLogDto, Payment>(PaymentLogDto, payment);
                    var currentupdate = db.Payment.Update(paymentUpdate);
                    await db.SaveChangesAsync();
                    return _mapper.Map<Payment, PaymentLogDto>(currentupdate.Entity);
                }
            }
        }
        catch (Exception ex)
        {
            return null;
        }
    }
    
    
    //check if student has paid the course form fee
    
    //check if student have paid the mandatory fees

    public bool HasPaidMandatoryFees(string matric, long semesterId)
    {
        bool hasPaidManatoryFees = false;
        using (var db = new ApplicationDbContext(dbOptions))
        {
            var payment = db.Payment.Where(x => x.MatricNo == matric &&
                                                x.SemesterId==semesterId && x.isSuccessful==true).ToList();
            if (payment != null)
            {
                hasPaidManatoryFees=true;
            }
            
        }

        return hasPaidManatoryFees;
    }

    public bool HasPaidCourseFormFees(string matric, long semesterId)
    {
        bool hasPaidCourseFormFees = false;
        using (var db = new ApplicationDbContext(dbOptions))
        {
            var payment = db.Payment.FirstOrDefault(x => x.MatricNo == matric 
                                                         && x.SemesterId==semesterId &&
                                                         x.PayCategory.Name.Contains("Portal Access") 
                                                         && x.isSuccessful==true);
            if (payment != null)
            {
                hasPaidCourseFormFees=true;
            }
            
        }

        return hasPaidCourseFormFees;
    }
    
    public bool HasApplicantPaidFormFees(string userId, long semesterId)
    {
        bool hasApplicantPaidFees = false;
        using (var db = new ApplicationDbContext(dbOptions))
        {
            var payment = db.Payment.FirstOrDefault(x => x.MatricNo == userId 
                                                         && x.SemesterId==semesterId &&
                                                         //applicant category is 10000
                                                         x.PayCategoryId==10000                                                        && x.isSuccessful==true);
            if (payment != null)
            {
                hasApplicantPaidFees=true;
            }
            
        }

        return hasApplicantPaidFees;
    }

    public IEnumerable<PaymentLogDto> GetPaymentByDetailsId(IEnumerable<long> PaymentDetailId)
    {
        using (var db = new ApplicationDbContext(dbOptions))
        {
            IEnumerable<Payment> payment =   db.Payment.Where(x => PaymentDetailId.Contains(x.PayDetailsId??0) &&x.isSuccessful==true).AsEnumerable();
           
            if (payment != null)
            {
                return  _mapper.Map<IEnumerable<Payment>, IEnumerable<PaymentLogDto>>(payment);
            }

            return  Enumerable.Empty<PaymentLogDto>();
        }
    }

    public async Task<IEnumerable<DebtorsListDto>> GetAllDebtors()
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
                Matric=g.Key.Matric,
                TotalAmount=g.Sum(x=>x.PayDetails.TotalAmount),
                TotalPaid=g.Where(x=>x.Payment.isSuccessful==true)
                .Sum(x=>(x.Payment.Amount!=null?x.Payment.Amount:0)),
                OutstandingAmount = (g.Sum(x=>x.PayDetails.TotalAmount)-g.
                Where(x => x.Payment.isSuccessful == true)
                .Sum(x => (x.Payment.Amount != null ? x.Payment.Amount : 0)))
            })
            .ToListAsync();

            if (debtors.Count() >0)
            {
                return debtors;
            }

            return Enumerable.Empty<DebtorsListDto>();
        }
    }
}
