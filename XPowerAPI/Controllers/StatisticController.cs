using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using XPowerAPI.Models;
using XPowerAPI.Models.Results;
using XPowerAPI.Repository.Collections;
using XPowerAPI.Services;

namespace XPowerAPI.Controllers
{
    [Route("api")]
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
            [FromHeader(Name = "Authorization")]string authorization)
        {
            if (deviceId == 0)
                return BadRequest("Cannot fetch statistics for a device with an invalid id");
            if (string.IsNullOrEmpty(authorization))
                return BadRequest("Invalid or missing session key");

            authorization = authorization[(authorization.IndexOf(' ', StringComparison.InvariantCultureIgnoreCase) + 1)..];

            if (string.IsNullOrEmpty(authorization) || authorization.Length != 36)
                return BadRequest("Invalid session key");

            StatisticResult res = new StatisticResult() { 
                Statistics = await statisticService.GetStatisticsForDeviceAsync(deviceId, authorization).ConfigureAwait(false)
            };

            res.Switches = res.Statistics.Items.Where(x => x.StatisticType == StatisticType.SWITCH).Count();
            res.TotalWattage = res.Statistics.Items.Where(x => x.StatisticType == StatisticType.WATTAGE).Sum((x) => long.Parse(x.Value, CultureInfo.InvariantCulture));

            return Ok(res);
        }

        [HttpGet("devices/{deviceId}/statistics/summarized")]
        public async Task<IActionResult> GetDeviceStatisticsDaily(
            [FromRoute]long deviceId,
            [FromHeader(Name = "Authorization")]string authorization)
        {
            if (deviceId == 0)
                return BadRequest("Cannot fetch statistics for a device with an invalid id");
            if (string.IsNullOrEmpty(authorization))
                return BadRequest("Invalid or missing session key");

            authorization = authorization[(authorization.IndexOf(' ', StringComparison.InvariantCultureIgnoreCase) + 1)..];

            if (string.IsNullOrEmpty(authorization) || authorization.Length != 36)
                return BadRequest("Invalid session key");

            StatisticResult res = new StatisticResult()
            {
                Statistics = await statisticService.GetStatisticsForDeviceSummarizedAsync(deviceId, authorization).ConfigureAwait(false)
            };

            res.TotalWattage = res.Statistics.Items.Sum(x => float.Parse(x.Value, CultureInfo.InvariantCulture));

            return Ok(res);
        }

        [HttpGet("devices/{deviceId}/statistics/{since}")]
        public async Task<IActionResult> GetDeviceStatisticsWithinTimeFrame(
            [FromRoute]long deviceId,
            [FromRoute]DateTime since,
            [FromHeader(Name = "Authorization")]string authorization)
        {
            if (deviceId == 0)
                return BadRequest("Cannot fetch statistics for a device with an invalid id");
            if (string.IsNullOrEmpty(authorization))
                return BadRequest("Invalid or missing session key");

            authorization = authorization[(authorization.IndexOf(' ', StringComparison.InvariantCultureIgnoreCase) + 1)..];

            if (string.IsNullOrEmpty(authorization) || authorization.Length != 36)
                return BadRequest("Invalid session key");

            StatisticResult res = new StatisticResult()
            {
                Statistics = await statisticService.GetStatisticsForDeviceAsync(deviceId, authorization).ConfigureAwait(false)
            };

            res.Switches = res.Statistics.Items.Where(x => x.StatisticType == StatisticType.SWITCH).Count();
            res.TotalWattage = res.Statistics.Items.Where(x => x.StatisticType == StatisticType.WATTAGE).Sum((x) => long.Parse(x.Value, CultureInfo.InvariantCulture));

            return Ok(res);
        }

        [HttpGet("groups/{groupId}/statistics")]
        public async Task<IActionResult> GetGroupStatistics(
            [FromRoute]long groupId,
            [FromHeader(Name = "Authorization")]string authorization)
        {
            if (groupId == 0)
                return BadRequest("Cannot fetch statistics for a device with an invalid id");
            if (string.IsNullOrEmpty(authorization))
                return BadRequest("Invalid or missing session key");

            authorization = authorization[(authorization.IndexOf(' ', StringComparison.InvariantCultureIgnoreCase) + 1)..];

            if (string.IsNullOrEmpty(authorization) || authorization.Length != 36)
                return BadRequest("Invalid session key");

            return Ok(await statisticService.GetStatisticsForDeviceAsync(groupId, authorization).ConfigureAwait(false));
        }
    }
}
