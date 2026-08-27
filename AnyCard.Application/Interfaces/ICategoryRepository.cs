

using AnyCard.Domain.Model;

namespace AnyCard.Application.Interfaces;
public interface ICategoryRepository
{
    Task<List<Category>> GetAllAsync();
    Task AddAsync(Category category);
    Task<Category?> GetByIdAsync(int id);
    void Delete(Category category);
    Task<bool> SaveChangesAsync();
}
