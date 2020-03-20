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
        private readonly string passwordPattern;
        private int minLength, maxLength;
        private IHashingService hashingService;

        public PasswordService(
            [FromServices]IHashingService service,
            int min, 
            int max)
        {
            this.MinLength = min;
            this.MaxLength = max;
            this.hashingService = service;

            this.passwordPattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{" + minLength + "," + maxLength + "}$";
        }

        public int MinLength { get => minLength; private set => minLength = value; }
        public int MaxLength { get => maxLength; private set => maxLength = value; }
        public string PasswordPattern { get => passwordPattern;}

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
            if (password.Length < MinLength || password.Length > MaxLength)
                throw new ArgumentOutOfRangeException(nameof(password));

            if (!Regex.IsMatch(password, passwordPattern))
                throw new ArgumentOutOfRangeException(nameof(password));
            else
                return hashingService.CreateHash(Encoding.UTF8.GetBytes(password), out salt);
        }
    }
}
