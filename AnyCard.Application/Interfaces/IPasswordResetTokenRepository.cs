using AnyCard.Domain.Model;

namespace AnyCard.Application.Interfaces;

public interface IPasswordResetTokenRepository
{
    Task AddAsync(PasswordResetToken token);
    Task<PasswordResetToken?> GetLatestUnusedAsync(int userId);
    Task<bool> SaveChangesAsync();
}
