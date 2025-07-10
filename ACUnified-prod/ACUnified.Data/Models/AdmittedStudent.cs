namespace ACUnified.Data.Models
{
    public class AdmittedStudent
    {
        public long Id { get; set; } // Primary key
        public string Surname { get; set; } = "";
        public string Othernames { get; set; } = "";
        public string Gender { get; set; } = "";
        public DateTime Birth_Date { get; set; }
        public string M_status { get; set; } = "";
        public string Student_phone { get; set; } = "";
        public string Student_email { get; set; } = "";
        public string State { get; set; } = "";
        public string Lga { get; set; } = "";
        public string Nationality { get; set; } = "";
        public string UTME_No { get; set; } = "";
        public string Reg_No { get; set; } = "";
        public string Matric_No { get; set; } = "";
        public int Deptid { get; set; }
        public string Dept { get; set; } = "";
        public int Programmid { get; set; }
        public string Programm { get; set; } = "";
        public string Entry_Session { get; set; } = "";
        public string Entry_Year { get; set; } = "";
        public string Entry_Mode { get; set; } = "";
        public int Entry_Level { get; set; }
        public int Degree { get; set; }
        public string Options { get; set; } = "";
        public int Current_Level { get; set; }
        public string Religion { get; set; } = "";
        public DateTime CreatedOn { get; set; }
        public bool IsActive { get; set; }
    }
}