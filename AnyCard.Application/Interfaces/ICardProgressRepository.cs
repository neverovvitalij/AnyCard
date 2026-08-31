
using AnyCard.Domain.Model;

namespace AnyCard.Application.Interfaces;
public interface ICardProgressRepository
{
    Task AddAsync(CardProgress cardProgress);
    Task<bool> SaveChangesAsync();
    Task<CardProgress?> GetByUserAndCardAsync(int userId, int cardId);
    Task<List<CardProgress>> GetDueForReviewAsync(int userId);
}
