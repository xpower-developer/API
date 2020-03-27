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
using System.Data.Common;

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

        public async Task<IPagedList<IStatistic>> GetPagedListAsync(object[] keyValues, int pageNumber = 0, int pageSize = 20, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (keyValues == null || keyValues.Length == 0)
                throw new ArgumentNullException(nameof(keyValues));

            //temporarily call the same sp regardless of which is requested, for test reasons
            StatisticParams param = (StatisticParams)keyValues[0];
            MySqlCommand cmd =
                new MySqlCommand("GetDeviceStatisticsDailySummary", con) { CommandType = System.Data.CommandType.StoredProcedure };

            if (param.FromTime != DateTime.MinValue)
                cmd.CommandText = "GetDeviceStatisticsDailySummary";
            else
                switch (param.SummaryType)
                {
                    case SummaryType.DAILY:
                        cmd.CommandText = "GetDeviceStatisticsDailySummary";
                        break;
                    case SummaryType.NONE:
                        cmd.CommandText = "GetDeviceStatisticsDailySummary";//"GetStatisticsPaged";
                        break;
                    default: break;
                };

            cmd.Parameters.Add(new MySqlParameter()
            {
                Direction = System.Data.ParameterDirection.Input,
                ParameterName = "pageSize",
                Value = pageSize,
                MySqlDbType = MySqlDbType.Int32
            });
            cmd.Parameters.Add(new MySqlParameter()
            {
                Direction = System.Data.ParameterDirection.Input,
                ParameterName = "pageNumber",
                Value = pageNumber,
                MySqlDbType = MySqlDbType.Int32
            });
            cmd.Parameters.Add(new MySqlParameter()
            {
                Direction = System.Data.ParameterDirection.Input,
                ParameterName = "deviceId",
                Value = param.DeviceId,
                MySqlDbType = MySqlDbType.Int64
            });
            cmd.Parameters.Add(new MySqlParameter()
            {
                Direction = System.Data.ParameterDirection.Input,
                ParameterName = "sessionKey",
                Value = param.SessionKey,
                MySqlDbType = MySqlDbType.VarChar
            });

            if (param.FromTime != DateTime.MinValue)
            {
                cmd.Parameters.Add(new MySqlParameter()
                {
                    Direction = System.Data.ParameterDirection.Input,
                    ParameterName = "fromtime",
                    Value = param.FromTime,
                    MySqlDbType = MySqlDbType.DateTime
                });
            }


            IList<IStatistic> stats = new List<IStatistic>();

            try
            {
                await con.OpenAsync().ConfigureAwait(false);
                DbDataReader reader = await cmd.ExecuteReaderAsync().ConfigureAwait(false);

                while (reader.Read())
                {
                    switch (reader.GetInt16(reader.GetOrdinal("StatisticTypeId")))
                    {
                        case (short)StatisticType.WATTAGE:
                            stats.Add(
                                new WattageStatistic(
                                    reader.GetInt64(reader.GetOrdinal("StatisticId")),
                                    reader.GetString(reader.GetOrdinal("Value")),
                                    reader.GetDateTime(reader.GetOrdinal("Created"))));
                            break;
                        case (short)StatisticType.SWITCH:
                            stats.Add(
                                new SwitchStatistic(
                                    reader.GetInt64(reader.GetOrdinal("StatisticId")),
                                    reader.GetString(reader.GetOrdinal("Value")),
                                    reader.GetDateTime(reader.GetOrdinal("Created"))));
                            break;
                        default:
                            break;
                    }
                }

                 //fix if true pagination is needed
                return new PagedList<IStatistic>(stats, stats.Count, 1, 1, 0);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                await con.CloseAsync().ConfigureAwait(false);
                cmd.Dispose();
            }
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
