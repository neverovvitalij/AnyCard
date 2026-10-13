
using AnyCard.Application.Common;
using AnyCard.Domain.Enums;
using AnyCard.Domain.Model;

namespace AnyCard.Application.Interfaces;

public interface ICardProgressRepository
{
    Task AddAsync(CardProgress cardProgress);
    Task<bool> SaveChangesAsync();
    Task<CardProgress?> GetByUserAndCardAsync(int userId, int cardId);
    Task<List<CardProgress>> GetDueForReviewAsync(int userId, int? categoryId);
    Task<PagedResult<CardProgress>> GetPagedByUserAsync(int userId, int? categoryId, UserRating? userRating, int pageNumber, int pageSize);
    Task<int> GetCountForReviewAsync(int userId);
}