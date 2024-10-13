using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Infrastructure.DataAccess.Repositories;

public abstract class Repository<T> : IRepository<T> where T : BaseEntity
{
    public DbSet<T> Entity { get; private set; }

    private MottuDbContext _context;

    public Repository(
        MottuDbContext context)
    {
        Entity = context.Set<T>();
        _context = context;
    }

    public async Task Add(
        T entity) => await Entity.AddAsync(entity);

    public async Task Add(
        IEnumerable<T> items) => await Entity.AddRangeAsync(items);

    public void Update(
        T entity)
    {
        var existingEntity = _context.Set<T>().Find(entity.Id);

        if (existingEntity != null)
            _context.Entry(existingEntity).CurrentValues.SetValues(entity);
        else
        {
            _context.Set<T>().Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }
    }

    public void Update(
        IEnumerable<T> items) => Entity.UpdateRange(items);

    public void Delete(
        T entity) => Entity.Remove(entity);

    public async Task ExecuteSqlRawAsync(
        string sql) => await _context.Database.ExecuteSqlRawAsync(sql);

    public async Task<bool> Any(
        Expression<Func<T, bool>> predicate)
        => await Entity.AnyAsync(predicate);

    public IQueryable<T> Where(
        Expression<Func<T, bool>> expression) => Entity.Where(expression).AsNoTracking();

    public async virtual Task<List<T>> GetAll()
        => await Entity?.ToListAsync();

    public async virtual Task<List<T>> GetAllWithIncludes(
        params Expression<Func<T, object>>[] includes)
    {
        IQueryable<T> query = Entity;

        foreach (var include in includes)
            query = query.Include(include);

        return await query?.ToListAsync();
    }

    public IQueryable<T> FromSqlRaw(
        string sql) => Entity.FromSqlRaw(sql);

    public List<T> RawSqlQuery(
        string query,
        Func<DbDataReader, T> map)
    {
        using (var command = _context.Database.GetDbConnection().CreateCommand())
        {
            command.CommandText = query;
            command.CommandType = CommandType.Text;

            _context.Database.OpenConnection();

            using (var result = command.ExecuteReader())
            {
                var entities = new List<T>();

                while (result.Read())
                    entities.Add(map(result));

                return entities;
            }
        }
    }

    public long Count()
        => Entity.Count();
}