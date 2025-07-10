
using ACUnified.Data.DTOs;
using FluentValidation;

namespace ACUnified.Portal.Pages.Applicant.Validator
{
    public class AcademicQualificationValidator : AbstractValidator<AcademicQualificationDto>
    {
        public AcademicQualificationValidator()
        {
            RuleFor(x => x.ExamType)
               .NotEmpty()
               .Length(1, 100).WithMessage(" ExamType cannot be Empty");

            RuleFor(x => x.UTMERegNo)
                .NotEmpty()
                .Length(1, 100).WithMessage(" UTMERegNo cannot be Empty");

            /*RuleFor(x => x.OtherName)
                 .NotEmpty()
                .Length(1, 100).WithMessage("Othername cannot be Empty"); */

            RuleFor(x => x.ExamNumber)
                .NotEmpty()
               .Length(1, 1000).WithMessage("Exam  Number cannot be Empty"); 

            RuleFor(x => x.UTMEScore)
               .NotEmpty()
               .InclusiveBetween(0, 400)
               .WithMessage("UTMEScore cannot be Empty and must be between 0 and 400");

            RuleFor(x => x.Subject1).NotEqual(x => x.Subject2).NotEqual(x => x.Subject3).NotEqual(x => x.Subject4).NotEqual(x => x.Subject5)
               .NotEmpty()
              .Length(1, 100).WithMessage("Subject cannot be Empty");

            RuleFor(x => x.Grade1)
               .NotEmpty()
              .Length(1, 100).WithMessage("Grade1 cannot be Empty");
              RuleFor(x => x.Subject2).NotEqual(x => x.Subject1).NotEqual(x => x.Subject3).NotEqual(x => x.Subject4).NotEqual(x => x.Subject5)
               .NotEmpty()
              .Length(1, 100).WithMessage("Subject cannot be Empty");

            RuleFor(x => x.Grade2)
               .NotEmpty()
              .Length(1, 100).WithMessage("Grade2 cannot be Empty");  
              RuleFor(x => x.Subject3).NotEqual(x => x.Subject2).NotEqual(x => x.Subject1).NotEqual(x => x.Subject4).NotEqual(x => x.Subject5)
               .NotEmpty()
              .Length(1, 100).WithMessage("Subject cannot be Empty");

            RuleFor(x => x.Grade3)
               .NotEmpty()
              .Length(1, 100).WithMessage("Grade3 cannot be Empty");  
              RuleFor(x => x.Subject4).NotEqual(x => x.Subject2).NotEqual(x => x.Subject3).NotEqual(x => x.Subject1).NotEqual(x => x.Subject5)
               .NotEmpty()
              .Length(1, 100).WithMessage("Subject cannot be Empty");
               RuleFor(x => x.UTMESubject1).NotEqual(x => x.UTMESubject2).NotEqual(x => x.UTMESubject3).NotEqual(x => x.UTMESubject4) .NotEmpty()
                .Length(1, 100).WithMessage("Subject cannot be Empty");

             RuleFor(x => x.UTMESubscore1)
               .NotEmpty()
               .InclusiveBetween(0, 100)
               .WithMessage("UTMEScore cannot be Empty and must be between 0 and 100");

               RuleFor(x => x.UTMESubject2).NotEqual(x => x.UTMESubject1).NotEqual(x => x.UTMESubject3).NotEqual(x => x.UTMESubject4) .NotEmpty()
                .Length(1, 100).WithMessage("Subject cannot be Empty");

             RuleFor(x => x.UTMESubscore2)
               .NotEmpty()
               .InclusiveBetween(0, 100)
               .WithMessage("UTMEScore cannot be Empty and must be between 0 and 100");

               RuleFor(x => x.UTMESubject3).NotEqual(x => x.UTMESubject2).NotEqual(x => x.UTMESubject1).NotEqual(x => x.UTMESubject4) .NotEmpty()
                .Length(1, 100).WithMessage("Subject cannot be Empty");

             RuleFor(x => x.UTMESubscore3)
               .NotEmpty()
               .InclusiveBetween(0, 100)
               .WithMessage("UTMEScore cannot be Empty and must be between 0 and 100");

               RuleFor(x => x.UTMESubject4).NotEqual(x => x.UTMESubject2).NotEqual(x => x.UTMESubject3).NotEqual(x => x.UTMESubject1) .NotEmpty()
                .Length(1, 100).WithMessage("Subject cannot be Empty");

             RuleFor(x => x.UTMESubscore4)
               .NotEmpty()
               .InclusiveBetween(0, 100)
               .WithMessage("UTMEScore4 cannot be Empty and must be between 0 and 100"); 
            RuleFor(x => x.Grade4)
               .NotEmpty()
              .Length(1, 100).WithMessage("Grade4 cannot be Empty");  
              RuleFor(x => x.Subject5).NotEqual(x => x.Subject2).NotEqual(x => x.Subject3).NotEqual(x => x.Subject4).NotEqual(x => x.Subject1)
               .NotEmpty()
              .Length(1, 100).WithMessage("Subject cannot be Empty");

            RuleFor(x => x.Modeofentry)
               .NotEmpty()
              .WithMessage("Mode of entry cannot be Empty");
            RuleFor(x => x.Modeofentry)
                .IsInEnum()
            .WithMessage("Mode of Entry cannot be Empty");
           

           // RuleFor(x => x.Gender)
           //    .NotEmpty()
           //   .Length(1, 100)
           //.WithMessage("Gender cannot be Empty");

            RuleFor(x => x.Choice1)
            .NotEmpty()
            .Length(1, 100)
            .WithMessage("Choice1 cannot be Empty");

            RuleFor(x => x.Choice2)
            .NotEmpty()
            .Length(1, 100)
            .WithMessage("Choice2 Number cannot be Empty");
           

        }

        public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
        {
            var result = await ValidateAsync(ValidationContext<AcademicQualificationDto>.CreateWithOptions((AcademicQualificationDto)model, x => x.IncludeProperties(propertyName)));
            if (result.IsValid)
                return Array.Empty<string>();
            return result.Errors.Select(e => e.ErrorMessage);
        };
    }
    
    
}
