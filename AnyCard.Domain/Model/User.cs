

namespace AnyCard.Domain.Model;
public class User
{
    public int Id { get; set; }
    public string Email {  get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    public ICollection<Category> Categories { get; set; } = new List<Category>();
    public ICollection<PasswordResetToken> PasswordResetTokens { get; set; } = new List<PasswordResetToken>();
}
