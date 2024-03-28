using Data.IRepositories;
using Domain.Commons;
using System.Collections.Generic;
using System.Linq.Expressions;
using System;
using Data.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Data.Repositories;

public class Repository<TEntity> : IRepository<TEntity> where TEntity : Auditable
{
    private readonly AppDbContext _dbContext;
    private readonly DbSet<TEntity> _dbSet;
    
    public Repository(AppDbContext appDbContext)
    {
        _dbContext = appDbContext;
        _dbSet = _dbContext.Set<TEntity>();
    }

    public async Task AddAsync(TEntity entity)
        => await _dbSet.AddAsync(entity);

    public void Update(TEntity entity)
    {
        entity.UpdatetAt = DateTime.UtcNow;
        _dbContext.Entry(entity).State = EntityState.Modified;
    }

    public void Delete(TEntity entity)
        => entity.IsDeleted = true;

    public void Destroy(TEntity entity)
        => _dbContext.Entry(entity).State = EntityState.Deleted;

    public async Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> expression, string[]? includes = null)
    {
        IQueryable<TEntity> query = _dbSet;

        if (includes is not null)
            foreach (var include in includes)
                query = query.Include(include);

        return await query.FirstOrDefaultAsync(expression);
    }

    public async Task<TEntity?> GetAsync(long id, string[]? includes = null)
        => await this.GetAsync(e => e.Id.Equals(id), includes);

    public IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>>? expression = null, bool isNoTracked = true, string[]? includes = null)
    {
        IQueryable<TEntity> query = _dbSet;

        if (expression is not null)
            query = query.Where(expression);

        if (isNoTracked)
            query = query.AsNoTracking();

        if (includes is not null)
            query = includes.Aggregate(query, (current, include) => current.Include(include));

        return query;
    }

    public async Task SaveAsync()
        => await _dbContext.SaveChangesAsync();
}
