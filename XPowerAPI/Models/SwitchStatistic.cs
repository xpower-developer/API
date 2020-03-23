using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XPowerAPI.Models
{
    public class SwitchStatistic : IStatistic
    {
        public SwitchStatistic(long statisticId, string value, DateTime created)
        {
            StatisticId = statisticId;
            StatisticType = StatisticType.SWITCH;
            Value = value;
            Created = created;
        }

        public long StatisticId { get; private set; }

        public StatisticType StatisticType { get; private set; }

        public string Value { get; private set; }

        public DateTime Created { get; private set; }
    }
}
