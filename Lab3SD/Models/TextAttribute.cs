using System.ComponentModel.DataAnnotations;
using Microsoft.IdentityModel.Tokens;

namespace Lab3SD.Models;

[AttributeUsage(AttributeTargets.Property | 
                AttributeTargets.Field, AllowMultiple = false)]
public class TextAttribute : ValidationAttribute
{
    public override bool IsValid(object value)
    {
        var role = (string)value;
        return value is string && !role.IsNullOrEmpty() && !string.IsNullOrWhiteSpace(role);
    }
}