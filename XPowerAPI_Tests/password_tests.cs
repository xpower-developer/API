using System;
using XPowerAPI.Services.Security;
using XPowerAPI.Services.Security.Account;
using Xunit;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace XPowerAPI_Tests
{
    public class password_tests
    {
        IConfigurationRoot conf =
                new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json")
                    .Build();

        [Fact]
        public void passwordservice_returns_correct_hash()
        {
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

            byte[]  salt1,
                    salt2,
                    salt3;
            //correct passwords
            byte[] correctpass1 = pwd.CreatePassword("he1llothereguy!Q", out salt1);
            byte[] correctpass2 = pwd.CreatePassword("helloth!@Sereguy2", out salt2);
            //incorrect length
            Assert.Throws<ArgumentOutOfRangeException>(() => pwd.CreatePassword("hel1!", out salt3));
            //incorrect character set (no A-Z)
            Assert.Throws<ArgumentOutOfRangeException>(() => pwd.CreatePassword("hel1!rrrrrr", out salt3));

            Assert.NotNull(correctpass1);
            Assert.NotNull(correctpass2);
            Assert.NotNull(salt1);
            Assert.NotNull(salt2);

            Assert.True(correctpass1.Length == 64);
            Assert.True(correctpass2.Length == 64);

            Assert.NotEqual(correctpass1, correctpass2);
        }

        [Fact]
        public void passwords_are_unique() {
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

            byte[] salt1, salt2;
            byte[]  correctpass1 = pwd.CreatePassword("hellothereguy123@A", out salt1), 
                    correctpass2 = pwd.CreatePassword("hellothereguy123@A", out salt2);

            Assert.NotNull(correctpass1);
            Assert.NotNull(correctpass2);
            Assert.NotNull(salt1);
            Assert.NotNull(salt2);

            Assert.True(correctpass1.Length == 64);
            Assert.True(correctpass2.Length == 64);
            //make sure salt is different
            Assert.NotEqual(correctpass1, correctpass2);
            Assert.NotEqual(salt1, salt2);
        }

        [Fact]
        public void passwords_compared_correctly() {
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

            string passString = "hellothereguy123A!";  
            byte[] salt1 = new byte[32];
            byte[] correctpass1 = pwd.CreatePassword(passString, out salt1);

            Assert.NotNull(correctpass1);
            Assert.NotNull(salt1);
            Assert.True(correctpass1.Length == 64);

            Assert.True(pwd.ComparePasswords(passString, correctpass1, salt1));
            Assert.False(pwd.ComparePasswords("notthesamepassword?A1", correctpass1, salt1));
        }
    }
}
