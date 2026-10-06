namespace AnyCard.Domain.Model;

public class PasswordResetToken
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    public DateTime ExpirationDate { get; set; }
    public bool IsUsed { get; set; }
    public int FailedAttempts { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
