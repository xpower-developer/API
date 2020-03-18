using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XPowerAPI.Models
{
    public class Device
    {
        public long DeviceId { get; private set; }
        public string UUID { get; private set; }
        public string Name { get; set; }
        public bool State { get; set; }
        //when statistics gets added
        //public IPagedList<Statistic> Statistics { get; set; }
        public DateTime Created { get; private set; }

        public Device(string uUID, string name)
        {
            UUID = uUID;
            Name = name;
        }

        public Device(long deviceId, string uUID, string name, bool state, DateTime created)
        {
            DeviceId = deviceId;
            UUID = uUID;
            Name = name;
            State = state;
            Created = created;
        }
    }
}
