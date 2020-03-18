using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XPowerAPI.Models.Params
{
    public class DeviceParams
    {
        public string ApiKey { get; private set; }
        public Device Device { get; private set; }
    }
}
