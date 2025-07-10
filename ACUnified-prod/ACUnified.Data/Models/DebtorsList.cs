using ACUnified.Data.Enum;

namespace ACUnified.Data.Models;

public class DebtorsList:BaseClass
{
    public long StudentId { get; set; }
    public Decimal OutstandingAmount { get; set; }
    public DateTime DueDate { get; set; }
    public PayPlan  PaymentPlan { get; set; }
    public virtual Semester Semester { get; set; }
    public long? SemesterId { get; set; }
    public long SessionId { get; set; }
    public virtual Session Session { get; set; }
    public virtual Student Student { get; set; }
    //institution provider
    public virtual long? InstitutionProviderId { get; set; }
    public virtual InstitutionProvider? InstitutionProvider { get; set; }
}