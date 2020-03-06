using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using Neo4jClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using XPowerAPI.Models;
using XPowerAPI.Repository.Collections;

namespace XPowerAPI.Repository
{
    public class CustomerRepository 
    {
        /*public async Task<Customer> FindAsync(params object[] keyValues)
        {
            if (keyValues.Length == 0 || keyValues[0] == null)
                return null;

            int id = 0;
            int.TryParse((string)keyValues[0], out id);

            if (id == 0)
                return null;

            try
            {
                MySqlCommand cmd = new MySqlCommand("CALL GetCustomerById($key);", con);
                cmd.Parameters.Add("$key", MySqlDbType.Int32);
                cmd.Parameters["$key"].Value = id;

                DbDataReader reader = await cmd.ExecuteReaderAsync();

                if (!reader.HasRows)
                    return null;

                Customer customer = null;
                while (reader.Read())
                {
                    byte[] salt = new byte[32];
                    reader.GetBytes(reader.GetOrdinal("Salt"), 0, salt, 0, 32);

                    customer = new Customer(
                            reader.GetInt32(reader.GetOrdinal("CustomerId")),
                            reader.GetString(reader.GetOrdinal("Username")),
                            reader.GetString(reader.GetOrdinal("Password")),
                            salt
                        );
                }

                return customer;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                await con.CloseAsync();
            }
        }*/
    }
}
