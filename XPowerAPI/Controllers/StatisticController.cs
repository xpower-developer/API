﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

            return Ok(await statisticService.GetStatisticsForDeviceAsync(deviceId, authorization).ConfigureAwait(false));
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
