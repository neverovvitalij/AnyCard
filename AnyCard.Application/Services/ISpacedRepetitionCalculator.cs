
using AnyCard.Domain.Enums;

namespace AnyCard.Application.Services;
public class SpacedRepetitionCalculator
{
    private const double GrowthFactor = 2.0;
    public static TimeSpan CalculateNextReview(UserRating userRating, int currentViewCounter) => userRating switch
    {
        UserRating.Again => TimeSpan.FromMinutes(20),
        UserRating.Hard => TimeSpan.FromHours(2) * Math.Pow(GrowthFactor, currentViewCounter),
        UserRating.Good => TimeSpan.FromDays(3) * Math.Pow(GrowthFactor, currentViewCounter),
        UserRating.Easy => TimeSpan.FromDays(7) * Math.Pow(GrowthFactor, currentViewCounter),
        _ => throw new ArgumentOutOfRangeException()
    };
    
}
