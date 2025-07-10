
using FluentValidation;
using ACUnified.Data.DTOs;
using System.Text.RegularExpressions;
namespace ACUnified.Portal.Pages.Applicant.Validator
{
    public class ReferenceValidator : AbstractValidator<ReferenceDto>
    {
        public ReferenceValidator()
        {
            RuleFor(x => x.FullName)
                .NotEmpty()
                .Length(1, 100).WithMessage("FullName cannot be Empty");

            RuleFor(x => x.Designation)
             .NotEmpty()
              .WithMessage("Designation cannot be Empty");

            RuleFor(x => x.Residential)
               .NotEmpty()
              .Length(1, 100).WithMessage("Residential cannot be Empty"); 

            RuleFor(x => x.MobileNumber)
              .Matches(@"^\d{11}$")
            .WithMessage(" mobile number must be 11 digits (0-9)");

            RuleFor(x => x.Email).EmailAddress()
            .Matches(@"^[\w\.-]+@(gmail\.com|outlook\.com|yahoo\.com|email\.com|acu.edu\.ng)$", RegexOptions.IgnoreCase)
            .WithMessage("Invalid email address. Only Gmail, Outlook,Email and Yahoo domains are allowed.")

               .NotEmpty();
              

            RuleFor(x => x.FullName2)
                .MinimumLength(10)
              .WithMessage("FullName not complete");

           // RuleFor(x => x.Gender)
           //    .NotEmpty()
           //   .Length(1, 100)
           //.WithMessage("Gender cannot be Empty");

            RuleFor(x => x.Designation2)
              .Length(1, 100).WithMessage("Designation not complete");

            RuleFor(x => x.Residential2)
               .NotEmpty()
               .Length(1, 100).WithMessage("Residential Address cannot be Empty");

                RuleFor(x => x.Email2).EmailAddress()
                .Matches(@"^[\w\.-]+@(gmail\.com|outlook\.com|yahoo\.com|email\.com)$", RegexOptions.IgnoreCase)
            .WithMessage("Invalid email address. Only Gmail, Outlook, Email and Yahoo domains are allowed.")
               .NotEmpty();

               RuleFor(x => x.MobileNumber2)
               .Matches(@"^\d{11}$")
            .WithMessage(" mobile number must be 11 digits (0-9)");

               RuleFor(x => x.FullName3)
                .NotEmpty()
                .Length(1, 100).WithMessage("FullName cannot be Empty");

            RuleFor(x => x.Designation3)
               .MinimumLength(6)
              .WithMessage("Designation not complete");

            RuleFor(x => x.Residential3)
               .NotEmpty()
              .Length(1, 100).WithMessage("Residential cannot be Empty"); 

            RuleFor(x => x.MobileNumber3)
             .Matches(@"^\d{11}$")
            .WithMessage("mobile number must be 11 digits (0-9)");

            RuleFor(x => x.Email3).EmailAddress()
            .Matches(@"^[\w\.-]+@(gmail\.com|outlook\.com|yahoo\.com|email\.com)$", RegexOptions.IgnoreCase)
            .WithMessage("Invalid email address. Only Gmail, Outlook, Email and Yahoo domains are allowed.")
            .NotEmpty();     
        }

        public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
        {
            var result = await ValidateAsync(ValidationContext<ReferenceDto>.CreateWithOptions((ReferenceDto)model, x => x.IncludeProperties(propertyName)));
            if (result.IsValid)
                return Array.Empty<string>();
            return result.Errors.Select(e => e.ErrorMessage);
        };
    }
    
    
}
