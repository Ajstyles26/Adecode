using ACUnified.Data.DTOs;


namespace ACUnified.Business.Repository.IRepository;

public interface IApplicantPaymentLogRepository
{
    Task<ApplicantPaymentLogDto> CreateApplicantPayment(ApplicantPaymentLogDto paymentDto);
    Task<bool> CreatePayments(List<ApplicantPaymentLogDto> paymentDto);
    Task<IEnumerable<ApplicantPaymentLogDto>> GetAllPayment();
    Task<ApplicantPaymentLogDto> GetPayment(long Id);
    Task DeletePayment(long Id);
    Task<ApplicantPaymentLogDto> UpdatePayment(ApplicantPaymentLogDto paymentDto);
    Task<ApplicantPaymentLogDto> GetLastPaymentByUserId(string userId);
    Task UpdateApplicantPayment(ApplicantPaymentLogDto payment);


    Task<IEnumerable<ApplicantPaymentLogDto>> GetAllPaymentByUserId(string matric);
    Task<IEnumerable<ApplicantPaymentLogDto>> GetAllPaymentByEmail(string Email);
    Task<IEnumerable<ApplicantPaymentLogDto>> GetAllPaymentByReferenceNo(string referenceNo); 
    //todo: add debtors list ,transaction chart(failed count vs successful ,by payment channel etc) student receipt, failed transaction requery

    bool HasPaidMandatoryFees(string matric, long semesterId);
    bool HasPaidCourseFormFees(string matric, long semesterId);
}