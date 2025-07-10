using ACUnified.Data.DTOs;
namespace ACUnified.Business.Repository.IRepository;

public interface IPaymentRepository
{
    Task<PaymentDto> CreatePayment(PaymentDto paymentDto);
    Task<bool> CreatePayments(List<PaymentDto> paymentDto);
    Task<IEnumerable<PaymentDto>> GetAllPayment();
    Task<IEnumerable<PaymentDto>> GetAllPayment2();
    Task<PaymentDto> GetPayment(long Id);
    Task DeletePayment(long Id);
    Task<PaymentDto> UpdatePayment(PaymentDto paymentDto);

    Task<IEnumerable<PaymentDto>> GetAllPaymentByMatric(string matric);
    Task<IEnumerable<PaymentDto>> GetAllPaymentByReferenceNo(string referenceNo);
    IEnumerable<PaymentDto> GetPaymentByDetailsId(IEnumerable<long> PaymentDetailId);
    //todo: add debtors list ,transaction chart(failed count vs successful ,by payment channel etc) student receipt, failed transaction requery
    Task<IEnumerable<DebtorsListDto>> GetAllDebtors();
    bool HasPaidMandatoryFees(string matric, long semesterId);
    bool HasPaidCourseFormFees(string matric, long semesterId);
}