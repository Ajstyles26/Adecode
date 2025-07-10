
using FluentValidation;
using ACUnified.Data.DTOs;
using System.Text.RegularExpressions;
namespace ACUnified.Portal.Pages.Applicant.Validator
{
    public class NextOfKinValidator : AbstractValidator<NextOfKinDto>
    {
        public NextOfKinValidator()
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

            RuleFor(x => x.Relationship)
               .NotEmpty()
              .Length(1, 100).WithMessage("Relationship cannot be Empty"); 

            RuleFor(x => x.Email).EmailAddress()
               .Matches(@"^[\w\.-]+@(gmail\.com|outlook\.com|yahoo\.com|email\.com)$", RegexOptions.IgnoreCase)
            .WithMessage("Invalid email address. Only Gmail, Outlook, and Yahoo domains are allowed.")
            .NotEmpty();

            RuleFor(x => x.MobileNumber)
             .Matches(@"^\d{11}$")
            .WithMessage("mobile number must be 11 digits (0-9)");

            RuleFor(x => x.Address)
                .NotEmpty()
               .Length(1, 100)
            .WithMessage("Address cannot be Empty");

           // RuleFor(x => x.Gender)
           //    .NotEmpty()
           //   .Length(1, 100)
           //.WithMessage("Gender cannot be Empty");

            RuleFor(x => x.Occupation)
            .NotEmpty()
            .Length(1, 100)
            .WithMessage("Occupation cannot be Empty");

    

        }

       

        public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
        {
            var result = await ValidateAsync(ValidationContext<NextOfKinDto>.CreateWithOptions((NextOfKinDto)model, x => x.IncludeProperties(propertyName)));
            if (result.IsValid)
                return Array.Empty<string>();
            return result.Errors.Select(e => e.ErrorMessage);
        };
    }
    
    
}
