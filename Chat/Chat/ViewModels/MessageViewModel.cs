namespace Chat.ViewModels;

public class MessageViewModel
{
    public int Id { get; set; }

    public DateTime CreationDate { get; set; }

    public int ChatId { get; set; }

    public int UserId { get; set; }

    public string Text { get; set; } = string.Empty;
}
