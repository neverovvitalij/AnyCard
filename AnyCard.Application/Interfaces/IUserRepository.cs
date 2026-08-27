

using AnyCard.Domain.Model;

namespace AnyCard.Application.Interfaces;
public interface IUserRepository
{
    Task<User?> GetByUsernameAsync(string username);
    Task AddAsync(User user);
    Task<bool> SaveChangesAsync();
}
