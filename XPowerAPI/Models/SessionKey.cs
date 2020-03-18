using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XPowerAPI.Models
{
    public class SessionKey
    {
        public SessionKey(string key, DateTime expirationDate)
        {
            Key = key;
            ExpirationDate = expirationDate;
        }

        public string Key { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}
