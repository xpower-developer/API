using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XPowerAPI.Models.Params
{
    public class SessionKeyParams
    {
        public SessionKeyParams(string email)
        {
            Email = email;
        }

        public SessionKeyParams(string email, SessionKey sessionKey)
        {
            Email = email;
            SessionKey = sessionKey;
        }

        public string Email { get; set; }
        public SessionKey SessionKey { get; set; }
    }
}
