using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XPowerAPI.Models
{
    public interface IStatistic
    {
        public long StatisticId { get; }
        public long DeviceId { get; set; }
        public string Value { get; set; }
        public DateTime Created { get; set; }
    }
}
