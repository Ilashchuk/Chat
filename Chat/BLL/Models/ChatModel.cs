using DAL.Entities;

namespace BLL.Models
{
    public class ChatModel
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public int UserId { get; set; }

        public List<int> UserIds { get; set; } = new List<int>();

        public List<MessageModel> Messages { get; set; } = new List<MessageModel>();
    }
}
