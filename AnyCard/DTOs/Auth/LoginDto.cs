using System.ComponentModel.DataAnnotations;


namespace AnyCard.DTOs.Auth;

public record LoginDto
(
    [EmailAddress, Required, MaxLength(254)]
    string Email,
    [Required]
    string Password
);
