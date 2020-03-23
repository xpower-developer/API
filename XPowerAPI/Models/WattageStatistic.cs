using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XPowerAPI.Models
{
    public class WattageStatistic : IStatistic
    {
        public WattageStatistic(long statisticId, string value, DateTime created)
        {
            StatisticId = statisticId;
            StatisticType = StatisticType.WATTAGE;
            Value = value;
            Created = created;
        }

        public long StatisticId { get; private set; }

        public StatisticType StatisticType { get; private set; }

        public string Value { get; private set; }

        public DateTime Created { get; private set; }
    }
}
