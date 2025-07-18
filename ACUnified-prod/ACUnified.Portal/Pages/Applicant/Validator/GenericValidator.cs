﻿using FluentValidation;

namespace ACUnified.Portal.Pages.Applicant.NewFolder
{
    public class GenericValidator<T> : AbstractValidator<T>
    {
        public GenericValidator(Action<IRuleBuilderInitial<T,T>> rule)
        {
            rule(RuleFor(x => x));
        }
        private IEnumerable<string> ValidateValue(T arg)
        {
            var result = Validate(arg);
            if (result.IsValid)
                return new string[0];
            return result.Errors.Select(e => e.ErrorMessage);
        }
        public Func<T, IEnumerable<string>> Validation => ValidateValue;
    }
}
