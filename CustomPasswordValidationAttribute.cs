using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

public class CustomPasswordValidationAttribute : ValidationAttribute
{
    private readonly string _usernameProperty;

    public CustomPasswordValidationAttribute(string usernameProperty)
    {
        _usernameProperty = usernameProperty;
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var password = value as string;
        var usernameProp = validationContext.ObjectType.GetProperty(_usernameProperty);
        var username = usernameProp?.GetValue(validationContext.ObjectInstance) as string ?? "";

        if (password == null)
            return ValidationResult.Success;

        bool hasUpper = password.Any(char.IsUpper);
        bool hasLower = password.Any(char.IsLower);
        bool hasDigit = password.Any(char.IsDigit);
        bool hasSpecial = password.Any(ch => !char.IsLetterOrDigit(ch));
        bool hasArmenian = password.Any(ch => ch >= 0x0530 && ch <= 0x058F);
        bool containsUsername = password.Contains(username);

        if (!hasUpper || !hasLower || !hasDigit || !hasSpecial || !hasArmenian || containsUsername)
        {
            return new ValidationResult("Password must contain uppercase, lowercase, digit, special character, Armenian letter, and not contain the username.");
        }

        return ValidationResult.Success;
    }
}
