using AnyCard.Domain.Common;
using AnyCard.Domain.Enums;
using AnyCard.Domain.Model;

namespace AnyCard.Application.Services;

public class SpacedRepetitionCalculator
{
    private const double GrowthFactor = 2.0;
    private const int MaxGrowthSteps = 10;
    public static TimeSpan CalculateNextReview(UserRating userRating, int currentViewCounter, UserSettings? userSettings)
    {
        var growth = Math.Pow(GrowthFactor, Math.Min(currentViewCounter, MaxGrowthSteps));
        var interval = userRating switch
        {
            UserRating.Again => userSettings?.AgainInterval ?? ReviewIntervalDefaults.Again,
            UserRating.Hard => (userSettings?.HardInterval ?? ReviewIntervalDefaults.Hard) * growth,
            UserRating.Good => (userSettings?.GoodInterval ?? ReviewIntervalDefaults.Good) * growth,
            UserRating.Easy => (userSettings?.EasyInterval ?? ReviewIntervalDefaults.Easy) * growth,
            _ => throw new ArgumentOutOfRangeException()
        };

        return interval > ReviewIntervalLimits.MaxCalculated ? ReviewIntervalLimits.MaxCalculated : interval;
    }

}
