using System.ComponentModel.DataAnnotations;

namespace AnyCard.DTOs.Auth;

public record RegisterDto(
    [EmailAddress, Required, MaxLength(254)]
    string Email,
    [Required, MinLength(8), MaxLength(100)]
    string Password
);
