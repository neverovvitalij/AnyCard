using System.ComponentModel.DataAnnotations;

namespace AnyCard.DTOs.Auth;

public record ResetPasswordDto
(
    [ Required, EmailAddress, MaxLength(254)]
    string Email,
    [ Required, MaxLength(16)]
    string Code,
    [ Required, MinLength(8), MaxLength(100)]
    string NewPassword
);
