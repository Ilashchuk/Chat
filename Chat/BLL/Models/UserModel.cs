namespace BLL.Models;

public class UserModel
{
    public int Id { get; set; }

    public string? Email { get; set; }

    public string? Username { get; set; }

    public string? Password { get; set; }

    public List<int> ChatIds { get; set; } = new List<int>();

    public List<int> MessageIds { get; set; } = new List<int>();
}
