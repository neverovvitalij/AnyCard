namespace AnyCard.Domain.Model;

public class UserSettings
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    public TimeSpan AgainInterval { get; set; }
    public TimeSpan HardInterval { get; set; }
    public TimeSpan GoodInterval { get; set; }
    public TimeSpan EasyInterval { get; set; }
}
