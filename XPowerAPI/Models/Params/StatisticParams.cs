﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XPowerAPI.Models.Params
{
    public class StatisticParams
    {
        public long DeviceId { get; set; }
        public int CustomerId { get; set; }
        public string SessionKey { get; set; }
        public IStatistic Statistic { get; set; }
    }
}