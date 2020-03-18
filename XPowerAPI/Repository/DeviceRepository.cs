﻿using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using XPowerAPI.Logging;
using XPowerAPI.Models;
using XPowerAPI.Models.Params;
using XPowerAPI.Repository.Collections;

namespace XPowerAPI.Repository
{
    public class DeviceRepository : IRepository<Device, DeviceParams>, IDisposable
    {
        private bool isDisposed = false;

        private MySqlConnection con;
        private ILogger logger;

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

        public void Delete(DeviceParams entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(params DeviceParams[] entities)
        {
            throw new NotImplementedException();
        }

        public void Delete(IEnumerable<DeviceParams> entities)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(object id)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(DeviceParams entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(params DeviceParams[] entities)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(IEnumerable<DeviceParams> entities)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public bool Exists(object key)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ExistsAsync(object key)
        {
            throw new NotImplementedException();
        }

        public Device Find(params object[] keyValues)
        {
            throw new NotImplementedException();
        }

        public Task<Device> FindAsync(params object[] keyValues)
        {
            throw new NotImplementedException();
        }

        public Task<Device> FindAsync(object[] keyValues, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Device> FromSql(string sql, params object[] parameters)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Device> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Device>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public IPagedList<Device> GetPagedList(object[] keyValues, int pageNumber = 0, int pageSize = 20)
        {
            throw new NotImplementedException();
        }

        public Task<IPagedList<Device>> GetPagedListAsync(object[] keyValues, int pageNumber = 0, int pageSize = 20, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Device Insert(DeviceParams entity)
        {
            throw new NotImplementedException();
        }

        public void Insert(params DeviceParams[] entities)
        {
            throw new NotImplementedException();
        }

        public void Insert(IEnumerable<DeviceParams> entities)
        {
            throw new NotImplementedException();
        }

        public Task<Device> InsertAsync(DeviceParams entity)
        {
            throw new NotImplementedException();
        }

        public Task InsertAsync(params DeviceParams[] entities)
        {
            throw new NotImplementedException();
        }

        public Task InsertAsync(IEnumerable<DeviceParams> entities, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Device Update(DeviceParams entity)
        {
            throw new NotImplementedException();
        }

        public void Update(params DeviceParams[] entities)
        {
            throw new NotImplementedException();
        }

        public void Update(IEnumerable<DeviceParams> entities)
        {
            throw new NotImplementedException();
        }

        public Task<Device> UpdateAsync(DeviceParams entity)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(params DeviceParams[] entities)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(IEnumerable<DeviceParams> entities)
        {
            throw new NotImplementedException();
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
    }
}
