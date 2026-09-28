

using AnyCard.Application.Interfaces;
using AnyCard.Domain.Model;
using AnyCard.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AnyCard.Infrastructure.Repositories;
public class CategoryRepository : ICategoryRepository
{
    private readonly AnyCardDbContext _anyCardDbContext;
    public CategoryRepository(AnyCardDbContext anyCardDbContext)
    {
        _anyCardDbContext = anyCardDbContext;
    }

    public async Task<List<Category>> GetAllAsync(int userId)
    {
        return await _anyCardDbContext.Categories.Where(c => c.UserId == userId).ToListAsync();
    }
    public async Task AddAsync(Category category)
    {
        await _anyCardDbContext.Categories.AddAsync(category);
    }
    public async Task<Category?> GetByIdAsync(int id, int userId)
    {
        return await _anyCardDbContext.Categories.FirstOrDefaultAsync(c => c.Id == id && c.UserId == userId);
    }
    public void Delete(Category category)
    {
        _anyCardDbContext.Categories.Remove(category);
    }
    public async Task<bool> SaveChangesAsync()
    {
        return await _anyCardDbContext.SaveChangesAsync() > 0;
    }
}
