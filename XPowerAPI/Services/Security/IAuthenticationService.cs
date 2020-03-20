using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XPowerAPI.Controllers;
using XPowerAPI.Models;

namespace XPowerAPI.Services.Security
{
    public interface IAuthenticationService : IDisposable
    {
        /// <summary>
        /// Authenticates a user and grants a new session key, should their login be correct
        /// </summary>
        /// <param name="request">the request parameters</param>
        /// <returns>the fresh session key</returns>
        Task<SessionKey> AuthenticateUser(CustomerSignin request);

        Task<SessionKey> IsSignedInAsync(string key, bool refresh = false);
    }
}
