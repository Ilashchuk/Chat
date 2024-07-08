namespace DAL.Entities;

public class User : BaseEntity
{
    public string Email { get; set; } = string.Empty;

    public string Username { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public ICollection<UsersChats> UsersChats { get; set; } = new List<UsersChats>();

    public ICollection<Chat> Chats { get; set; } = new List<Chat>();

    public ICollection<Message> Messages { get; set; } = new List<Message>();
}
