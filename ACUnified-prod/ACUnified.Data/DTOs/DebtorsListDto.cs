using ACUnified.Data.Enum;
using ACUnified.Data.Models;

namespace ACUnified.Data.DTOs;

public class DebtorsListDto
{
    public long Id { get; set; }
    public long StudentId { get; set; }
    public Decimal OutstandingAmount { get; set; }
    public Decimal TotalAmount { get; set; }
    public Decimal TotalPaid { get; set; }
    public DateTime DueDate { get; set; }
    public PayPlan  PaymentPlan { get; set; }
    public string Matric { get; set; }
    public virtual Semester Semester { get; set; }
    public long? SemesterId { get; set; }
    public long SessionId { get; set; }
    public virtual Session Session { get; set; }
    public virtual Student Student { get; set; }
    //institution provider
    public virtual long? InstitutionProviderId { get; set; }

}