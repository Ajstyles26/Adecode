using System;
using System.ComponentModel.DataAnnotations.Schema;

 namespace ACUnified.BioVerification.Data
{
    [Table("Student")]
    public class Student
    {

        public long Id { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string OtherName { get; set; }
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
       
        
        public int YearOfEntry { get; set; }

        public bool IsGraduate { get; set; }

      

        public string UserImage { get; set; }
        public string FingerUrl { get; set; }

        

        public DateTime? DateCaptured { get; set; }


    }
}

