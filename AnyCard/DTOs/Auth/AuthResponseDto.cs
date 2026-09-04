namespace AnyCard.DTOs.Auth;

public record AuthResponseDto
(
    string RefreshToken,
    string AccessToken
);
