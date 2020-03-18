using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XPowerAPI.Models
{
    public class SessionKey
    {
        public string Key { get; private set; }
        public string ApiKey { get; set; }
        public DateTime ExpirationDate { get; set; }

        public bool Expired { get => DateTime.Now > ExpirationDate; }

        public SessionKey() { }

        public SessionKey(string key, DateTime expirationDate)
        {
            Key = key;
            ExpirationDate = expirationDate;
        }

        public SessionKey(string key, string apiKey, DateTime expirationDate)
        {
            Key = key;
            ApiKey = apiKey;
            ExpirationDate = expirationDate;
        }
    }
}
