using System.ComponentModel.DataAnnotations;

namespace AnyCard.DTOs.Auth;

public record ForgotPasswordDto
(
    [Required, EmailAddress, MaxLength(254)]
    string Email
);
