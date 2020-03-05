using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using XPowerAPI.Models;
using XPowerAPI.Repository;

namespace XPowerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeviceController : ControllerBase
    {
        IRepository<Device> deviceRepo = new DeviceRepository();

        [HttpGet]
        public IEnumerable<Device> GetDevices() {
            throw new NotImplementedException();
        }

        [HttpGet]
        [Route("{id:string}")]
        public Device GetDevice() {
            throw new NotImplementedException();
        }
    }
}