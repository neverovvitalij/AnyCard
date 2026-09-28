
namespace AnyCard.Domain.Model;
public class Category
{
    public int Id { get; set; }
    public string Name {  get; set; } = string.Empty;
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    public ICollection<Card> Cards { get; set; } = new List<Card>();
}
