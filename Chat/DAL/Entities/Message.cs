namespace DAL.Entities;

public class Message : BaseEntity
{
    public string Text { get; set; } = string.Empty;

    public DateTime CreationDate { get; set; }

    public int UserId { get; set; }

    public User? User { get; set; }

    public int ChatId { get; set; }

    public Chat? Chat { get; set; }
}
