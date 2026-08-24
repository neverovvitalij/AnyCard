using Microsoft.EntityFrameworkCore;
using AnyCard.Domain.Model;

namespace AnyCard.Infrastructure.Data;
public class AnyCardDbContext : DbContext
{
    public DbSet<Card> Cards { get; set; }
}
