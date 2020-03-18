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
            if (input == null || input.Length == 0)
                throw new ArgumentNullException(nameof(input));
            if (hash == null || hash.Length == 0)
                throw new ArgumentNullException(nameof(hash));
            if (input.Length < 0)
                throw new ArgumentOutOfRangeException(nameof(input));

            //compute hash
            byte[] freshHash;
            using (SHA512 sha = new SHA512Managed())
            {
                byte[] data = new byte[input.Length + 32];
                Array.Copy(input, 0, data, 0, input.Length);
                Array.Copy(salt, 0, data, input.Length, 32);
                freshHash = sha.ComputeHash(data);
            }

            //compare hashes
            for (int i = 0; i < hash.Length; i++)
            {
                if (hash[i] != freshHash[i]) return false;
            }

            return true;
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
                Array.Copy(salt, 0, data, input.Length, 32);
                hash = sha.ComputeHash(data);
            }

            //return finished hash
            return hash;
        }
    }
}
