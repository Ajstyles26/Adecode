using ACUnified.Data.DTOs;
namespace ACUnified.Business.Repository.IRepository;

public interface IPaymentLogRepository
{
    Task<PaymentLogDto> CreatePayment(PaymentLogDto PaymentLogDto);
    Task<bool> CreatePayments(List<PaymentLogDto> PaymentLogDto);
    Task<IEnumerable<PaymentLogDto>> GetAllPayment();
    Task<IEnumerable<PaymentLogDto>> GetAllPayment2();
    Task<PaymentLogDto> GetPayment(long Id);
    Task DeletePayment(long Id);
    Task<PaymentLogDto> UpdatePayment(PaymentLogDto PaymentLogDto);

    Task<IEnumerable<PaymentLogDto>> GetAllPaymentByMatric(string matric);
    Task<IEnumerable<PaymentLogDto>> GetAllPaymentByReferenceNo(string referenceNo);
    IEnumerable<PaymentLogDto> GetPaymentByDetailsId(IEnumerable<long> PaymentDetailId);
    //todo: add debtors list ,transaction chart(failed count vs successful ,by payment channel etc) student receipt, failed transaction requery
    Task<IEnumerable<DebtorsListDto>> GetAllDebtors();
    bool HasPaidMandatoryFees(string matric, long semesterId);
    bool HasPaidCourseFormFees(string matric, long semesterId);
}