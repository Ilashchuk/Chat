using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.Data;

public class SimpleChatContext : DbContext
{
    public SimpleChatContext(DbContextOptions<SimpleChatContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; } = null!;

    public DbSet<Chat> Chats { get; set; } = null!;

    public DbSet<Message> Messages { get; set; } = null!;

    public DbSet<UsersChats> UsersChats { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // When deleting a user
        modelBuilder.Entity<Message>()
            .HasOne(m => m.User)
            .WithMany(u => u.Messages)
            .HasForeignKey(m => m.UserId)
            .OnDelete(DeleteBehavior.NoAction); // do not delete the messages

        modelBuilder.Entity<Chat>()
            .HasOne(c => c.User)
            .WithMany(u => u.Chats)
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Cascade); // delete all chats of this user

        modelBuilder.Entity<UsersChats>()
            .HasOne(uc => uc.User)
            .WithMany(u => u.UsersChats)
            .HasForeignKey(uc => uc.UserId)
            .OnDelete(DeleteBehavior.NoAction); // do not delete UsersChats of this user

        // When deleting a chat
        modelBuilder.Entity<Message>()
            .HasOne(m => m.Chat)
            .WithMany(c => c.Messages)
            .HasForeignKey(m => m.ChatId)
            .OnDelete(DeleteBehavior.Cascade); // delete messages of this chat

        modelBuilder.Entity<UsersChats>()
            .HasOne(uc => uc.Chat)
            .WithMany(c => c.UsersChats)
            .HasForeignKey(uc => uc.ChatId)
            .OnDelete(DeleteBehavior.Cascade); // delete UsersChats of this chat
    }
}
