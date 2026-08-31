
using AnyCard.Domain.Model;

namespace AnyCard.Application.Interfaces;
public interface ICardRepository
{
    Task AddAsync(Card card);
    Task<List<Card>> GetAllAsync();
    Task<Card?> GetByIdAsync(int id);
    Task<bool> SaveChangesAsync();
    void Delete(Card card);

}
