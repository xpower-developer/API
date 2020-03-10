using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace XPowerAPI.Services.Security.Account
{
    public class PasswordService : IPasswordService
    {
        const string PASSWORD_PATTERN = "^[\\w]([\\d\\w!\"#$%&'()*+,\\-./:;<=>?@\\[\\]\\^_`{|}~])+";
        int minLength, maxLength;
        IHashingService hashingService;

        public PasswordService(
            [FromServices]IHashingService service,
            int min, 
            int max)
        {
            this.minLength = min;
            this.maxLength = max;
            this.hashingService = service;
        }

        public bool ComparePasswords(string password, byte[] hash, byte[] salt)
        {
            if (password == null || password.Length == 0)
                throw new ArgumentNullException(nameof(password));
            if (hash == null || hash.Length == 0)
                throw new ArgumentNullException(nameof(hash));
            if (salt == null || salt.Length == 0)
                throw new ArgumentNullException(nameof(salt));

            return hashingService.CompareHash(Encoding.UTF8.GetBytes(password), salt, hash);
        }

        public byte[] CreatePassword(string password, out byte[] salt)
        {
            if (password == null || password.Length == 0)
                throw new ArgumentNullException(nameof(password));
            if (password.Length < minLength || password.Length > maxLength)
                throw new ArgumentOutOfRangeException(nameof(password));

            if (!Regex.IsMatch(password, PASSWORD_PATTERN))
                throw new ArgumentOutOfRangeException(nameof(password));
            else
                return hashingService.CreateHash(Encoding.UTF8.GetBytes(password), out salt);
        }
    }
}
