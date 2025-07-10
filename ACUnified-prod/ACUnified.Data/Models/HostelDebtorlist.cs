using ACUnified.Data.Enum;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ACUnified.Data.Models;

  
    public class HostelDebtorList:BaseClass
{
    public long ApplicationFormId { get; set; }
    public Decimal OutstandingAmount { get; set; }
    public DateTime DueDate { get; set; }
    public PayPlan  PaymentPlan { get; set; }
    public virtual Semester Semester { get; set; }
    public long? SemesterId { get; set; }
    
    public virtual ApplicationForm ApplicationForm { get; set; }
    //institution provider
    
 }