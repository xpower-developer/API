using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XPowerAPI.Models
{
    public class Customer
    {
        private byte[] salt;
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public byte[] GetSalt()
        {
            return (byte[])salt.Clone();
        }

        public void SetSalt(byte[] value) {
            salt = value;
        }

        public Customer(in int id, in string username, in string password, in byte[] salt)
        {
            Id = id;
            Username = username;
            Password = password;
            SetSalt(salt);
        }

        public Customer(in string username, in string password)
        {
            Username = username;
            Password = password;
        }
    }
}
