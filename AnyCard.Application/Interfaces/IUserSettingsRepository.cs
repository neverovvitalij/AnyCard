using AnyCard.Domain.Model;

namespace AnyCard.Application.Interfaces;

public interface IUserSettingsRepository
{
    Task AddAsync(UserSettings userSettings);
    Task<UserSettings?> GetByUserIdAsync(int userId);
    Task<bool> SaveChangesAsync();
}
