using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using XPowerAPI.Models;
using XPowerAPI.Models.Params;
using XPowerAPI.Repository;
using XPowerAPI.Services.Security.Account;
using XPowerAPI.Logging;
using XPowerAPI.Services.Account;
using XPowerAPI.Services.Security;
using Microsoft.AspNetCore.Cors;

namespace XPowerAPI.Controllers
{
    /// <summary>
    /// Controller responsible for administrating, fetching and inserting customer data
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        ICustomerService customerService;
        IPasswordService passwordService;
        IAuthenticationService authenticationService;
        ILogger logger;

        [HttpGet("confirmation")]
        public IActionResult YoureGoodFam()
        {
            return Ok("You're good fam");
        }

        /// <summary>
        /// Creates a new instance of the customer controller with the specified repository and password service
        /// </summary>
        /// <param name="customerRepo">the desired customer repository</param>
        /// <param name="passwordService">´the desired password service</param>
        public CustomerController(
            [FromServices]ICustomerService customerService,
            [FromServices]IPasswordService passwordService,
            [FromServices]IAuthenticationService authenticationService,
            [FromServices]ILogger logger)
        {
            this.customerService = customerService;
            this.passwordService = passwordService;
            this.authenticationService = authenticationService;
            this.logger = logger;
        }

        /// <summary>
        /// Creates a new customer in the database with the requested details, 
        /// as long as a customer with the specified email does not exist
        /// </summary>
        /// <param name="logger">the logging implementation to be used</param>
        /// <param name="request">the request data to be used when creating the customer</param>
        /// <returns>a http response code, as well as debug text</returns>
        [HttpPost("signup")]
        public async Task<IActionResult> CreateCustomer([FromBody]CustomerSignup request)
        {
            //check whether a request body is supplied
            if (request == null)
                return BadRequest("No request object was supplied");
            //check whether the email has a valid length and syntax
            if (request.Email.Length == 0)
                return BadRequest("Email was not supplied");
            try
            {
                if (!Regex.IsMatch(request.Email, "^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$"))
                    return BadRequest("Email was invalid");
            }
            catch (RegexMatchTimeoutException e)
            {
                await logger
                    .LogAsync($"a regex search at {DateTime.Now} timed out, possible dos attack attempt, err msg: {e.Message}")
                    .ConfigureAwait(false);
            }
            catch (Exception)
            {
                throw;
            }
            //check name values
            if (request.FirstName.Length == 0 ||
                request.LastName.Length == 0)
                return BadRequest("Missing name parameters");
            //check location values
            if (request.CityId == 0 ||
                request.Street == null ||
                request.Street.Length == 0 ||
                request.StreetNumber == null ||
                request.StreetNumber.Length == 0)
                return BadRequest("The address information is invalid");
            if (request.Password.Length < passwordService.MinLength || request.Password.Length > passwordService.MaxLength)
                return BadRequest($"The supplied password length was invalid({passwordService.MinLength}-{passwordService.MaxLength} long, one uppercase, lowercase, number and special character minimum)");

            //check if the customer is already created within the database
            if (await customerService.CustomerExists(request.Email).ConfigureAwait(false))
                return BadRequest("An account using that email already exists");

            Customer cust = await customerService.CreateCustomer(request).ConfigureAwait(false);

            //return 200OK if the insert was successful
            if (cust != null)
                return Ok("Signed up customer successfully!");
            else
                return BadRequest("Unable to sign up user");
        }

        /// <summary>
        /// Signs in a customer and grants them a session key
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("signin")]
        public async Task<IActionResult> SigninCustomer([FromBody]CustomerSignin request)
        {
            //check parameters
            if (request == null)
                return BadRequest("the supplied request object was null");
            if (request.email == null || request.email.Length == 0)
                return BadRequest("the supplied email was null or empty");
            if (request.password == null || request.password.Length == 0)
                return BadRequest("the supplied password was null or empty");

            try
            {
                SessionKey key = await authenticationService.AuthenticateUser(request).ConfigureAwait(false);

                if (key != null)
                    return Ok(key);
            }
            catch (ArgumentNullException ane) {
                return BadRequest(ane.ParamName + " was null");
            }
            catch (ArgumentException ae)
            {
                return BadRequest(ae.Message);
            }
            catch (Exception)
            {
                throw;
            }

            return BadRequest("an error occured whilst creating the session key");
        }
    }

    //signup request data model
    public class CustomerSignup
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int CityId { get; set; }
        public string Street { get; set; }
        public string StreetNumber { get; set; }

        public CustomerSignup(string fname, string lname, string email, string password, int city, string street, string streetNumber)
        {
            FirstName = fname;
            LastName = lname;
            Email = email;
            Password = password;
            CityId = city;
            Street = street;
            StreetNumber = streetNumber;
        }

        public CustomerSignup()
        {

        }
    }

    public class CustomerSignin
    {
        public string email { get; set; }
        public string password { get; set; }

        public CustomerSignin(string email, string password)
        {
            this.email = email;
            this.password = password;
        }

        public CustomerSignin()
        {

        }
    }
}