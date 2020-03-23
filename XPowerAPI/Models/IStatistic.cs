using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XPowerAPI.Models
{
    public interface IStatistic
    {
        public long StatisticId { get; }
        public StatisticType StatisticType { get; }
        public string Value { get; }
        public DateTime Created { get; }
    }

    public enum StatisticType { 
        WATTAGE = 1,
        SWITCH = 2
    }
}
