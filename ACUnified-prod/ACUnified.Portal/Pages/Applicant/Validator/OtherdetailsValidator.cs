
using FluentValidation;
using ACUnified.Data.DTOs;
using System.Text.RegularExpressions;
namespace ACUnified.Portal.Pages.Applicant.Validator
{
    public class OtherDetailsValidator : AbstractValidator<OtherDetailsDto>
    {
        public OtherDetailsValidator()
        {
            RuleFor(x => x.RedAddress)
                .NotEmpty()
                .Length(1, 100).WithMessage("Residential Address cannot be Empty");

            /*RuleFor(x => x.OtherName)
                 .NotEmpty()
                .Length(1, 100).WithMessage("Othername cannot be Empty"); */

            RuleFor(x => x.PostalAddress)
               .MinimumLength(6)
              .WithMessage("Postal Address not complete");

            RuleFor(x => x.Denomination)
               .NotEmpty()
              .Length(1, 100).WithMessage("Denomination cannot be Empty"); 

            RuleFor(x => x.ParentName)
               .NotEmpty()
              .Length(1, 100).WithMessage("ParentName cannot be Empty");

            RuleFor(x => x.ContactAddress)
               .NotEmpty()
              .Length(1, 100).WithMessage("Contact Address cannot be Empty");

            RuleFor(x => x.FatherNo)
               .Matches(@"^\d{11}$")
            .WithMessage("Father's mobile number must be 11 digits (0-9)");
           // RuleFor(x => x.Gender)
           //    .NotEmpty()
           //   .Length(1, 100)
           //.WithMessage("Gender cannot be Empty");

            RuleFor(x => x.MotherNo)
               .Matches(@"^\d{11}$")
            .WithMessage("Mother's mobile number must be 11 digits (0-9)");

                 RuleFor(x => x.Disability)
                  .NotEmpty()
              .WithMessage("Must not be  empty");

            //   RuleFor(x => x.Disabilitycomment)
            //       .NotEmpty()
            //   .WithMessage("Must not be  empty");
            RuleFor(x => x.ParentEmail).EmailAddress()
                        .Matches(@"^[\w\.-]+@(gmail\.com|outlook\.com|yahoo\.com|email\.com)$", RegexOptions.IgnoreCase)
            .WithMessage("Invalid email address. Only Gmail, Outlook, and Yahoo domains are allowed.")
               .NotEmpty();
           

        }

        public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
        {
            var result = await ValidateAsync(ValidationContext<OtherDetailsDto>.CreateWithOptions((OtherDetailsDto)model, x => x.IncludeProperties(propertyName)));
            if (result.IsValid)
                return Array.Empty<string>();
            return result.Errors.Select(e => e.ErrorMessage);
        };
    }
    
    
}
