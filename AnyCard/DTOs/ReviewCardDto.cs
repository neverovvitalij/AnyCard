using AnyCard.Domain.Enums;

namespace AnyCard.DTOs;

public record ReviewCardDto
(
    int CardId,
    UserRating UserRating
);
