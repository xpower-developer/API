using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;
using XPowerAPI.Controllers;
using XPowerAPI.Logging;
using XPowerAPI.Models;
using XPowerAPI.Models.Params;
using XPowerAPI.Repository;
using XPowerAPI.Services.Security;
using XPowerAPI.Services.Security.Account;
using Xunit;

namespace XPowerAPI_Tests
{
    public class database_tests
    {
        IConfigurationRoot conf =
                new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json")
                    .Build();

        [Fact]
        public void maindb_can_connect()
        {
            string connectionString = conf.GetSection("ConnectionStrings")["maria"];

            MySqlConnection con = new MySqlConnection(connectionString);
            con.Open();
            Assert.True(con.State == System.Data.ConnectionState.Open);
            con.Close();
            Assert.True(con.State == System.Data.ConnectionState.Closed);
        }

        [Fact]
        public void testdb_can_connect()
        {
            string connectionString = conf.GetSection("ConnectionStrings")["mariaTest"];

            MySqlConnection con = new MySqlConnection(connectionString);
            con.Open();
            Assert.True(con.State == System.Data.ConnectionState.Open);
            con.Close();
            Assert.True(con.State == System.Data.ConnectionState.Closed);
        }

        [Fact]
        public void testdb_can_create_user()
        {
            ClearTestDB();

            string connectionString = conf.GetSection("ConnectionStrings")["mariaTest"];

            IPasswordService pwd =
                new PasswordService(
                    new SHA512HashingService(),
                    ((int)conf
                        .GetSection("Account")
                        .GetSection("Security")
                        .GetValue(
                            typeof(int),
                            "PasswordMinLength")),
                    ((int)conf
                        .GetSection("Account")
                        .GetSection("Security")
                        .GetValue(
                            typeof(int),
                            "PasswordMaxLength")));
            IRepository<Customer, CustomerParams> repo =
                new CustomerRepository(
                    new MySqlConnection(connectionString),
                    new EmptyLogger());

            byte[] p, s = new byte[32];
            p = pwd.CreatePassword("goodpassword12@B", out s);

            Customer cust = repo.Insert(
                new CustomerParams
                {
                    Key = null,
                    Customer = new Customer(
                    "guyman@gmail.com",
                    "guy",
                    "man",
                    4100,
                    "road",
                    "road number",
                    p,
                    s)
                });

            Assert.NotNull(cust);
            Assert.True(cust.FirstName.Equals("guy"));
            Assert.True(cust.LastName.Equals("man"));
            Assert.True(cust.Email.Equals($"guyman@gmail.com"));
            Assert.True(cust.Street.Equals("road"));
            Assert.True(cust.StreetNumber.Equals("road number"));
        }

        [Fact]
        public void testdb_can_login_user()
        {
            ClearTestDB();

            string connectionString = conf.GetSection("ConnectionStrings")["mariaTest"];

            MySqlConnection con = new MySqlConnection(connectionString);

            //"goodpassword12@B"
            CustomerController ctl = new CustomerController(
                new SessionKeyRepository(
                    con,
                    new EmptyLogger()),
                new CustomerRepository(
                    con,
                    new EmptyLogger()),
                new PasswordService(
                    new SHA512HashingService(),
                    ((int)conf
                            .GetSection("Account")
                            .GetSection("Security")
                            .GetValue(
                                typeof(int),
                                "PasswordMinLength")),
                    ((int)conf
                           .GetSection("Account")
                           .GetSection("Security")
                           .GetValue(
                               typeof(int),
                               "PasswordMaxLength"))),
                new EmptyLogger());

            ctl.CreateCustomer(
                new CustomerSignup(
                    "guy",
                    "man",
                    "guy@man.com",
                    "GuyM@n1337",
                    4100,
                    "lillegade",
                    "888.th v.st"))
                .ConfigureAwait(false);

            ctl.SigninCustomer(
                new CustomerSignin(
                    "guy@man.com",
                    "GuyM@n1337"))
                .ConfigureAwait(false);

            con.Open();
            MySqlCommand cmd =
                new MySqlCommand(
                    "select * from SessionKey join Customer on Customer.Email = 'guy@man.com' where Customer.CustomerId = SessionKey.CustomerId limit 1;", con)
                {
                    CommandType = System.Data.CommandType.Text
                };

            //the uuid of the sessionkey
            Guid result = (Guid)cmd.ExecuteScalar();

            Assert.True(result != null);
            Assert.True(result.ToString().Length == 36);

            con.Close();
            con.Dispose();
            cmd.Dispose();
        }

        private void ClearTestDB()
        {
            var conf =
                new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json")
                    .Build();

            string connectionString = conf.GetSection("ConnectionStrings")["mariaTest"];
            MySqlConnection con = new MySqlConnection(connectionString);
            MySqlCommand cmd =
                new MySqlCommand(
                    "ClearDBForTesting",
                    con)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };

            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
            con.Dispose();
            cmd.Dispose();
        }
    }
}
