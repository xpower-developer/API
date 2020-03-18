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
        public int CustomerId { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Email { get; private set; }
        public int CityId { get; private set; }
        public string City { get; private set; }
        public long StreetId { get; private set; }
        public string Street { get; private set; }
        public string StreetNumber { get; private set; }
        public DateTime Created { get; private set; }

        /// <summary>
        /// Creates a customer from existing values
        /// </summary>
        public Customer(
            int customerId, 
            string email, 
            string fname, 
            string lname, 
            int cityId, 
            string city, 
            long streetId, 
            string street, 
            string streetNumber, 
            DateTime created, 
            byte[] password, 
            byte[] salt)
        {
            CustomerId = customerId;
            FirstName = fname;
            LastName = lname;
            Email = email;
            CityId = cityId;
            City = city;
            StreetId = streetId;
            Street = street;
            StreetNumber = streetNumber;
            Created = created;
            this.password = password;
            this.salt = salt;
        }

        /// <summary>
        /// Creates a new customer
        /// </summary>
        public Customer(
            string email, 
            string fname, 
            string lname, 
            int cityId, 
            string street, 
            string streetNumber, 
            byte[] password, 
            byte[] salt)
        {
            Email = email;
            FirstName = fname;
            LastName = lname;
            CityId = cityId;
            Street = street;
            StreetNumber = streetNumber;
            this.password = password;
            this.salt = salt;
        }

        /// <summary>
        /// Creates a new customer without password details for display purposes
        /// </summary>
        public Customer(
            int customerId, 
            string email, 
            string fname, 
            string lname, 
            string city, 
            int cityId, 
            string street, 
            long streetId, 
            string streetNumber, 
            DateTime created)
        {
            CustomerId = customerId;
            Email = email;
            FirstName = fname;
            LastName = lname;
            City = city;
            CityId = cityId;
            Street = street;
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
