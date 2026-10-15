using AnyCard.Domain.Common;
using AnyCard.Domain.Enums;
using AnyCard.Domain.Model;

namespace AnyCard.Application.Services;

public class SpacedRepetitionCalculator
{
    private const double GrowthFactor = 2.0;
    public static TimeSpan CalculateNextReview(UserRating userRating, int currentViewCounter, UserSettings? userSettings) => userRating switch
    {
        UserRating.Again => userSettings?.AgainInterval ?? ReviewIntervalDefaults.Again,
        UserRating.Hard => (userSettings?.HardInterval ?? ReviewIntervalDefaults.Hard) * Math.Pow(GrowthFactor, currentViewCounter),
        UserRating.Good => (userSettings?.GoodInterval ?? ReviewIntervalDefaults.Good) * Math.Pow(GrowthFactor, currentViewCounter),
        UserRating.Easy => (userSettings?.EasyInterval ?? ReviewIntervalDefaults.Easy) * Math.Pow(GrowthFactor, currentViewCounter),
        _ => throw new ArgumentOutOfRangeException()
    };

}
