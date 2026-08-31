

using AnyCard.Application.Interfaces;
using AnyCard.Domain.Model;
using AnyCard.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AnyCard.Infrastructure.Repositories;
public class CardProgressRepository : ICardProgressRepository
{
    private readonly AnyCardDbContext _anyCardDbContext;
    public CardProgressRepository(AnyCardDbContext anyCardDbContext)
    {
        _anyCardDbContext = anyCardDbContext;
    }

    public async Task AddAsync(CardProgress cardProgress)
    {
        await _anyCardDbContext.CardProgresses.AddAsync(cardProgress);
    }
    public async Task<bool> SaveChangesAsync()
    {
        return await _anyCardDbContext.SaveChangesAsync() > 0;
    }
    public async Task<CardProgress?> GetByUserAndCardAsync(int userId, int cardId)
    {
        return await _anyCardDbContext.CardProgresses.Where(cp => cp.UserId == userId && cp.CardId == cardId).FirstOrDefaultAsync();
    }
    public async Task<List<CardProgress>> GetDueForReviewAsync(int userId)
    {
        return await _anyCardDbContext.CardProgresses.Include(cp => cp.Card)
            .Where(cp => cp.UserId == userId && cp.NextShowtime <= DateTime.UtcNow).ToListAsync();
    }
}
