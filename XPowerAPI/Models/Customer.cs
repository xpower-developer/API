using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XPowerAPI.Models
{
    public class Customer
    {
        private byte[] salt;
        private byte[] password;
        public int CustomerId { get; set; }
        public string Email { get; set; }
        public int CityId { get; set; }
        public long StreetId { get; set; }
        public string StreetNumber { get; set; }
        public DateTime Created { get; set; }

        /// <summary>
        /// Creates a customer from existing values
        /// </summary>
        public Customer(int customerId, string email, int cityId, long streetId, string streetNumber, DateTime created, byte[] password, byte[] salt)
        {
            CustomerId = customerId;
            Email = email;
            CityId = cityId;
            StreetId = streetId;
            StreetNumber = streetNumber;
            Created = created;
            this.password = password;
            this.salt = salt;
        }

        /// <summary>
        /// Creates a new customer
        /// </summary>
        public Customer(string email, int cityId, long streetId, string streetNumber, byte[] password, byte[] salt)
        {
            Email = email;
            CityId = cityId;
            StreetId = streetId;
            StreetNumber = streetNumber;
            this.password = password;
            this.salt = salt;
        }

        /// <summary>
        /// Creates a new customer without password details for display purposes
        /// </summary>
        public Customer(int customerId, string email, int cityId, long streetId, string streetNumber, DateTime created)
        {
            CustomerId = customerId;
            Email = email;
            CityId = cityId;
            StreetId = streetId;
            StreetNumber = streetNumber;
            Created = created;
        }

        public byte[] GetSalt()
        {
            return (byte[])salt.Clone();
        }

        public byte[] GetPassword()
        {
            return (byte[])password.Clone();
        }

        public void SetSalt(byte[] value) {
            salt = value;
        }

        public void SetPassword(byte[] value)
        {
            password = value;
        }
    }
}
