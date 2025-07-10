using ACUnified.Data.Models;
using System;
namespace ACUnified.Data.DTOs
{
	public class BioDataDto
    {
        public long Id { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string OtherName { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Gender { get; set; } 
        public string PostalAddress { get; set; }
        public string State { get; set; }
        public string LGA { get; set; }
        public DateTime? DOB { get; set; }
        public string MaritalStatus { get; set; }
        public string Nationality { get; set; }
        public string UserId { get; set; }
        //institution provider
        public  long? InstitutionProviderId { get; set; }
        public long SessionId { get; set; }

    }
}

