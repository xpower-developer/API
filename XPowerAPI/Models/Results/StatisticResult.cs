using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XPowerAPI.Repository.Collections;

namespace XPowerAPI.Models.Results
{
    public class StatisticResult
    {
        public float TotalWattage { get; set; }
        public int Switches { get; set; }
        public IPagedList<IStatistic> Statistics { get; set; }
    }
}
