using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XPowerAPI.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public byte[] Salt { get; set; }

        public Customer(int id, string username, string password, byte[] salt)
        {
            Id = id;
            Username = username;
            Password = password;
            Salt = salt;
        }
    }
}
