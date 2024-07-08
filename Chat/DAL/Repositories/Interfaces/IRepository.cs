using DAL.Entities;

namespace DAL.Repositories.Interfaces;

public interface IRepository<TEntity> where TEntity : BaseEntity
{
    Task<IEnumerable<TEntity>> GetAllAsync();

    Task<TEntity?> GetByIdAsync(int id);

    Task AddAsync(TEntity entity);

    Task DeleteByIdAsync(int id);

    void Update(TEntity entity);
}
