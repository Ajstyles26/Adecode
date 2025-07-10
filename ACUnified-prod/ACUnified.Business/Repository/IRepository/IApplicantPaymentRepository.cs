using ACUnified.Data.DTOs;


namespace ACUnified.Business.Repository.IRepository;

public interface IApplicantPaymentRepository
{
    Task<ApplicantPaymentDto> CreateApplicantPayment(ApplicantPaymentDto paymentDto);
    Task<ApplicantPaymentDto> CreateApplicantPayments(ApplicantPaymentDto paymentDto);
    Task<bool> CreatePayments(List<ApplicantPaymentDto> paymentDto);
    Task<IEnumerable<ApplicantPaymentDto>> GetAllPayment();
    Task<ApplicantPaymentDto> GetPayment(long Id);
    Task<bool> UpdatePayment(ApplicantPaymentDto payment);
    Task DeletePayment(long Id);
        Task<IEnumerable<ApplicationFormDto>> GetAll();
    Task<ApplicantPaymentDto> GetAllPaymentReferenceNo(string referenceNo);
    Task<IEnumerable<ApplicantPaymentDto>> GetAllPaymentByUserId(string matric);

    Task<IEnumerable<ApplicantPaymentDto>> GetAllPaymentByReferenceNo(string referenceNo); 
    //todo: add debtors list ,transaction chart(failed count vs successful ,by payment channel etc) student receipt, failed transaction requery

    bool HasPaidMandatoryFees(string matric, long semesterId);
    bool HasPaidCourseFormFees(string matric, long semesterId);
}