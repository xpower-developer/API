using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XPowerAPI.Controllers;
using XPowerAPI.Models;
using XPowerAPI.Models.Params;
using XPowerAPI.Repository;
using XPowerAPI.Services.Security.Account;
using XPowerAPI.Logging;
using System.Data.Common;
using Microsoft.AspNetCore.Mvc;

namespace XPowerAPI.Services.Account
{
    public class CustomerService : ICustomerService
    {
        private bool isDisposed = false;

        private IRepository<Customer, CustomerParams> customerRepo;
        private IPasswordService passwordService;
        private ILogger logger;

        public CustomerService(
            [FromServices]IRepository<Customer, CustomerParams> customerRepo,
            [FromServices]IPasswordService passwordService,
            [FromServices]ILogger logger)
        {
            this.customerRepo = customerRepo;
            this.passwordService = passwordService;
            this.logger = logger;
        }

        public async Task<bool> CustomerExists(string email) {
            return await customerRepo.ExistsAsync(email).ConfigureAwait(true);
        }

        public async Task<Customer> CreateCustomer(CustomerSignup request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

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

            return cust;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool desposing)
        {
            if (isDisposed) return;

            if (desposing)
            {
                customerRepo.Dispose();
                logger.Dispose();
            }
        }

        public async Task<Customer> FindCustomer(string email)
        {
            return await customerRepo.FindAsync(email).ConfigureAwait(true);
        }
    }
}
