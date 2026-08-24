
using AnyCard.Domain.Enums;

namespace AnyCard.Domain.Model;
public class CardProgress
{
    public int Id { get; set; }
    public int CardId {  get; set; }
    public Card Card { get; set; } = null!;
    public int UserId {  get; set; }
    public User User { get; set; } = null!;
    public DateTime NextShowtime { get; set; }
    public UserRating UserRating { get; set; }
    public int ViewCounter {  get; set; }
}
