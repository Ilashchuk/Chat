﻿using DAL.Data;
using DAL.Entities;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories;

public class Repository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
{
    private readonly SimpleChatContext _context;
    private readonly DbSet<TEntity> _dbSet;

    public Repository(SimpleChatContext dbContext)
    {
        _context = dbContext;
        _dbSet = dbContext.Set<TEntity>();
    }

    public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        return await _dbSet.AsNoTracking().ToListAsync();
    }

    public virtual async Task<TEntity?> GetByIdAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    public virtual async Task AddAsync(TEntity entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public virtual void Update(TEntity entity)
    {
        _dbSet.Attach(entity);
        _context.Entry(entity).State = EntityState.Modified;
    }

    public virtual async Task DeleteByIdAsync(int id)
    {
        var entityToRemove = await _dbSet.FindAsync(id);
        if (entityToRemove != null)
        {
            _dbSet.Remove(entityToRemove);
        }
    }
}
