
using System.Text.RegularExpressions;
using ACUnified.Data.DTOs;
using FluentValidation;

namespace ACUnified.Portal.Pages.Applicant.Validator
{
    public class BiodataValidator : AbstractValidator<BioDataDto>
    {
        public BiodataValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty()
                .Length(1, 100).WithMessage("Firstname cannot be Empty");

            /*RuleFor(x => x.OtherName)
                 .NotEmpty()
                .Length(1, 100).WithMessage("Othername cannot be Empty"); */

            RuleFor(x => x.LastName)
                .NotEmpty()
               .Length(1, 100).WithMessage("Lastname cannot be Empty"); 

            RuleFor(x => x.State)
               .NotEmpty()
              .Length(1, 100).WithMessage("State cannot be Empty"); 

            RuleFor(x => x.Email)
            .Matches(@"^[\w\.-]+@(gmail\.com|outlook\.com|yahoo\.com|email\.com)$", RegexOptions.IgnoreCase)
            .WithMessage("Invalid email address. Only Gmail, Outlook, and Yahoo domains are allowed.")
               .NotEmpty();

            RuleFor(x => x.MobileNumber)
              .Matches(@"^\d{11}$")
            .WithMessage(" mobile number must be 10 digits (0-9)");

            RuleFor(x => x.Address)
                .NotEmpty()
               .Length(1, 100)
            .WithMessage("Address cannot be Empty");

           // RuleFor(x => x.Gender)
           //    .NotEmpty()
           //   .Length(1, 100)
           //.WithMessage("Gender cannot be Empty");

            RuleFor(x => x.MaritalStatus)
            .NotEmpty()
            .Length(1, 100)
            .WithMessage("Marital Status cannot be Empty");

            RuleFor(x => x.DOB).NotEmpty().Must(BeEligible).WithMessage("Date of Birth must be greater than 16 and not empty");

        }

        private bool BeEligible(DateTime? dateOfBirth)
        {
            var today = DateTime.Today;
            var age = today.Year - dateOfBirth.Value.Year;

            // Adjust age if the birthday hasn't occurred yet this year
            if (dateOfBirth.Value.Date > today.AddYears(-age))
            {
                age--;
            }

            return age >= 10;
        }

        public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
        {
            var result = await ValidateAsync(ValidationContext<BioDataDto>.CreateWithOptions((BioDataDto)model, x => x.IncludeProperties(propertyName)));
            if (result.IsValid)
                return Array.Empty<string>();
            return result.Errors.Select(e => e.ErrorMessage);
        };
    }
    
    
}
