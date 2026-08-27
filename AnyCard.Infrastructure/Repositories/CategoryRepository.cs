

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

    public async Task<List<Category>> GetAllAsync()
    {
        return await _anyCardDbContext.Categories.ToListAsync();
    }
    public async Task AddAsync(Category category)
    {
        await _anyCardDbContext.Categories.AddAsync(category);
    }
    public async Task<Category?> GetByIdAsync(int id)
    {
        return await _anyCardDbContext.Categories.FindAsync(id);
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
