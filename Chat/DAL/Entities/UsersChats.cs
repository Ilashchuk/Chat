namespace DAL.Entities;

public class UsersChats : BaseEntity
{
    public int UserId { get; set; }

    public int ChatId { get; set; }

    public User? User { get; set; }

    public Chat? Chat { get; set; }
}
