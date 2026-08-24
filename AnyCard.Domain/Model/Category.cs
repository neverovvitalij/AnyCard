
namespace AnyCard.Domain.Model;
public class Category
{
    public int Id { get; set; }
    public string Name {  get; set; } = string.Empty;
    public ICollection<Card> Cards { get; set; } = new List<Card>();
}
