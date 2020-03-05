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
using XPowerAPI.Models.Params;
using XPowerAPI.Repository.Collections;

namespace XPowerAPI.Repository
{
    public class CustomerRepository : IRepository<Customer, CustomerParams>
    {
        ILogger logger;

        GraphClient client = new GraphClient(new Uri(""), "user", "pass");
        MySqlConnection con = new MySqlConnection("");

        public CustomerRepository(ILogger logger)
        {
            this.logger = logger;
        }

        ~CustomerRepository() {
            con.Dispose();
            client.Dispose();
        }

        public int Count()
        {
            throw new NotImplementedException();
        }

        public void Delete(object id)
        {
            throw new NotImplementedException();
        }

        public void Delete(CustomerParams entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(params CustomerParams[] entities)
        {
            throw new NotImplementedException();
        }

        public void Delete(IEnumerable<CustomerParams> entities)
        {
            throw new NotImplementedException();
        }

        public bool Exists(object key)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ExistsAsync(object key)
        {
            throw new NotImplementedException();
        }

        public Customer Find(params object[] keyValues)
        {
            throw new NotImplementedException();
        }

        public async Task<Customer> FindAsync(params object[] keyValues)
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
        }

        public Task<Customer> FindAsync(object[] keyValues, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Customer> FromSql(string sql, params object[] parameters)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Customer> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Customer>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public IPagedList<Customer> GetPagedList(object[] keyValues, int pageIndex = 0, int pageSize = 20)
        {
            throw new NotImplementedException();
        }

        public Task<IPagedList<Customer>> GetPagedListAsync(object[] keyValues, int pageIndex = 0, int pageSize = 20, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Customer Insert(CustomerParams entity)
        {
            throw new NotImplementedException();
        }

        public void Insert(params CustomerParams[] entities)
        {
            throw new NotImplementedException();
        }

        public void Insert(IEnumerable<CustomerParams> entities)
        {
            throw new NotImplementedException();
        }

        public Task InsertAsync(params CustomerParams[] entities)
        {
            throw new NotImplementedException();
        }

        public Task InsertAsync(IEnumerable<CustomerParams> entities, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public void Update(CustomerParams entity)
        {
            throw new NotImplementedException();
        }

        public void Update(params CustomerParams[] entities)
        {
            throw new NotImplementedException();
        }

        public void Update(IEnumerable<CustomerParams> entities)
        {
            throw new NotImplementedException();
        }
    }
}
