using AnyCard.Domain.Enums;

namespace AnyCard.DTOs;

public record CardOverviewDto
(
    int CardId,
    string CategoryName,
    string Question,
    UserRating UserRating,
    int CurrentStreak,
    DateTime NextShowtime
);
