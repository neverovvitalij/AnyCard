

using AnyCard.Application.Interfaces;
using AnyCard.Domain.Model;
using AnyCard.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AnyCard.Infrastructure.Repositories;
public class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly AnyCardDbContext _anyCardDbContext;
    public RefreshTokenRepository(AnyCardDbContext anyCardDbContext)
    {
        _anyCardDbContext = anyCardDbContext;
    }

    public async Task AddNewTokenAsync(RefreshToken token)
    {
        await _anyCardDbContext.RefreshTokens.AddAsync(token);
    }

    public async Task<RefreshToken?> GetByTokenAsync(string refreshToken)
    {
        return await _anyCardDbContext.RefreshTokens.Include(t => t.User).Where(u => u.Token == refreshToken).FirstOrDefaultAsync();
    }

    public async Task<bool> SaveChangesAsync()
    {
        return await _anyCardDbContext.SaveChangesAsync() > 0;
    }
}
