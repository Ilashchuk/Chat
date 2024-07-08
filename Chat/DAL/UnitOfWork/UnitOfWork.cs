using DAL.Repositories.Interfaces;
using DAL.Repositories;
using DAL.Data;

namespace DAL.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly SimpleChatContext _context;
    private bool disposed = false;

    public UnitOfWork(SimpleChatContext context)
    {
        _context = context;

        Chats = new ChatRepository(_context);

        Messages = new MessageRepository(_context);

        Users = new UserRepository(_context);
    }

    public IChatRepository Chats { get; private set; }

    public IMessageRepository Messages { get; private set; }

    public IUserRepository Users { get; private set; }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }

    public int Complete()
    {
        return _context.SaveChanges();
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposed)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            disposed = true;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
