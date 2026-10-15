namespace AnyCard.Domain.Common;

public static class ReviewIntervalDefaults
{
    public static readonly TimeSpan Again = TimeSpan.FromMinutes(1);
    public static readonly TimeSpan Hard = TimeSpan.FromHours(2);
    public static readonly TimeSpan Good = TimeSpan.FromDays(2);
    public static readonly TimeSpan Easy = TimeSpan.FromDays(6);
}
