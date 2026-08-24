

namespace AnyCard.Domain.Model;
public class User
{
    public int Id { get; set; }
    public string Username {  get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
}
