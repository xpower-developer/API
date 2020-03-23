using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XPowerAPI.Models;

namespace XPowerAPI.Services
{
    public interface IStatisticService
    {
        Task<IEnumerable<IStatistic>> GetStatisticsForDeviceAsync(long deviceId, string sessionKey);
        Task<IDictionary<Device, IEnumerable<IStatistic>>> GetStatisticsForGroupAsync(long groupId, string sessionKey);
    }
}
