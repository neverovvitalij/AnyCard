

namespace AnyCard.Domain.Model;
public class Card
{
    public int Id { get; set; }
    public string Question { get; set; } = string.Empty;
    public string Answer {  get; set; } = string.Empty;
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;
    public ICollection<CardProgress> CardProgress { get; set; } = new List<CardProgress>();

}
