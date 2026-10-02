

using AnyCard.Domain.Model;

namespace AnyCard.Application.Interfaces;
public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email);
    Task AddAsync(User user);
    Task<bool> SaveChangesAsync();
}
