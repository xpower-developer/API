using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using XPowerAPI.Models;
using XPowerAPI.Models.Params;
using XPowerAPI.Repository;
using XPowerAPI.Services.Security.Account;

namespace XPowerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        IRepository<Customer, CustomerParams> customerRepo;
        IPasswordService passwordService;

        public CustomerController(IRepository<Customer, CustomerParams> customerRepo, IPasswordService passwordService)
        {
            this.customerRepo = customerRepo;
            this.passwordService = passwordService;
        }

        [HttpPost]
        [Route("/signup")]
        public async Task<IActionResult> CreateCustomer([FromBody]CustomerSignup request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));
            if (request.Email.Length == 0)
                return BadRequest("Email was not supplied");
            if (!Regex.IsMatch(request.Email, "^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$"))
                return BadRequest("Email was invalid");
            if (request.CityId == 0 ||
                request.StreetId == 0 ||
                request.StreetNumber.Length == 0)
                return BadRequest("The address information is invalid");

            if (await customerRepo.ExistsAsync(request.Email).ConfigureAwait(true))
                return BadRequest("An account using that email already exists");

            byte[] password, salt;
            password = passwordService.CreatePassword(request.Password, out salt);

            Customer cust =
                await customerRepo
                    .InsertAsync(
                        new CustomerParams()
                        {
                            Customer = new Customer(
                                request.Email,
                                request.CityId,
                                request.StreetId,
                                request.StreetNumber,
                                password,
                                salt
                                )
                        })
                    .ConfigureAwait(true);

            if (cust != null)
                return Ok("Signed up customer successfully!");
            else
                return BadRequest("Unable to sign up user");
        }

    }
    public class CustomerSignup
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public int CityId { get; set; }
        public long StreetId { get; set; }
        public string StreetNumber { get; set; }

        public CustomerSignup(string email, string password, int city, long street, string streetNumber)
        {
            Email = email;
            Password = password;
            CityId = city;
            StreetId = street;
            StreetNumber = streetNumber;
        }
    }
}