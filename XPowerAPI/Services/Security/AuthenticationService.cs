using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XPowerAPI.Controllers;
using XPowerAPI.Models;
using XPowerAPI.Models.Params;
using XPowerAPI.Repository;
using XPowerAPI.Services.Security.Account;

namespace XPowerAPI.Services.Security
{
    public class AuthenticationService : IAuthenticationService
    {
        private bool isDisposed = false;

        private IRepository<Customer, CustomerParams> customerRepo;
        private IRepository<SessionKey, SessionKeyParams> sessionKeyRepo;
        private IPasswordService passwordService;

        public AuthenticationService(
            [FromServices]IRepository<Customer, CustomerParams> customerRepo,
            [FromServices]IRepository<SessionKey, SessionKeyParams> sessionKeyRepo, 
            [FromServices]IPasswordService passwordService)
        {
            this.customerRepo = customerRepo;
            this.sessionKeyRepo = sessionKeyRepo;
            this.passwordService = passwordService;
        }

        public async Task<SessionKey> AuthenticateUser(CustomerSignin request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

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
                throw new ArgumentException($"a customer with the email {request.email} does not exist");

            SessionKey key = null;
            try
            {
                //check if customer password matches
                if (passwordService.ComparePasswords(request.password, cust.GetPassword(), cust.GetSalt()))
                {
                    //create new session key and return it to the requestee
                    key = await sessionKeyRepo.InsertAsync(new SessionKeyParams(request.email)).ConfigureAwait(true);
                }
            }
            catch (Exception)
            {
                throw;
            }

            return key;
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
                sessionKeyRepo.Dispose();
                customerRepo.Dispose();
            }
        }
    }
}
