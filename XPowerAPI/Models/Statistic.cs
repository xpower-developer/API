using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XPowerAPI.Models
{
    public class Statistic
    {
        public string Kind { get; set; }
        public string Value { get; set; }
        public DateTime Time { get; set; }
    }
}
