using System;
using ACUnified.Data.Models;
using ACUnified.Data.Enum;

namespace ACUnified.Data.DTOs
{
	public class StudentDto
    {
        
        public long Id { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string OtherName { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
        public string Address { get; set; }
        public string Matric { get; set; }
        public string RefNo { get; set; }
        public string State { get; set; }
        public string LGA { get; set; }
        public string Gender { get; set; }
        public DateTime DOB { get; set; }
        public string MaritalStatus { get; set; }
        public string Nationality { get; set; }
        
        public string? UserId { get; set; }
        public virtual ApplicationUser? CurrentUser { get; set; }
       
        public int YearOfEntry { get; set; }
        

        public string? UserImage { get; set; }
        public string? FingerUrl { get; set; }

        public CaptureStatus CaptureStatus { get; set; }

        public DateTime? DateCaptured { get; set; }
        //institution provider
        public long? InstitutionProviderId { get; set; }

        public EntryMode? StudentEntryMode { get; set; }
        public MigrationStatus MigrationStatus { get; set; }


    }
}

