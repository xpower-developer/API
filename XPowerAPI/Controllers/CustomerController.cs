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

namespace XPowerAPI.Controllers
{
    /// <summary>
    /// Controller responsible for administrating, fetching and inserting customer data
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        IRepository<SessionKey, SessionKeyParams> sessionKeyRepo;
        IRepository<Customer, CustomerParams> customerRepo;
        IPasswordService passwordService;
        ILogger logger;

        [HttpGet("confirmation")]
        public async Task<IActionResult> YoureGoodFam() {
            return Ok("You're good fam");
        }

        /// <summary>
        /// Creates a new instance of the customer controller with the specified repository and password service
        /// </summary>
        /// <param name="customerRepo">the desired customer repository</param>
        /// <param name="passwordService">´the desired password service</param>
        public CustomerController(
            [FromServices]IRepository<SessionKey, SessionKeyParams> sessionKeyRepo,
            [FromServices]IRepository<Customer, CustomerParams> customerRepo,
            [FromServices]IPasswordService passwordService,
            [FromServices]ILogger logger)
        {
            this.sessionKeyRepo = sessionKeyRepo;
            this.customerRepo = customerRepo;
            this.passwordService = passwordService;
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
            if (await customerRepo.ExistsAsync(request.Email).ConfigureAwait(true))
                return BadRequest("An account using that email already exists");

            //create a new password hash/salt for the customer
            //based on the password parameter
            byte[] password = new byte[64],
                    salt = new byte[32];

            try
            {
                password = passwordService.CreatePassword(request.Password, out salt);

            }
            catch (ArgumentNullException)
            {
                await logger
                    .LogAsync("a null parameter was passed to the password service in the CreateCustomer action in the customer controller")
                    .ConfigureAwait(false);
            }

            //initialize an empty customer object
            Customer cust = null;

            try
            {
                //attempt to insert the customer into the database
                cust =
                    await customerRepo
                        .InsertAsync(
                            new CustomerParams()
                            {
                                Key = null,
                                Customer = new Customer(
                                    request.Email,
                                    request.FirstName,
                                    request.LastName,
                                    request.CityId,
                                    request.Street,
                                    request.StreetNumber,
                                    password,
                                    salt
                                    )
                            })
                        .ConfigureAwait(false);
            }
            //log potential database errors
            catch (DbException e)
            {
                await logger.LogAsync($"An error occured when trying to insert user with email {request.Email} into the database: DbError-{e.ErrorCode} {e.Message}").ConfigureAwait(false);
            }
            catch (Exception)
            {
                throw;
            }

            //return 200OK if the insert was successful
            if (cust != null)
                return Ok("Signed up customer successfully!");
            else
                return BadRequest("Unable to sign up user");
        }

        [HttpPost]
        [Route("signin")]
        public async Task<IActionResult> SigninCustomer([FromBody]CustomerSignin request)
        {
            //check parameters
            if (request == null)
                return BadRequest("the supplied request object was null");
            if (request.email == null || request.email.Length == 0)
                return BadRequest("the supplied email was null or empty");
            if (request.password == null || request.password.Length == 0)
                return BadRequest("the supplied password was null or empty");

            Customer cust;
            try
            {
                //check if customer exists and fetch it if it does
                cust = await customerRepo.FindAsync(request.email).ConfigureAwait(true);
            }
            catch (Exception)
            {
                throw;
            }

            if (cust == null)
                return BadRequest($"an account with the email '{request.email}' does not exist");

            SessionKey key;
            try
            {
                //check if customer password matches
                if (!passwordService.ComparePasswords(request.password, cust.GetPassword(), cust.GetSalt()))
                    return Unauthorized("invalid email/password combination");

                //create new session key and return it to the requestee
                key = await sessionKeyRepo.InsertAsync(new SessionKeyParams(request.email)).ConfigureAwait(true);
            }
            catch (Exception)
            {
                throw;
            }

            if (key != null)
                return Ok(key);

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