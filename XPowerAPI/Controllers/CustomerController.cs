using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace XPowerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetById() {
            if (!HttpContext.Request.Headers.ContainsKey("Authorization"))
                return Unauthorized("Bearer token wasn't included in the request");

            string key = HttpContext.Request.Headers["Authorization"][0][7..];

            if (string.IsNullOrEmpty(key))
                return Forbid("Bearer token was null");

            //call repo etc.
        }

    }
}