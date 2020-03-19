using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XPowerAPI.Controllers;
using XPowerAPI.Models;

namespace XPowerAPI.Services.Account
{
    interface ICustomerService : IDisposable
    {
        /// <summary>
        /// Creates a new customer with the specified values, so long as a customer with the specified email does not exist
        /// </summary>
        /// <param name="request">the request parameters</param>
        /// <returns>a shallow copy of the newly created customer</returns>
        Task<Customer> CreateCustomer(CustomerSignup request);

        Task<Customer> FindCustomer(string email);

        Task<bool> CustomerExists(string email);
    }
}
