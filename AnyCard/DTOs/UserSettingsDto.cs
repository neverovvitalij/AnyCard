namespace AnyCard.DTOs;

public record UserSettingsDto
(
    TimeSpan Again,
    TimeSpan Hard,
    TimeSpan Good,
    TimeSpan Easy
);
