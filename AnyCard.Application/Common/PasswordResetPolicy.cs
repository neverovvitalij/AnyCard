namespace AnyCard.Application.Common;

public static class PasswordResetPolicy
{
    public static readonly TimeSpan TokenLifetime = TimeSpan.FromMinutes(15);
    public const int MaxFailedAttempts = 5;
    public static readonly TimeSpan MinRequestInterval = TimeSpan.FromMinutes(1);
}
