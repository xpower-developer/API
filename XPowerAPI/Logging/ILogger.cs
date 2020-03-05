using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XPowerAPI.Logging
{
    interface ILogger
    {
        Task Log(string msg);
    }
}
