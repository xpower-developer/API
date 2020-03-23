using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using XPowerAPI.Services.Security;
using XPowerAPI.Logging;
using XPowerAPI.Models;

namespace XPowerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SessionController : ControllerBase
    {
        IAuthenticationService authenticationService;
        ILogger logger;

        public SessionController(
            [FromServices]IAuthenticationService authenticationService, 
            [FromServices]ILogger logger)
        {
            this.authenticationService = authenticationService;
            this.logger = logger;
        }

        /// <summary>
        /// Checks whether the input session key is not expired and thus can still be used
        /// </summary>
        /// <param name="key">the Authorization header key value</param>
        /// <returns>a 200OK code if the key is still valid, otherwise a 403Forbidden is returned</returns>
        [HttpGet]
        public async Task<IActionResult> VerifySession(
            [FromHeader(Name = "Authorization")]string key)
        {
            if (string.IsNullOrEmpty(key))
                return BadRequest("a session key is required");

            string sessionkey = key[(key.IndexOf(' ', StringComparison.InvariantCultureIgnoreCase) + 1)..];

            if (string.IsNullOrEmpty(sessionkey) || sessionkey.Length != 36)
                return BadRequest("invalid session key");

            try
            {
                SessionKey dbkey = await authenticationService.IsSignedInAsync(sessionkey).ConfigureAwait(false);
                if (dbkey == null)
                    return Forbid("The given key is no longer valid");
                else
                    return Ok("the given key is still valid for another " + (dbkey.ExpirationDate - DateTime.Now).ToString());
            }
            catch (Exception e)
            {
                await logger.LogAsync("An error occuredd whilst trying to check a session key, error: " + e.Message).ConfigureAwait(false);
                throw;
            }
        }

        /// <summary>
        /// Checks whether the input session key is not expired and thus can still be used, and if so, refreshes the expiration date of the key
        /// </summary>
        /// <param name="key">the Authorization header key value</param>
        /// <returns>a 200OK code if the key is still valid, otherwise a 403Forbidden is returned</returns>
        [HttpGet("refresh")]
        public async Task<IActionResult> RefreshSession([FromHeader(Name = "Authorization")]string key) {
            if (string.IsNullOrEmpty(key))
                return BadRequest("a session key is required");

            string sessionkey = key[key.IndexOf(' ', StringComparison.InvariantCultureIgnoreCase)..];

            if (string.IsNullOrEmpty(sessionkey) || sessionkey.Length != 36)
                return BadRequest("invalid session key");

            try
            {
                SessionKey dbkey = await authenticationService.IsSignedInAsync(sessionkey, true).ConfigureAwait(false);
                if (dbkey == null)
                    return Forbid("The given key is no longer valid");
                else
                    return Ok("the given key is valid for the next " + (dbkey.ExpirationDate - DateTime.Now).ToString());
            }
            catch (Exception e)
            {
                await logger.LogAsync("An error occuredd whilst trying to check a session key, error: " + e.Message).ConfigureAwait(false);
                throw;
            }
        }
    }
}