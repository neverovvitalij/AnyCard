

using AnyCard.Domain.Model;

namespace AnyCard.Application.Interfaces;
public interface ICategoryRepository
{
    Task<List<Category>> GetAllAsync(int userId);
    Task AddAsync(Category category);
    Task<Category?> GetByIdAsync(int id, int userId);
    void Delete(Category category);
    Task<bool> SaveChangesAsync();
}
