using System;
using ACUnified.Data.Models;

namespace ACUnified.Data.DTOs
{
    public class EducationalRecordDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Qualification { get; set; }
        public string Grade { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string DocumentPath { get; set; }
        public string UserId { get; set; }

         public long? ApplicationFormId { get; set; }
        public ApplicationForm? ApplicationForm { get; set; }
    }
}