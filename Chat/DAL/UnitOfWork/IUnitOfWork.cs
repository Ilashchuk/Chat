using DAL.Repositories.Interfaces;

namespace DAL.UnitOfWork;

public interface IUnitOfWork : IDisposable
{
    IChatRepository Chats { get; }

    IMessageRepository Messages { get; }

    IUserRepository Users { get; }

    Task SaveAsync();

    int Complete();
}
