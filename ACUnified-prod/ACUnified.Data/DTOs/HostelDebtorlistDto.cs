using ACUnified.Data.Enum;
using ACUnified.Data.Models;

namespace ACUnified.Data.DTOs;

public class HostelDebtorListDto
{    public long Id { get; set; }
    public long  ApplicationFormId{ get; set; }

    public Decimal OutstandingAmount { get; set; }
    public Decimal TotalAmount { get; set; }
    public Decimal TotalPaid { get; set; }

       public long?  ApplicantPaymentId{ get; set; }
       public long?  ApplicantPayDetailsId {get; set;}
    public virtual ApplicantPayment ApplicantPayment { get; set; }
    public virtual ApplicantPayDetails ApplicantPayDetails { get; set; }
    public DateTime DueDate { get; set; }
    public PayPlan  PaymentPlan { get; set; }
    
    public virtual Semester Semester { get; set; }
    public long? SemesterId { get; set; }
    
    public virtual ApplicationForm ApplicationForm { get; set; }
    //institution provider
    

}