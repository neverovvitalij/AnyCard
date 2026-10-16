namespace AnyCard.Domain.Common;

public static class ReviewIntervalLimits
{
    public static readonly TimeSpan MinSetting = TimeSpan.FromMinutes(1);
    public static readonly TimeSpan MaxSetting = TimeSpan.FromDays(30);
    public static readonly TimeSpan MaxCalculated = TimeSpan.FromDays(365);
}
