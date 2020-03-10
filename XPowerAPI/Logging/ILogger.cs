using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XPowerAPI.Logging
{
    public interface ILogger
    {
        void Log(string msg);
        Task LogAsync(string msg);
    }
}
