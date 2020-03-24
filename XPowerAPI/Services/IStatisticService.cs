using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XPowerAPI.Models;
using XPowerAPI.Repository.Collections;

namespace XPowerAPI.Services
{
    public interface IStatisticService
    {
        Task<IPagedList<IStatistic>> GetStatisticsForDeviceAsync(long deviceId, string sessionKey);
        Task<IPagedList<IStatistic>> GetStatisticsForDeviceAsync(long deviceId, DateTime since, string sessionKey);
        Task<IDictionary<Device, IPagedList<IStatistic>>> GetStatisticsForGroupAsync(long groupId, string sessionKey);
    }
}
