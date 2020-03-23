using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XPowerAPI.Models;
using XPowerAPI.Models.Params;
using XPowerAPI.Repository;
using XPowerAPI.Repository.Collections;

namespace XPowerAPI.Services
{
    public class StatisticService : IStatisticService
    {
        IRepository<IStatistic, StatisticParams> statisticRepo;
        IRepository<Device, DeviceParams> deviceRepo;

        public StatisticService(
            [FromServices]IRepository<IStatistic, StatisticParams> statisticRepo,
            [FromServices]IRepository<Device, DeviceParams> deviceRepo)
        {
            this.statisticRepo = statisticRepo;
            this.deviceRepo = deviceRepo;
        }

        public async Task<IPagedList<IStatistic>> GetStatisticsForDeviceAsync(long deviceId, string sessionKey)
        {
            if (deviceId == 0)
                throw new ArgumentNullException(nameof(deviceId));
            if (string.IsNullOrEmpty(sessionKey) || sessionKey.Length != 36)
                throw new ArgumentNullException(nameof(sessionKey));

            try
            {
                return await statisticRepo.GetPagedListAsync(
                    new object[] { 
                        new StatisticParams(){ 
                            DeviceId = deviceId,
                            SessionKey = sessionKey
                        },
                        0,
                        1000
                    })
                    .ConfigureAwait(false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IDictionary<Device, IPagedList<IStatistic>>> GetStatisticsForGroupAsync(long groupId, string sessionKey)
        {
            if (groupId == 0)
                throw new ArgumentNullException(nameof(groupId));
            if (string.IsNullOrEmpty(sessionKey) || sessionKey.Length != 36)
                throw new ArgumentNullException(nameof(sessionKey));

            try
            {
                IEnumerable<Device> devices = await deviceRepo.FromSqlAsync(
                    "GetDevicesByGroupId",
                    new object[] { 
                        System.Data.CommandType.StoredProcedure,
                        new MySqlParameter() { 
                            ParameterName = "groupId",
                            MySqlDbType = MySqlDbType.Int64,
                            Direction = System.Data.ParameterDirection.Input,
                            Value = groupId
                        },
                        new MySqlParameter(){ 
                            ParameterName = "sessionKey",
                            Direction = System.Data.ParameterDirection.Input,
                            MySqlDbType = MySqlDbType.VarChar,
                            Value = sessionKey
                        }
                    })
                    .ConfigureAwait(false);

                IDictionary<Device, IPagedList<IStatistic>> deviceStats = new Dictionary<Device, IPagedList<IStatistic>>();

                for (int i = 0; i < devices.Count(); i++)
                {
                    deviceStats.Add(devices.ElementAt(i), await statisticRepo.GetPagedListAsync(
                    new object[] {
                        new StatisticParams(){
                            DeviceId = devices.ElementAt(i).DeviceId,
                            SessionKey = sessionKey
                        },
                        0,
                        50000
                    })
                    .ConfigureAwait(false));
                }

                return deviceStats;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
