namespace ACUnified.Portal.MenuHelper
{
    public class MainMenuViewModel
    {
        public bool FreshApplicantDashboard { get; set; }
        public bool BioData { get; set; }
        public bool OtherDetails { get; set; }
        public bool AcademicQualification { get; set; }
        public bool NextOfKin { get; set; }
        public bool UploadDocuments { get; set; }
        public bool ApplicationStatus { get; set; }
    
        //Student
        public bool StudentDashboard { get; set; }
        public bool CourseRegistration { get; set; }
        public bool HostelRegistration { get; set; }
        public bool HealthAppointment { get; set; }
        public bool FeePayment { get; set; }
    
        //Busary
        public bool BusaryDashboard { get; set; }
    
        //Hostel
    
    
    
        //Admissions
        public bool AdmissionsOfficerDashboard { get; set; }
        public bool AdmissionsOfficerDashboardBTH { get; set; }
        public bool AdmissionsOfficerDashboardPG { get; set; }


        //Health
        public bool HealthOfficerDashboard { get; set; }
    
        //Alumni
        public bool AlumniDashboard { get; set; }
    
        //Payment
        public bool PaymentDashboard { get; set; }
    
        //public Result Manager
        public bool ResultManagerDashboard { get; set; }
    
        //Public Hostel Manager
        public bool HostelManagerDashboard { get; set; }
    
        //Public ICT Manager
        public bool ICTManagerDashboard { get; set; }

        //Public AdmissionDecisionFinalization Manager

        public bool AdmissionDecisionFinalizationDashboard {get; set;}
        public bool AdmissionDecisionFinalizationDashboardBTH { get; set; }
        public bool AdmissionDecisionFinalizationDashboardPG { get; set; }
        public bool AdmissionDecisionFinalizationDashboardJUPEB{get; set;}

    }
}

