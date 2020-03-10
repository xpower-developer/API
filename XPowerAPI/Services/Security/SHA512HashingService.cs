using Org.BouncyCastle.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace XPowerAPI.Services.Security
{
    public class SHA512HashingService : IHashingService
    {
        private SecureRandom random = new SecureRandom();

        public bool CompareHash(in byte[] input, byte[] salt, in byte[] hash)
        {
            //check params
            if (input == null)
                throw new ArgumentNullException(nameof(input));
            if (hash == null)
                throw new ArgumentNullException(nameof(hash));
            if (input.Length < 1)
                throw new ArgumentOutOfRangeException(nameof(input));

            //compute hash
            byte[] freshHash;
            using (SHA512 sha = new SHA512Managed())
            {
                byte[] data = new byte[input.Length + 32];
                Array.Copy(input, 0, data, 0, input.Length);
                Array.Copy(salt, input.Length, data, 0, 32);
                freshHash = sha.ComputeHash(data);
            }

            //compare hashes
            return hash.Equals(freshHash);
        }

        public byte[] CreateHash(in byte[] input, out byte[] salt)
        {
            //assign salt
            salt = new byte[32];
            //check params
            if (input == null)
                throw new ArgumentNullException(nameof(input));
            if (input.Length < 1)
                throw new ArgumentOutOfRangeException(nameof(input));

            //generate salt
            random.NextBytes(salt);
            //compute hash
            byte[] hash;
            using (SHA512 sha = new SHA512Managed())
            {
                byte[] data = new byte[input.Length + 32];
                Array.Copy(input, 0, data, 0, input.Length);
                Array.Copy(salt, input.Length, data, 0, 32);
                hash = sha.ComputeHash(data);
            }

            //return finished hash
            return hash;
        }
    }
}
