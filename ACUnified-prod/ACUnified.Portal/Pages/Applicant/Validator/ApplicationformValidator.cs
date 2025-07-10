
using FluentValidation;
using ACUnified.Data.DTOs;
using System.Text.RegularExpressions;
namespace ACUnified.Portal.Pages.Applicant.Validator
{
    public class ApplicationFormValidator : AbstractValidator<ApplicationFormDto>
    {
        public ApplicationFormValidator()
        {
            RuleFor(x => x.Iagree)
            .Equal(true)
            .WithMessage("rewrite 'I agree to this terms and conditions'");
        }

        public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
        {
            var result = await ValidateAsync(ValidationContext<ApplicationFormDto>.CreateWithOptions((ApplicationFormDto)model, x => x.IncludeProperties(propertyName)));
            if (result.IsValid)
                return Array.Empty<string>();
            return result.Errors.Select(e => e.ErrorMessage);
        };
    }


}
