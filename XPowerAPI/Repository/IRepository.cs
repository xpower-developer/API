using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using XPowerAPI.Repository.Collections;

namespace XPowerAPI.Repository
{
    public interface IRepository<TEntity, TParam> 
        where TEntity : class
        where TParam : class
    {
        IEnumerable<TEntity> FromSql(string sql, params object[] parameters);

        TEntity Find(params object[] keyValues);

        Task<TEntity> FindAsync(params object[] keyValues);

        Task<TEntity> FindAsync(object[] keyValues, CancellationToken cancellationToken);

        IEnumerable<TEntity> GetAll();

        Task<IEnumerable<TEntity>> GetAllAsync();

        IPagedList<TEntity> GetPagedList(object[] keyValues,
                                         int pageNumber = 0,
                                         int pageSize = 20);

        Task<IPagedList<TEntity>> GetPagedListAsync(object[] keyValues,
                                                    int pageNumber = 0,
                                                    int pageSize = 20,
                                                    CancellationToken cancellationToken = default(CancellationToken));

        int Count();

        Task<int> CountAsync();

        bool Exists(object key);

        Task<bool> ExistsAsync(object key);

        TEntity Insert(TParam entity);

        void Insert(params TParam[] entities);

        void Insert(IEnumerable<TParam> entities);

        Task<TEntity> InsertAsync(TParam entity);

        Task InsertAsync(params TParam[] entities);

        Task InsertAsync(IEnumerable<TParam> entities, CancellationToken cancellationToken = default(CancellationToken));

        TEntity Update(TParam entity);

        void Update(params TParam[] entities);

        void Update(IEnumerable<TParam> entities);

        Task<TEntity> UpdateAsync(TParam entity);

        Task UpdateAsync(params TParam[] entities);

        Task UpdateAsync(IEnumerable<TParam> entities);

        void Delete(object id);

        void Delete(TParam entity);

        void Delete(params TParam[] entities);

        void Delete(IEnumerable<TParam> entities);

        Task DeleteAsync(object id);

        Task DeleteAsync(TParam entity);

        Task DeleteAsync(params TParam[] entities);

        Task DeleteAsync(IEnumerable<TParam> entities);
    }
}
