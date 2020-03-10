using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XPowerAPI.Services.Security.Account
{
    public interface IPasswordService
    {
        byte[] CreatePassword(string password, out byte[] salt);
        bool ComparePasswords(string password, byte[] hash, byte[] salt);
    }
}
