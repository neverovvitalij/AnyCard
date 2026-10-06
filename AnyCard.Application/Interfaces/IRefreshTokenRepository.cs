

using AnyCard.Domain.Model;

namespace AnyCard.Application.Interfaces;
public interface IRefreshTokenRepository
{
    Task AddNewTokenAsync(RefreshToken token);
    Task<RefreshToken?> GetByTokenAsync(string refreshToken);
    Task RevokeAllForUserAsync(int userId);
    Task<bool> SaveChangesAsync();
}
