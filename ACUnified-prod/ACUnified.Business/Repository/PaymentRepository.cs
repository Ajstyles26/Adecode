using ACUnified.Business.Repository.IRepository;

namespace ACUnified.Business.Repository;

using AutoMapper;

using Microsoft.EntityFrameworkCore;
using System.Linq;
using ACUnified.Data.Models;
using ACUnified.Data.DTOs;

public class PaymentRepository : IPaymentRepository
{
    private readonly DbContextOptions<ApplicationDbContext> dbOptions;
    private readonly IMapper _mapper;
    public PaymentRepository(DbContextOptions<ApplicationDbContext> options, IMapper mapper)
    {
        _mapper = mapper;
        dbOptions = options;
    }

    public async Task<PaymentDto> CreatePayment(PaymentDto paymentDto)
    {
        using (var db = new ApplicationDbContext(dbOptions))
        {
            Payment payment = _mapper.Map<PaymentDto, Payment>(paymentDto);
            var addedPayment = db.Payment.Add(payment);
            await db.SaveChangesAsync();
            return _mapper.Map<Payment, PaymentDto>(addedPayment.Entity);
        }
    }
    
    public async Task<bool> CreatePayments(List<PaymentDto> paymentDto)
    {
        using ( var db = new ApplicationDbContext(dbOptions))
        {
            List<Payment> payment = _mapper.Map<List<PaymentDto>, List<Payment>>(paymentDto);
            var addedPayment = db.Payment.AddRangeAsync(payment);
            await db.SaveChangesAsync();
            return addedPayment.IsCompletedSuccessfully;
        }
    }

    public async Task<IEnumerable<PaymentDto>> GetAllPayment()
    {
        try
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                IEnumerable<PaymentDto> paymentDtos =
                    _mapper.Map<IEnumerable<Payment>, IEnumerable<PaymentDto>>(
                        db.Payment.Include(x=>x.PayDetails)
                        .Include(x=>x.PayDetails.PayCategory)
                        .Include(x=>x.Semester)
                        .Include(x=>x.Semester.Session)
                        );
                //var paymentNames = paymentDtos.Select(x => x.tran_ref).ToList();
                //db.Payment
                //    .Include(x=>x.PayDetails)
                //    .Where(x => paymentNames.Contains(x.tran_ref));
                return paymentDtos;
            }
        }
        catch (Exception ex)
        {
            return null;
        }
    }
    public async Task<IEnumerable<PaymentDto>> GetAllPayment2()
    {
        try
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                IEnumerable<PaymentDto> paymentDtos =
                    _mapper.Map<IEnumerable<Payment>, IEnumerable<PaymentDto>>(
                        db.Payment.Include(x => x.PayDetails)
                        .Include(x => x.PayDetails.PayCategory)
                        .Include(x => x.Semester)
                        .Include(x => x.Semester.Session).Take(10)
                        );
                //var paymentNames = paymentDtos.Select(x => x.tran_ref).ToList();
                //db.Payment
                //    .Include(x=>x.PayDetails)
                //    .Where(x => paymentNames.Contains(x.tran_ref));
                return paymentDtos;
            }
        }
        catch (Exception ex)
        {
            return null;
        }
    }
    public async Task<IEnumerable<PaymentDto>> GetAllPaymentByMatric(string matric)
    {
        try
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                

                var paymentlist=await db.Payment.Where(x =>  x.MatricNo == matric).ToListAsync();
                IEnumerable<PaymentDto> paymentDtos =
                    _mapper.Map<IEnumerable<Payment>, IEnumerable<PaymentDto>>(paymentlist);

                return paymentDtos;
            }
        }
        catch (Exception ex)
        {
            return null;
        }
    }
    
    public async Task<IEnumerable<PaymentDto>> GetAllPaymentByReferenceNo(string referenceNo)
    {
        try
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                IEnumerable<PaymentDto> paymentDtos =
                    _mapper.Map<IEnumerable<Payment>, IEnumerable<PaymentDto>>
                    (db.Payment.
                    Where(x => x.ReferenceNo == referenceNo).Include(x=>x.PayDetails).
                        Include(x=>x.PayDetails.PayCategory).
                        Include(x=>x.Semester).
                        Include(x=>x.Semester.Session)
                        .ToList()
                    );
                //var paymentNames = paymentDtos.Select(x => x.ReferenceNo).ToList();
                //db.Payment.Where(x => paymentNames.Contains(x.ReferenceNo) && x.ReferenceNo == referenceNo);
                return paymentDtos;
            }
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    public async Task<bool> PaymentExists(PaymentDto paymentDto)
    {
        if (paymentDto == null) return false;
        try
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {

                return await db.Payment.AnyAsync(f => f.tran_ref == paymentDto.tran_ref);
            }
        }
        catch (Exception ex)
        {
           
            return false;
        }
    }

    public async Task<PaymentDto> GetPayment(long Id)
    {
        try
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                PaymentDto paymentDto =
                    _mapper.Map<Payment, PaymentDto>(await db.Payment.FirstOrDefaultAsync(x => x.Id == Id));
                return paymentDto;
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

    public async Task<PaymentDto> UpdatePayment(PaymentDto paymentDto)
    {
        try
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                Payment payment = db.Payment.FirstOrDefault(x => x.Id == paymentDto.Id);
                if (payment == null)
                {
                    return null;
                }
                else
                {
                    Payment paymentUpdate = _mapper.Map<PaymentDto, Payment>(paymentDto, payment);
                    var currentupdate = db.Payment.Update(paymentUpdate);
                    await db.SaveChangesAsync();
                    return _mapper.Map<Payment, PaymentDto>(currentupdate.Entity);
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

    public IEnumerable<PaymentDto> GetPaymentByDetailsId(IEnumerable<long> PaymentDetailId)
    {
        using (var db = new ApplicationDbContext(dbOptions))
        {
            IEnumerable<Payment> payment =   db.Payment.Where(x => PaymentDetailId.Contains(x.PayDetailsId??0) &&x.isSuccessful==true).AsEnumerable();
           
            if (payment != null)
            {
                return  _mapper.Map<IEnumerable<Payment>, IEnumerable<PaymentDto>>(payment);
            }

            return  Enumerable.Empty<PaymentDto>();
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
