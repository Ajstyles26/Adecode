using FluentValidation;
using ACUnified.Data.DTOs;
using System.Text.RegularExpressions;

namespace ACUnified.Portal.Pages.Applicant.Validator
{
    public class TransferFormValidator : AbstractValidator<TransferFormDto>
    {
        public TransferFormValidator()
        {
            RuleFor(x => x.Surname)
                .NotEmpty().WithMessage("Surname is required")
                .Length(2, 50).WithMessage("Surname must be between 2 and 50 characters")
                .Matches(@"^[a-zA-Z\s-']+$").WithMessage("Surname can only contain letters, spaces, hyphens and apostrophes");

            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First Name is required")
                .Length(2, 50).WithMessage("First Name must be between 2 and 50 characters")
                .Matches(@"^[a-zA-Z\s-']+$").WithMessage("First Name can only contain letters, spaces, hyphens and apostrophes");

            RuleFor(x => x.Middlename)
                .Length(2, 50).WithMessage("Middle Name must be between 2 and 50 characters when provided")
                .Matches(@"^[a-zA-Z\s-']+$").When(x => !string.IsNullOrEmpty(x.Middlename))
                .WithMessage("Middle Name can only contain letters, spaces, hyphens and apostrophes");

            RuleFor(x => x.MatricNo)
                .NotEmpty().WithMessage("Matric Number is required")
                .Length(5, 20).WithMessage("Matric Number must be between 5 and 20 characters")
                .Matches(@"^[a-zA-Z0-9/-]+$").WithMessage("Matric Number can only contain letters, numbers, forward slashes and hyphens");

            RuleFor(x => x.Institution)
                .NotEmpty().WithMessage("Institution name is required")
                .Length(5, 100).WithMessage("Institution name must be between 5 and 100 characters");

            RuleFor(x => x.Level)
                .NotEmpty().WithMessage("Level is required")
                .Matches(@"^[1-5][0-9]{2}$").WithMessage("Level must be a valid academic level (e.g., 100, 200, 300, etc.)");

            RuleFor(x => x.Cgpa)
                .NotEmpty().WithMessage("CGPA is required")
                .InclusiveBetween(0.00m, 5.00m).WithMessage("CGPA must be between 0.00 and 5.00")
                .ScalePrecision(2, 3).WithMessage("CGPA must have at most 2 decimal places");

            RuleFor(x => x)
                .Must(x => x.OLevelDeficiency || x.OLevelSubjectsNotRelevant || x.NoJAMBAdmission || 
                          x.PoorAcademicPerformance || x.FinancialReason)
                .WithMessage("At least one reason for transfer must be selected");

            When(x => x.FinancialReason, () =>
            {
                RuleFor(x => x.FinancialReasonExplanation)
                    .NotEmpty().WithMessage("Financial reason explanation is required when financial reason is selected")
                    .MinimumLength(20).WithMessage("Financial reason explanation must be at least 20 characters")
                    .MaximumLength(500).WithMessage("Financial reason explanation cannot exceed 500 characters");
            });

            RuleFor(x => x.Crime)
                .NotEmpty().WithMessage("Crime field is required. Write 'nil' if not applicable")
                .MaximumLength(200).WithMessage("Crime description cannot exceed 200 characters");

            RuleFor(x => x.Immorality)
                .NotEmpty().WithMessage("Immorality/Indiscipline/Indecency field is required. Write 'nil' if not applicable")
                .MaximumLength(200).WithMessage("Immorality description cannot exceed 200 characters");

            RuleFor(x => x.Cultism)
                .NotEmpty().WithMessage("Cultism/Bullying/Gangsterism/Drugs field is required. Write 'nil' if not applicable")
                .MaximumLength(200).WithMessage("Cultism description cannot exceed 200 characters");

            RuleFor(x => x.OtherReasons)
                .MaximumLength(500).WithMessage("Other reasons cannot exceed 500 characters");
        }

        public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
        {
            var result = await ValidateAsync(ValidationContext<TransferFormDto>.CreateWithOptions((TransferFormDto)model, x => x.IncludeProperties(propertyName)));
            if (result.IsValid)
                return Array.Empty<string>();
            return result.Errors.Select(e => e.ErrorMessage);
        };
    }
}