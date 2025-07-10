using ACUnified.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACUnified.Data.DTOs
{
    public class ApplicantPaymentDto
    {
        public long Id { get; set; }

        public decimal Amount { get; set; }


        public string? Comments { get; set; }

        public long? ApplicantPayCategoryId { get; set; }
        public virtual ApplicantPayCategory ApplicantPayCategory { get; set; }

        public long? ApplicantPayDetailsId { get; set; }
        public virtual ApplicantPayDetails ApplicantPayDetails { get; set; }

         public long? ApplicationFormId { get; set; }
    public virtual ApplicationForm ApplicationForm { get; set; }

        public virtual Semester Semester { get; set; }
        public long? SemesterId { get; set; }

        public long? SessionId { get; set; }

        public virtual Session AcademicSession { get; set; }

        public long? ProgramSetupId { get; set; }
        public virtual ProgramSetup ProgramSetup { get; set; }

        public string? ReferenceNo { get; set; }

        public string? RRRRNo { get; set; }

        public string? UserId { get; set; }

        public string? StaffNo { get; set; }

        public string? email { get; set; }

        public string? tran_id { get; set; }

        public string? tran_type { get; set; }

        public string? tran_ref { get; set; }

        public string? pay_response { get; set; }

        public string? tran_status { get; set; }

        //vendor client code
        public string? client_code { get; set; }

        //client that made payment for the payee
        public string? client_name { get; set; }
        //payee full name
        public string? full_name { get; set; }
        //cocat paysetup full name session
        public string? tran_name { get; set; }
        //listing of the items in the pay items.
        public bool isSuccessful { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
