using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XPowerAPI.Services;

namespace XPowerAPI.Controllers
{
    [Route("api/customer")]
    [ApiController]
    public class StatisticController : ControllerBase
    {
        IStatisticService statisticService;

        public StatisticController(
            [FromServices]IStatisticService statisticService)
        {
            this.statisticService = statisticService;
        }

        [HttpGet("devices/{deviceId}/statistics")]
        public async Task<IActionResult> GetDeviceStatistics(
            [FromRoute]long deviceId,
            [FromHeader(Name = "Authorization")]string sessionKey) {
            if (deviceId == 0)
                return BadRequest("Cannot fetch statistics for a device with an invalid id");
            if (string.IsNullOrEmpty(sessionKey))
                return BadRequest("Invalid or missing session key");

            sessionKey = sessionKey[(sessionKey.IndexOf(' ', StringComparison.InvariantCultureIgnoreCase) + 1)..];

            if (string.IsNullOrEmpty(sessionKey) || sessionKey.Length != 36)
                return BadRequest("Invalid session key");

            return Ok(await statisticService.GetStatisticsForDeviceAsync(deviceId, sessionKey).ConfigureAwait(false));
        }
    }
}
