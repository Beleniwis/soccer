using System.Text.RegularExpressions;

namespace Soccer.Validators;

public static class NameValidation
{
    public static bool IsValid(string? name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return false;
        }

        name = name.Trim();

        if (name.Length < 2)
        {
            return false;
        }

        return Regex.IsMatch(
            name,
            @"^[\p{L}]+(?:[\s'-][\p{L}]+)*$");
    }
}