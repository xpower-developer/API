using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using XPowerAPI.Models;
using XPowerAPI.Models.Params;
using XPowerAPI.Repository.Collections;
using XPowerAPI.Logging;
using Microsoft.AspNetCore.Mvc;

namespace XPowerAPI.Repository
{
    public class StatisticRepository : IRepository<IStatistic, StatisticParams>, IDisposable
    {
        private bool isDisposed = false;

        private MySqlConnection con;
        private ILogger logger;

        public StatisticRepository(
            [FromServices]MySqlConnection con,
            [FromServices]ILogger logger)
        {
            this.con = con;
            this.logger = logger;
        }

        ~StatisticRepository()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (isDisposed)
                return;

            if (disposing)
            {
                con.Dispose();
                logger.Dispose();
            }
        }
        public int Count()
        {
            throw new NotImplementedException();
        }

        public Task<int> CountAsync()
        {
            throw new NotImplementedException();
        }

        public void Delete(object id)
        {
            throw new NotImplementedException();
        }

        public void Delete(StatisticParams entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(params StatisticParams[] entities)
        {
            throw new NotImplementedException();
        }

        public void Delete(IEnumerable<StatisticParams> entities)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(object id)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(StatisticParams entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(params StatisticParams[] entities)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(IEnumerable<StatisticParams> entities)
        {
            throw new NotImplementedException();
        }

        public bool Exists(object key)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ExistsAsync(object key)
        {
            throw new NotImplementedException();
        }

        public IStatistic Find(params object[] keyValues)
        {
            throw new NotImplementedException();
        }

        public Task<IStatistic> FindAsync(params object[] keyValues)
        {
            throw new NotImplementedException();
        }

        public Task<IStatistic> FindAsync(object[] keyValues, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IStatistic> FromSql(string sql, params object[] parameters)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<IStatistic>> FromSqlAsync(string sql, params object[] parameters)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IStatistic> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<IStatistic>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public IPagedList<IStatistic> GetPagedList(object[] keyValues, int pageNumber = 0, int pageSize = 20)
        {
            throw new NotImplementedException();
        }

        public Task<IPagedList<IStatistic>> GetPagedListAsync(object[] keyValues, int pageNumber = 0, int pageSize = 20, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public IStatistic Insert(StatisticParams entity)
        {
            throw new NotImplementedException();
        }

        public void Insert(params StatisticParams[] entities)
        {
            throw new NotImplementedException();
        }

        public void Insert(IEnumerable<StatisticParams> entities)
        {
            throw new NotImplementedException();
        }

        public Task<IStatistic> InsertAsync(StatisticParams entity)
        {
            throw new NotImplementedException();
        }

        public Task InsertAsync(params StatisticParams[] entities)
        {
            throw new NotImplementedException();
        }

        public Task InsertAsync(IEnumerable<StatisticParams> entities, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public IStatistic Update(StatisticParams entity)
        {
            throw new NotImplementedException();
        }

        public void Update(params StatisticParams[] entities)
        {
            throw new NotImplementedException();
        }

        public void Update(IEnumerable<StatisticParams> entities)
        {
            throw new NotImplementedException();
        }

        public Task<IStatistic> UpdateAsync(StatisticParams entity)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(params StatisticParams[] entities)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(IEnumerable<StatisticParams> entities)
        {
            throw new NotImplementedException();
        }
    }
}
