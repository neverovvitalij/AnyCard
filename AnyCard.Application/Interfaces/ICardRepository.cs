
using AnyCard.Domain.Model;

namespace AnyCard.Application.Interfaces;
public interface ICardRepository
{
    Task AddAsync(Card card);
    Task<List<Card>> GetAllAsync(int userId);
    Task<Card?> GetByIdAsync(int id, int userId);
    Task<bool> SaveChangesAsync();
    void Delete(Card card);

}
