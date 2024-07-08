namespace DAL.Entities;

public class Chat : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public int UserId { get; set; }

    public User? User {  get; set; }

    public ICollection<UsersChats> UsersChats { get; set; } = new List<UsersChats>();

    public ICollection<Message> Messages { get; set; } = new List<Message>();
}
