namespace AnyCard.DTOs;

public record CreateCardDto
(
    string Question,
    string Answer,
    int CategoryId
);
