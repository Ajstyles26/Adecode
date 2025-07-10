using ACUnified.Data.Enum;

namespace ACUnified.Portal.MenuHelper
{
    public static class RolePage
    {
        public static readonly string[] ApplicantPages =
        {
               MainMenu.FreshApplicantDashboard.RoleName,
               MainMenu.BioData.RoleName,
               MainMenu.NextOfKin.RoleName,
               MainMenu.AcademicQualification.RoleName,
               MainMenu.UploadDocuments.RoleName,
               MainMenu.ApplicationStatus.RoleName
        };

        public static readonly string[] BusaryPages =
        {
            MainMenu.Bursary.RoleName,
            MainMenu.BursaryDashboard.RoleName
        };
        public static readonly string[] ParentPages =
        {

        };
        public static readonly string[] AdmissionOfficerPages =
        {
            MainMenu.AdmissionOffice.RoleName
        };

        public static readonly string[] AdmissionOfficerPagesBTH =
       {
            MainMenu.AdmissionOfficeBTH.RoleName
        };

        public static readonly string[] AdmissionOfficerPagesPG =
      {
            MainMenu.AdmissionOfficePG.RoleName
        };

        public static readonly string[] AdmissionDecisionFinalizationPages =
        {
            MainMenu.AdmissionDecisionFinalization.RoleName
        };

        public static readonly string[] AdmissionDecisionFinalizationPagesBTH =
        {
            MainMenu.AdmissionDecisionFinalizationBTH.RoleName
        };

        public static readonly string[] AdmissionDecisionFinalizationPagesPG =
       {
            MainMenu.AdmissionDecisionFinalizationPG.RoleName
        };
        public static readonly string[] AdmissionDecisionFinalizationPagesJUPEB =
       {
            MainMenu.AdmissionDecisionFinalizationJUPEB.RoleName
        };

        public static readonly string[] StudentPages =
        {
            MainMenu.StudentDashboard.RoleName,
            MainMenu.CourseRegistration.RoleName,
            MainMenu.HostelRegistration.RoleName,
            MainMenu.HealthAppointment.RoleName,
        };
        public static readonly string[] ICTPages =
        {
            MainMenu.ICT.RoleName
        };
        public static readonly string[] HealthPages =
        {
            MainMenu.Health.RoleName
        };
        public static readonly string[] AlumniPages = { };
        public static readonly string[] HostelPages = {
            MainMenu.Hostel.RoleName
         };
        public static readonly string[] TranscriptPages = { };


        public static string[] GetByAppUserPage(UserType _AppUserType)
        {
            string[] _PageCollection;

            if (_AppUserType == UserType.Applicant)
            {
                _PageCollection = RolePage.ApplicantPages;
            }
            if (_AppUserType == UserType.Student)
            {
                _PageCollection = RolePage.StudentPages;
            }
            else if (_AppUserType == UserType.AdmissionOfficer)
            {
                _PageCollection = RolePage.AdmissionOfficerPages;
            }
            else if (_AppUserType == UserType.AdmissionOfficerBTH)
            {
                _PageCollection = RolePage.AdmissionOfficerPagesBTH;
            }
            else if (_AppUserType == UserType.AdmissionOfficerPG)
            {
                _PageCollection = RolePage.AdmissionOfficerPagesPG;
            }
            else if (_AppUserType == UserType.AdmissionDecisionFinalization)
            {
                _PageCollection = RolePage.AdmissionDecisionFinalizationPages;
            }
            else if (_AppUserType == UserType.AdmissionDecisionFinalizationJUPEB)
            {
                _PageCollection = RolePage.AdmissionDecisionFinalizationPagesJUPEB;
            }
            else if (_AppUserType == UserType.AdmissionDecisionFinalizationBTH)
            {
                _PageCollection = RolePage.AdmissionDecisionFinalizationPagesBTH;
            }
            else if (_AppUserType == UserType.AdmissionDecisionFinalizationPG)
            {
                _PageCollection = RolePage.AdmissionDecisionFinalizationPagesPG;
            }
            else if (_AppUserType == UserType.Alumni)
            {
                _PageCollection = RolePage.AlumniPages;
            }
            else if (_AppUserType == UserType.Bursary)
            {
                _PageCollection = RolePage.BusaryPages;
            }
            else if (_AppUserType == UserType.Health)
            {
                _PageCollection = RolePage.HealthPages;
            }
            else if (_AppUserType == UserType.Hostel)
            {
                _PageCollection = RolePage.HostelPages;
            }
            else if (_AppUserType == UserType.ICT)
            {
                _PageCollection = RolePage.ICTPages;
            }
            else if (_AppUserType == UserType.Parent)
            {
                _PageCollection = RolePage.ParentPages;
            }
            else if (_AppUserType == UserType.Transcript)
            {
                _PageCollection = RolePage.TranscriptPages;
            }

            else
            {
                _PageCollection = RolePage.ApplicantPages;
            }

            return _PageCollection;
        }
    }
}

