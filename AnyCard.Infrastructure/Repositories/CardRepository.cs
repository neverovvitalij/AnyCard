
using AnyCard.Application.Interfaces;
using AnyCard.Domain.Model;
using AnyCard.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AnyCard.Infrastructure.Repositories;
public class CardRepository : ICardRepository
{
    private readonly AnyCardDbContext _anyCardDbContext;
    public CardRepository(AnyCardDbContext anyCardDbContext)
    {
        _anyCardDbContext = anyCardDbContext;
    }

    public async Task AddAsync(Card card)
    {
        await _anyCardDbContext.Cards.AddAsync(card);
    }
    public async Task<List<Card>> GetAllAsync(int userId)
    {
        return await _anyCardDbContext.Cards.Include(c => c.Category).Where(c => c.UserId == userId).ToListAsync();
    }
    public async Task<Card?> GetByIdAsync(int id, int userId)
    {
        return await _anyCardDbContext.Cards
            .Include(c => c.Category)
            .Where(c => c.Id == id && c.UserId == userId).FirstOrDefaultAsync();
    }
    public async Task<bool> SaveChangesAsync()
    {
        return await _anyCardDbContext.SaveChangesAsync() > 0;
    }
    public void Delete(Card card)
    {
        _anyCardDbContext.Cards.Remove(card);
    }
}
