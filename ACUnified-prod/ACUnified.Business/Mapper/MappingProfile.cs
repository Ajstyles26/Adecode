using ACUnified.Data.DTOs;
using ACUnified.Data.Models;
using AutoMapper;

namespace ACUnified.Business.Mapper
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<ApplicantPayUploadCategory, ApplicantPayUploadCategoryDto>().ReverseMap();
            CreateMap<ApplicantPayUploadCategoryBatch, ApplicantPayUploadCategoryBatchDto>().ReverseMap();
            CreateMap<Course, CourseDto>().ReverseMap();
            CreateMap<AcademicQualification, AcademicQualificationDto>().ReverseMap();
            CreateMap<CourseRegistration, CourseRegistrationDto>().ReverseMap();
            CreateMap<ApplicationProgram, ApplicationProgramDto>().ReverseMap();
            CreateMap<BioData, BioDataDto>().ReverseMap();
            CreateMap<CompanyInfo, CompanyInfoDto>().ReverseMap();
            CreateMap<Department, DepartmentDto>().ReverseMap();
            CreateMap<Designation, DesignationDto>().ReverseMap();
            CreateMap<Event, EventDto>().ReverseMap();
            CreateMap<EducationalRecord, EducationalRecordDto>().ReverseMap();
            CreateMap<TransferForm, TransferFormDto>().ReverseMap();
            CreateMap<Degree, DegreeDto>().ReverseMap();
            CreateMap<Faculty, FacultyDto>().ReverseMap();
            CreateMap<Level, LevelDto>().ReverseMap();
            CreateMap<LGA, LGADto>().ReverseMap();
            CreateMap<NextOfKin, NextOfKinDto>().ReverseMap();
            CreateMap<OtherDetails, OtherDetailsDto>().ReverseMap();
            CreateMap<Reference,ReferenceDto>().ReverseMap();
            CreateMap<ProgramChoice, ProgramChoiceDto>().ReverseMap();
            CreateMap<Session, SessionDto>().ReverseMap();
            CreateMap<State, StateDto>().ReverseMap();
            CreateMap<Student, StudentDto>().ReverseMap();
            CreateMap<StudentEnrolment, StudentEnrolmentDto>().ReverseMap();
            CreateMap<SubjectSetup, SubjectSetupDto>().ReverseMap();
            CreateMap<Subject, SubjectDto>().ReverseMap();
            CreateMap<UTMESubject,UTMESubjectDto>().ReverseMap();
            CreateMap<HealthAppointment, HealthAppointmentDto>().ReverseMap();
            CreateMap<ProgramSetup, ProgramSetupDto>().ReverseMap();
            CreateMap<PayDetails, PayDetailsDto>().ReverseMap();
            CreateMap<PayCategory, PayCategoryDto>().ReverseMap();
            CreateMap<PayUploadCategory, PayUploadCategoryDto>().ReverseMap();
            CreateMap<PayUploadCategoryBatch, PayUploadCategoryBatchDto>().ReverseMap();
            CreateMap<Payment, PaymentDto>().ReverseMap();
            CreateMap<Semester, SemesterDto>().ReverseMap();
            CreateMap<Country, CountryDto>().ReverseMap();
            CreateMap<ApplicantPayCategory, ApplicantPayCategoryDto>().ReverseMap();
            CreateMap<ApplicantPayDetails, ApplicantPayDetailsDto>().ReverseMap();
            CreateMap<ApplicantPaymentLog, ApplicantPaymentLogDto>().ReverseMap();
            CreateMap<ApplicantPayment, ApplicantPaymentDto>().ReverseMap();
            CreateMap<ApplicationDocument, ApplicationDocumentDto>().ReverseMap();
            CreateMap<ApplicationForm, ApplicationFormDto>().ReverseMap();
            CreateMap<ApplicationProgram, ApplicationProgramDto>().ReverseMap();
            CreateMap<DebtorsList, DebtorsListDto>().ReverseMap();
            CreateMap<HostelBuilding, HostelBuildingDto>().ReverseMap();
            CreateMap<HostelAllocation, HostelAllocationDto>().ReverseMap();
            CreateMap<HostelRooms, HostelRoomsDto>().ReverseMap();
            CreateMap<PayUploadDetails, PayUploadDetailsDto>().ReverseMap();
            CreateMap<AdmittedStudent, Admitted>().ReverseMap();
            CreateMap<PayUploadConcession, PayUploadConcessionDto>().ReverseMap();
            CreateMap<RemitaNotification, RemitaNotificationDto>().ReverseMap();
            CreateMap<StudentResult, StudentResultDto>().ReverseMap();
            CreateMap<StudentRegistrationDto, StudentRegistrationDto>(); 
            //Mapper extraordinaire
            CreateMap<PayUploadDetailsDto, PayDetails>(). ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<PayDetails , PayUploadDetailsDto>().ForMember(dest => dest.Id, opt => opt.Ignore());


            // Forward mapping configuration
            CreateMap<PayUploadCategoryDto, PayCategoryDto>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            // Reverse mapping configuration (including the custom configuration for ignoring the Id property)
            CreateMap<PayCategoryDto, PayUploadCategoryDto>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<PayUploadDetailsDto,PayDetailsDto>().ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<PayDetailsDto, PayUploadDetailsDto> ().ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<PayUploadDetailsDto, PayDetailsDto>().ForMember(dest => dest.Student, opt => opt.Ignore());

            CreateMap<PayDetailsDto, PayUploadDetailsDto>().ForMember(dest => dest.Student, opt => opt.Ignore());


            CreateMap<PayUploadConcessionDto, PayConcessionDto>().ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<PayConcessionDto,  PayUploadConcessionDto> ().ReverseMap().ForMember(dest => dest.Id, opt => opt.Ignore());
            CreateMap<ProgramSetup, ProgramSetupDto>()
    .ForMember(dest => dest.Department, opt => opt.MapFrom(src => src.Department))
    .ForMember(dest => dest.Faculty, opt => opt.MapFrom(src => src.Faculty));
    CreateMap<ApplicantPayment, ApplicantPaymentDto>()
    .ForMember(dest => dest.ApplicantPayCategory, opt => opt.MapFrom(src => src.ApplicantPayCategory))
    .ForMember(dest => dest.ApplicantPayDetails, opt => opt.MapFrom(src => src.ApplicantPayDetails))
    .ForMember(dest => dest.Semester, opt => opt.MapFrom(src => src.Semester))
     .ForMember(dest => dest.ApplicationForm, opt => opt.MapFrom(src => src.ApplicationForm));
            CreateMap<EducationalRecord, EducationalRecordDto>()
                       .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                       .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                       .ForMember(dest => dest.Qualification, opt => opt.MapFrom(src => src.Qualification))
                       .ForMember(dest => dest.Grade, opt => opt.MapFrom(src => src.Grade))
                       .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.StartDate))
                       .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.EndDate));
                     


            CreateMap<HostelAllocation, HostelAllocationDto>()
            .ForMember(dest => dest.HostelRoom, opt => opt.MapFrom(src => src.HostelRoom))
            .ForMember(dest => dest.Student, opt => opt.MapFrom(src => src.Student))
            .ForMember(dest => dest.ApplicationForm, opt => opt.MapFrom(src => src.ApplicationForm));
             CreateMap<Course, CourseDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.CourseCode, opt => opt.MapFrom(src => src.CourseCode))
                .ForMember(dest => dest.CourseUnit, opt => opt.MapFrom(src => src.CourseUnit))
                .ForMember(dest => dest.Semester, opt => opt.MapFrom(src => src.Semester))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.courseCategory, opt => opt.MapFrom(src => src.courseCategory))
                // .ForMember(dest => dest.FacultyId, opt => opt.MapFrom(src => src.FacultyId))
                .ForMember(dest => dest.DegreeId, opt => opt.MapFrom(src => src.DegreeId))
                .ForMember(dest => dest.DepartmentId, opt => opt.MapFrom(src => src.DepartmentId))
                // .ForMember(dest => dest.ProgramSetupId, opt => opt.MapFrom(src => src.ProgramSetupId))
                .ForMember(dest => dest.LevelId, opt => opt.MapFrom(src => src.LevelId))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.Prerequisite, opt => opt.MapFrom(src => src.Prerequisite))
                .ForMember(dest => dest.InstitutionProviderId, opt => opt.MapFrom(src => src.InstitutionProviderId));
      
        CreateMap<HostelRooms, HostelRoomsDto>()
            .ForMember(dest => dest.HostelBuilding, opt => opt.MapFrom(src => src.HostelBuilding));

        CreateMap<HostelBuilding, HostelBuildingDto>();
        CreateMap<ApplicantDebtorList, ApplicantDebtorListDto>().ReverseMap();
         CreateMap<HostelDebtorList, HostelDebtorListDto>().ReverseMap();

        }
    }
}

