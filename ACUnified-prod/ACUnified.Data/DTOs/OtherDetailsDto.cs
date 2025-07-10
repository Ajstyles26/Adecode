using ACUnified.Data.Models;
using System;
namespace ACUnified.Data.DTOs
{
	public class OtherDetailsDto
    {
        public long Id { get; set; }
        public string RedAddress { get; set; }
        public string PostalAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string Denomination { get; set; }
        public string ParentName { get; set; }
        public string ContactAddress { get; set; }
        public string FatherNo { get; set; }
        public string MotherNo { get; set; }
        public string ParentEmail { get; set; }
        public string UserId { get; set; }
        public string  Disability { get; set; }
        public string? Disabilitycomment { get; set; }
    }
}

