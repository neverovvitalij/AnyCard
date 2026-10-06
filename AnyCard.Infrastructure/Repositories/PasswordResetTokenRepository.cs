using AnyCard.Application.Interfaces;
using AnyCard.Domain.Model;
using AnyCard.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AnyCard.Infrastructure.Repositories;

public class PasswordResetTokenRepository : IPasswordResetTokenRepository
{
    private readonly AnyCardDbContext _anyCardDbContext;
    
    public PasswordResetTokenRepository(AnyCardDbContext anyCardDbContext)
    {
        _anyCardDbContext = anyCardDbContext;
    }

    public async Task AddAsync(PasswordResetToken token)
    {
        await _anyCardDbContext.PasswordResetTokens.AddAsync(token);
    }

    public async Task<bool> SaveChangesAsync()
    {
        return await _anyCardDbContext.SaveChangesAsync() > 0;
    }

    public async Task<PasswordResetToken?> GetLatestUnusedAsync(int userId)
    {
        return await _anyCardDbContext.PasswordResetTokens
            .Where(t => t.UserId == userId && t.IsUsed == false)
            .OrderByDescending(t => t.CreatedAt)
            .FirstOrDefaultAsync();
    }
}
