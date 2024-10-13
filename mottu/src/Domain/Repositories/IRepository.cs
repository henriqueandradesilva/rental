using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Domain.Repositories;

public interface IRepository<T> where T : class
{
    Task Add(
        T entity);

    Task Add(
        IEnumerable<T> items);

    void Update(
        T entity);

    void Update(
        IEnumerable<T> items);

    void Delete(
        T entity);

    Task ExecuteSqlRawAsync(
        string sql);

    Task<bool> Any(
        Expression<Func<T, bool>> predicate);

    IQueryable<T> Where(
        Expression<Func<T, bool>> expression);

    Task<List<T>> GetAll();

    Task<List<T>> GetAllWithIncludes(
        params Expression<Func<T, object>>[] includes);

    IQueryable<T> FromSqlRaw(
        string sql);

    List<T> RawSqlQuery(
        string query,
        Func<DbDataReader, T> map);

    long Count();
}