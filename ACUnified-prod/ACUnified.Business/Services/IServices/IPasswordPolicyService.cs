using System.Text.RegularExpressions;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace ACUnified.Business.Services
{
    public interface IPasswordPolicyService
    {
        bool ValidatePassword(string password);
        string GetPasswordRequirements();
        bool IsCommonPassword(string password);
        (bool isValid, List<string> failedRules) ValidatePasswordWithDetails(string password);
    }

    public class PasswordPolicyConfiguration
    {
        public int MinimumLength { get; set; } = 8;
        public int MaximumLength { get; set; } = 128;
        public bool RequireUppercase { get; set; } = true;
        public bool RequireLowercase { get; set; } = true;
        public bool RequireDigits { get; set; } = true;
        public bool RequireSpecialCharacters { get; set; } = true;
        public int MinimumUniqueCharacters { get; set; } = 4;
        public bool PreventCommonPasswords { get; set; } = true;
    }

    public class PasswordPolicyService : IPasswordPolicyService
    {
        private readonly PasswordPolicyConfiguration _configuration;
        private readonly HashSet<string> _commonPasswords;

        public PasswordPolicyService(IOptions<PasswordPolicyConfiguration> configuration)
        {
            _configuration = configuration.Value;
            _commonPasswords = InitializeCommonPasswords();
        }

        public bool ValidatePassword(string password)
        {
            var (isValid, _) = ValidatePasswordWithDetails(password);
            return isValid;
        }

        public (bool isValid, List<string> failedRules) ValidatePasswordWithDetails(string password)
        {
            var failedRules = new List<string>();

            if (string.IsNullOrWhiteSpace(password))
            {
                failedRules.Add("Password cannot be empty.");
                return (false, failedRules);
            }

            // Check length
            if (password.Length < _configuration.MinimumLength)
                failedRules.Add($"Password must be at least {_configuration.MinimumLength} characters long.");

            if (password.Length > _configuration.MaximumLength)
                failedRules.Add($"Password must not exceed {_configuration.MaximumLength} characters.");

            // Check character requirements
            if (_configuration.RequireUppercase && !password.Any(char.IsUpper))
                failedRules.Add("Password must contain at least one uppercase letter.");

            if (_configuration.RequireLowercase && !password.Any(char.IsLower))
                failedRules.Add("Password must contain at least one lowercase letter.");

            if (_configuration.RequireDigits && !password.Any(char.IsDigit))
                failedRules.Add("Password must contain at least one number.");

            if (_configuration.RequireSpecialCharacters && !ContainsSpecialCharacter(password))
                failedRules.Add("Password must contain at least one special character.");

            // Check unique characters
            if (password.Distinct().Count() < _configuration.MinimumUniqueCharacters)
                failedRules.Add($"Password must contain at least {_configuration.MinimumUniqueCharacters} unique characters.");

            // Check for common passwords
            if (_configuration.PreventCommonPasswords && IsCommonPassword(password))
                failedRules.Add("This password is too common. Please choose a more unique password.");

            // Check for repeating characters
            if (HasRepeatingCharacters(password))
                failedRules.Add("Password cannot contain repeating characters more than 3 times in a row.");

            // Check for keyboard patterns
            if (ContainsKeyboardPattern(password))
                failedRules.Add("Password cannot contain common keyboard patterns.");

            return (!failedRules.Any(), failedRules);
        }

        public string GetPasswordRequirements()
        {
            var requirements = new List<string>
            {
                $"At least {_configuration.MinimumLength} characters long",
                $"Maximum {_configuration.MaximumLength} characters"
            };

            if (_configuration.RequireUppercase)
                requirements.Add("At least one uppercase letter");
            
            if (_configuration.RequireLowercase)
                requirements.Add("At least one lowercase letter");
            
            if (_configuration.RequireDigits)
                requirements.Add("At least one number");
            
            if (_configuration.RequireSpecialCharacters)
                requirements.Add("At least one special character");

            if (_configuration.MinimumUniqueCharacters > 0)
                requirements.Add($"At least {_configuration.MinimumUniqueCharacters} unique characters");

            return "Password requirements:\n• " + string.Join("\n• ", requirements);
        }

        public bool IsCommonPassword(string password)
        {
            return _commonPasswords.Contains(password.ToLower());
        }

        private bool ContainsSpecialCharacter(string password)
        {
            return Regex.IsMatch(password, @"[!@#$%^&*(),.?""':{}|<>]");
        }

        private bool HasRepeatingCharacters(string password)
        {
            return Regex.IsMatch(password, @"(.)\1{3,}");
        }

        private bool ContainsKeyboardPattern(string password)
        {
            var commonPatterns = new[]
            {
                "qwerty", "asdfgh", "zxcvbn", "123456", "password",
                "qwertz", "azerty", "ytrewq", "hgfdsa", "nbvcxz"
            };

            return commonPatterns.Any(pattern => 
                password.ToLower().Contains(pattern) || 
                password.ToLower().Contains(new string(pattern.Reverse().ToArray())));
        }

        private HashSet<string> InitializeCommonPasswords()
        {
            // This is a small subset of common passwords. In a production environment,
            // you would want to load this from a more comprehensive file or database
            return new HashSet<string>
            {
                "password",
                "123456",
                "12345678",
                "qwerty",
                "abc123",
                "football",
                "letmein",
                "monkey",
                "696969",
                "shadow",
                "master",
                "666666",
                "qwertyuiop",
                "123321",
                "mustang",
                "123456789",
                "michael",
                "654321",
                "superman",
                "1qaz2wsx",
                "7777777",
                "1234567",
                "dragon",
                "121212",
                "000000",
                "qazwsx",
                "123qwe",
                "killer",
                "trustno1",
                "jordan",
                "jennifer",
                "password1",
                "hunter",
                "welcome"
            };
        }
    }

    // Extension method for service registration
    public static class PasswordPolicyServiceExtensions
    {
        public static IServiceCollection AddPasswordPolicyService(
            this IServiceCollection services,
            Action<PasswordPolicyConfiguration> configureOptions = null)
        {
            if (configureOptions != null)
            {
                services.Configure(configureOptions);
            }
            else
            {
                services.Configure<PasswordPolicyConfiguration>(options => { });
            }

            services.AddScoped<IPasswordPolicyService, PasswordPolicyService>();
            
            return services;
        }
    }
}