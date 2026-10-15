using AnyCard.Application.Interfaces;
using AnyCard.Domain.Model;
using AnyCard.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AnyCard.Infrastructure.Repositories;

public class UserSettingsRepository : IUserSettingsRepository
{
    private readonly AnyCardDbContext _anyCardDbContext;
    public UserSettingsRepository(AnyCardDbContext anyCardDbContext)
    {
        _anyCardDbContext = anyCardDbContext;
    }

    public async Task AddAsync(UserSettings userSettings)
    {
        await _anyCardDbContext.UserSettings.AddAsync(userSettings);
    }

    public async Task<UserSettings?> GetByUserIdAsync(int userId)
    {
        return await _anyCardDbContext.UserSettings.FirstOrDefaultAsync(us => us.UserId == userId);
    }

    public async Task<bool> SaveChangesAsync()
    {
        return await _anyCardDbContext.SaveChangesAsync() > 0;
    }
}
