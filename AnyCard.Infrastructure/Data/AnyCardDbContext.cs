using Microsoft.EntityFrameworkCore;
using AnyCard.Domain.Model;

namespace AnyCard.Infrastructure.Data;
public class AnyCardDbContext : DbContext
{
    public DbSet<Card> Cards { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<CardProgress> CardProgresses { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public AnyCardDbContext(DbContextOptions<AnyCardDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CardProgress>()
            .HasIndex(cp => new { cp.CardId, cp.UserId })
            .IsUnique();
    }
}
