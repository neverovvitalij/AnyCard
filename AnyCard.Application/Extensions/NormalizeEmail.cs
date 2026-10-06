namespace AnyCard.Application.Extensions;

public static class EmailExtensions 
{
    public static string NormalizeEmail(this string email) => email.Trim().ToLowerInvariant();
}
        