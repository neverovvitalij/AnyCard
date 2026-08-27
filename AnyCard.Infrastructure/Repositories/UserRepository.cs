
using AnyCard.Application.Interfaces;
using AnyCard.Domain.Model;
using AnyCard.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AnyCard.Infrastructure.Repositories;
public class UserRepository : IUserRepository
{
    private readonly AnyCardDbContext _anyCardDbContext;
    public UserRepository(AnyCardDbContext anyCardDbContext)
    {
        _anyCardDbContext = anyCardDbContext;
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        return await _anyCardDbContext.Users.Where(u => u.Username == username).FirstOrDefaultAsync();
    }

    public async Task AddAsync(User user)
    {
        await _anyCardDbContext.Users.AddAsync(user);
    }

    public async Task<bool> SaveChangesAsync()
    {
        return await _anyCardDbContext.SaveChangesAsync() > 0;
    }
}
