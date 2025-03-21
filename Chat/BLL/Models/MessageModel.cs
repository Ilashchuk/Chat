﻿using DAL.Entities;

namespace BLL.Models;

public class MessageModel
{
    public int Id { get; set; }

    public string? Text { get; set; }

    public DateTime CreationDate { get; set; }

    public int UserId { get; set; }

    public int ChatId { get; set; }
}
