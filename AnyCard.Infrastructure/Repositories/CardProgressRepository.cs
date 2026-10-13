using AnyCard.Application.Common;
using AnyCard.Application.Interfaces;
using AnyCard.Domain.Enums;
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
    public async Task<List<CardProgress>> GetDueForReviewAsync(int userId, int? categoryId)
    {
        return await _anyCardDbContext.CardProgresses.Include(cp => cp.Card).ThenInclude(c => c.Category)
            .Where(cp => cp.UserId == userId && cp.NextShowtime <= DateTime.UtcNow
            && (categoryId == null || cp.Card.CategoryId == categoryId))
            .ToListAsync();
    }
    public async Task<int> GetCountForReviewAsync(int userId)
    {
        return await _anyCardDbContext.CardProgresses.CountAsync(cp => cp.UserId == userId && cp.NextShowtime <= DateTime.UtcNow);
    }
    public async Task<PagedResult<CardProgress>> GetPagedByUserAsync(int userId, int? categoryId, UserRating? userRating, int pageNumber, int pageSize)
    {
        if (pageNumber < 1)
        {
            pageNumber = 1;
        }

        var query = _anyCardDbContext.CardProgresses.AsNoTracking().Include(cp => cp.Card).ThenInclude(c => c.Category)
            .Where(cp => cp.UserId == userId
            && (categoryId == null || cp.Card.CategoryId == categoryId)
            && (userRating == null || cp.UserRating == userRating));

        var totalCount = await query.CountAsync();

        var items = await query.OrderBy(cp => cp.NextShowtime)
            .ThenBy(cp => cp.Id)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedResult<CardProgress>(items, totalCount, pageNumber, pageSize);
    }
}
