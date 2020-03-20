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
using XPowerAPI.Logging;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;

namespace XPowerAPI.Repository
{
    public class CustomerRepository : IRepository<Customer, CustomerParams>, IDisposable
    {
        private bool isDisposed = false;

        private MySqlConnection con;
        private ILogger logger;

        public CustomerRepository(
            [FromServices]MySqlConnection con,
            [FromServices]ILogger logger)
        {
            this.con = con;
            this.logger = logger;
        }

        ~CustomerRepository()
        {
            Dispose(false);
        }

        public int Count()
        {
            throw new NotImplementedException();
        }

        public Task<int> CountAsync()
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

        public Task DeleteAsync(object id)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(CustomerParams entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(params CustomerParams[] entities)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(IEnumerable<CustomerParams> entities)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool desposing)
        {
            if (isDisposed) return;

            if (desposing)
            {
                con.Dispose();
            }
        }

        public bool Exists(object key)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));

            string val = key.ToString();

            if (string.IsNullOrWhiteSpace(val) || val.Length == 0)
                throw new ArgumentNullException(val);

            MySqlCommand cmd;

            //val is an email
            if (val.Contains('@', StringComparison.OrdinalIgnoreCase))
            {
                cmd = new MySqlCommand("GetCustomerIdByEmail", con)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                cmd.Parameters.Add("email", MySqlDbType.VarChar);
                cmd.Parameters["email"].Value = val;
            }
            //val is a key
            else
            {
                cmd = new MySqlCommand("GetCustomerIdByAssociatedKey", con)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                cmd.Parameters.Add("apikey", MySqlDbType.VarChar);
                cmd.Parameters["apikey"].Value = val;
            }

            bool res = false;

            try
            {
                con.Open();
                res = cmd.ExecuteScalar() != null;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                cmd.Dispose();
                con.Close();
            }

            return res;
        }

        public Task<bool> ExistsAsync(object key)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));

            string val = key.ToString();

            if (string.IsNullOrWhiteSpace(val) || val.Length == 0)
                throw new ArgumentNullException(val);

            MySqlCommand cmd;

            //val is an email
            if (val.Contains('@', StringComparison.OrdinalIgnoreCase))
            {
                cmd = new MySqlCommand("GetCustomerIdByEmail", con)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                cmd.Parameters.Add("email", MySqlDbType.VarChar);
                cmd.Parameters["email"].Value = val;
            }
            //val is a key
            else
            {
                cmd = new MySqlCommand("GetCustomerIdByAssociatedKey", con)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                cmd.Parameters.Add("apikey", MySqlDbType.VarChar);
                cmd.Parameters["apikey"].Value = val;
            }

            bool res = false;

            try
            {
                con.Open();
                res = cmd.ExecuteScalar() != null;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                cmd.Dispose();
                con.Close();
            }

            return Task.FromResult(res);
        }

        public Customer Find(params object[] keyValues)
        {
            if (keyValues == null || keyValues.Length == 0 || keyValues[0] == null)
                throw new ArgumentNullException(nameof(keyValues));

            MySqlCommand cmd = new MySqlCommand("GetCustomerByEmail", con) { CommandType = System.Data.CommandType.StoredProcedure };
            cmd.Parameters.Add(new MySqlParameter()
            {
                ParameterName = "email",
                Value = keyValues[0],
                MySqlDbType = MySqlDbType.VarChar,
                Direction = System.Data.ParameterDirection.Input
            });

            Customer cust = null;
            try
            {
                con.Open();
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    byte[] pass = new byte[64],
                            salt = new byte[32];

                    reader.GetBytes(reader.GetOrdinal("Password"), 0, pass, 0, 64);
                    reader.GetBytes(reader.GetOrdinal("Salt"), 0, salt, 0, 64);

                    cust = new Customer(
                            reader.GetInt32(reader.GetOrdinal("CustomerId")),
                            reader.GetString(reader.GetOrdinal("Email")),
                            reader.GetString(reader.GetOrdinal("FirstName")),
                            reader.GetString(reader.GetOrdinal("LastName")),
                            reader.GetInt32(reader.GetOrdinal("CityId")),
                            reader.GetString(reader.GetOrdinal("CityName")),
                            0/*reader.GetInt64(reader.GetOrdinal("StreetId"))*/, //street table not yet available
                            ""/*reader.GetString(reader.GetOrdinal("Street"))*/,
                            reader.GetString(reader.GetOrdinal("StreetNumber")),
                            reader.GetDateTime(reader.GetOrdinal("Created")),
                            pass,
                            salt
                            );
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                con.Close();
                cmd.Dispose();
            }

            return cust;
        }

        public async Task<Customer> FindAsync(params object[] keyValues)
        {
            if (keyValues == null || keyValues.Length == 0 || keyValues[0] == null)
                throw new ArgumentNullException(nameof(keyValues));

            MySqlCommand cmd = new MySqlCommand("GetCustomerByEmail", con) { CommandType = System.Data.CommandType.StoredProcedure };
            cmd.Parameters.Add(new MySqlParameter()
            {
                ParameterName = "email",
                Value = keyValues[0],
                MySqlDbType = MySqlDbType.VarChar,
                Direction = System.Data.ParameterDirection.Input
            });

            Customer cust = null;
            try
            {
                using (CancellationTokenSource tkn = new CancellationTokenSource(5000))
                {
                    //open connection
                    await con.OpenAsync(tkn.Token).ConfigureAwait(true);

                    //if unable to connect to the db, cancel the request
                    if (tkn.IsCancellationRequested)
                        return null;
                }

                DbDataReader reader = await cmd.ExecuteReaderAsync().ConfigureAwait(false);

                while (await reader.ReadAsync().ConfigureAwait(false))
                {
                    byte[]  pass = new byte[64],
                            salt = new byte[32];

                    reader.GetBytes(reader.GetOrdinal("Password"), 0, pass, 0, 64);
                    reader.GetBytes(reader.GetOrdinal("Salt"), 0, salt, 0, 32);

                    cust = new Customer(
                            reader.GetInt32(reader.GetOrdinal("CustomerId")),
                            reader.GetString(reader.GetOrdinal("Email")),
                            reader.GetString(reader.GetOrdinal("FirstName")),
                            reader.GetString(reader.GetOrdinal("LastName")),
                            reader.GetInt32(reader.GetOrdinal("CityId")),
                            reader.GetString(reader.GetOrdinal("CityName")),
                            0/*reader.GetInt64(reader.GetOrdinal("StreetId"))*/, //street table not yet available
                            ""/*reader.GetString(reader.GetOrdinal("Street"))*/,
                            reader.GetString(reader.GetOrdinal("StreetNumber")),
                            reader.GetDateTime(reader.GetOrdinal("Created")),
                            pass,
                            salt
                            );
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                await con.CloseAsync().ConfigureAwait(false);
                cmd.Dispose();
            }

            return cust;
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

        public IPagedList<Customer> GetPagedList(object[] keyValues, int pageNumber = 0, int pageSize = 20)
        {
            throw new NotImplementedException();
        }

        public Task<IPagedList<Customer>> GetPagedListAsync(object[] keyValues, int pageNumber = 0, int pageSize = 20, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Customer Insert(CustomerParams entity)
        {
            //check parameter validity
            if (entity == null ||
                entity.Customer == null)
                throw new ArgumentNullException(nameof(entity));

            //initialize the command
            MySqlCommand cmd = new MySqlCommand("CreateCustomer", con);
            int res;

            try
            {
                //declare command type
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                //add parameters
                cmd.Parameters.AddRange(
                    new MySqlParameter[] {
                        new MySqlParameter("email", MySqlDbType.VarChar),
                        new MySqlParameter("fname", MySqlDbType.VarChar),
                        new MySqlParameter("lname", MySqlDbType.VarChar),
                        new MySqlParameter("pass", MySqlDbType.Binary),
                        new MySqlParameter("salt", MySqlDbType.Binary),
                        new MySqlParameter("cityId", MySqlDbType.Int32),
                        new MySqlParameter("street", MySqlDbType.VarChar),
                        new MySqlParameter("streetNumber", MySqlDbType.VarChar)
                    });

                //add paramter values
                cmd.Parameters["email"].Value = entity.Customer.Email;
                cmd.Parameters["fname"].Value = entity.Customer.FirstName;
                cmd.Parameters["lname"].Value = entity.Customer.LastName;
                cmd.Parameters["pass"].Value = entity.Customer.GetPassword();
                cmd.Parameters["salt"].Value = entity.Customer.GetSalt();
                cmd.Parameters["cityId"].Value = entity.Customer.CityId;
                cmd.Parameters["street"].Value = entity.Customer.Street;
                cmd.Parameters["streetNumber"].Value = entity.Customer.StreetNumber;

                //open connection
                con.Open();

                //execute the query
                res = cmd.ExecuteNonQuery();
            }
            //TODO: useful logging
            catch (Exception)
            {
                throw;
            }
            finally
            {
                cmd.Dispose();
                con.Close();
            }

            if (res != 0)
                //return the customer object if successfull
                return entity.Customer;
            else
                //return null if insert was unsuccessfull
                return null;
        }

        public void Insert(params CustomerParams[] entities)
        {
            //check parameter validity
            if (entities.Length == 0 || entities == null)
                throw new ArgumentNullException(nameof(entities));

            //initialize the command
            MySqlCommand cmd = new MySqlCommand("CreateCustomer", con);

            try
            {
                //declare command type
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                //add parameters
                cmd.Parameters.AddRange(
                    new MySqlParameter[] {
                        new MySqlParameter("email", MySqlDbType.VarChar),
                        new MySqlParameter("fname", MySqlDbType.VarChar),
                        new MySqlParameter("lname", MySqlDbType.VarChar),
                        new MySqlParameter("pass", MySqlDbType.Binary),
                        new MySqlParameter("salt", MySqlDbType.Binary),
                        new MySqlParameter("cityId", MySqlDbType.Int32),
                        new MySqlParameter("street", MySqlDbType.VarChar),
                        new MySqlParameter("streetNumber", MySqlDbType.VarChar)
                    });

                for (int i = 0; i < entities.Length; i++)
                {
                    if (entities[i] == null)
                        continue;

                    //add paramter values
                    cmd.Parameters["email"].Value = entities[i].Customer.Email;
                    cmd.Parameters["fname"].Value = entities[i].Customer.FirstName;
                    cmd.Parameters["lname"].Value = entities[i].Customer.LastName;
                    cmd.Parameters["pass"].Value = entities[i].Customer.GetPassword();
                    cmd.Parameters["salt"].Value = entities[i].Customer.GetSalt();
                    cmd.Parameters["cityId"].Value = entities[i].Customer.CityId;
                    cmd.Parameters["street"].Value = entities[i].Customer.Street;
                    cmd.Parameters["streetNumber"].Value = entities[i].Customer.StreetNumber;

                    //open connection
                    con.Open();

                    //execute the query
                    cmd.ExecuteNonQuery();
                }
            }
            //TODO: useful logging
            catch (Exception)
            {
                throw;
            }
            finally
            {
                cmd.Dispose();
                con.Close();
            }
        }

        public void Insert(IEnumerable<CustomerParams> entities)
        {
            Insert(entities.ToArray());
        }

        public async Task<Customer> InsertAsync(CustomerParams entity)
        {
            //check parameter validity
            if (entity == null ||
                entity.Customer == null)
                throw new ArgumentNullException(nameof(entity));

            //initialize the command
            MySqlCommand cmd = new MySqlCommand("CreateCustomer", con);
            int res;

            try
            {
                //declare command type
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                //add parameters
                cmd.Parameters.AddRange(
                    new MySqlParameter[] {
                        new MySqlParameter("email", MySqlDbType.VarChar),
                        new MySqlParameter("fname", MySqlDbType.VarChar),
                        new MySqlParameter("lname", MySqlDbType.VarChar),
                        new MySqlParameter("pass", MySqlDbType.Binary),
                        new MySqlParameter("salt", MySqlDbType.Binary),
                        new MySqlParameter("cityId", MySqlDbType.Int32),
                        new MySqlParameter("street", MySqlDbType.VarChar),
                        new MySqlParameter("streetNumber", MySqlDbType.VarChar)
                    });

                //add paramter values
                cmd.Parameters["email"].Value = entity.Customer.Email;
                cmd.Parameters["fname"].Value = entity.Customer.FirstName;
                cmd.Parameters["lname"].Value = entity.Customer.LastName;
                cmd.Parameters["pass"].Value = entity.Customer.GetPassword();
                cmd.Parameters["salt"].Value = entity.Customer.GetSalt();
                cmd.Parameters["cityId"].Value = entity.Customer.CityId;
                cmd.Parameters["street"].Value = entity.Customer.Street;
                cmd.Parameters["streetNumber"].Value = entity.Customer.StreetNumber;

                using (CancellationTokenSource tkn = new CancellationTokenSource(5000))
                {
                    //open connection
                    await con.OpenAsync(tkn.Token).ConfigureAwait(true);

                    //if unable to connect to the db, cancel the request
                    if (tkn.IsCancellationRequested)
                        return null;
                }

                //execute the query
                res = await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
            }
            //TODO: useful logging
            catch (Exception)
            {
                throw;
            }
            finally
            {
                cmd.Dispose();
                await con.CloseAsync().ConfigureAwait(false);
            }

            if (res != 0)
                //return the customer object if successfull
                return entity.Customer;
            else
                //return null if insert was unsuccessfull
                return null;
        }

        public async Task InsertAsync(params CustomerParams[] entities)
        {
            //check parameter validity
            if (entities.Length == 0 || entities == null)
                throw new ArgumentNullException(nameof(entities));

            //initialize the command
            MySqlCommand cmd = new MySqlCommand("CreateCustomer", con);

            try
            {
                //declare command type
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                //add parameters
                cmd.Parameters.AddRange(
                    new MySqlParameter[] {
                        new MySqlParameter("email", MySqlDbType.VarChar),
                        new MySqlParameter("fname", MySqlDbType.VarChar),
                        new MySqlParameter("lname", MySqlDbType.VarChar),
                        new MySqlParameter("pass", MySqlDbType.Binary),
                        new MySqlParameter("salt", MySqlDbType.Binary),
                        new MySqlParameter("cityId", MySqlDbType.Int32),
                        new MySqlParameter("street", MySqlDbType.VarChar),
                        new MySqlParameter("streetNumber", MySqlDbType.VarChar)
                    });

                for (int i = 0; i < entities.Length; i++)
                {
                    if (entities[i] == null)
                        continue;

                    //add paramter values
                    cmd.Parameters["email"].Value = entities[i].Customer.Email;
                    cmd.Parameters["fname"].Value = entities[i].Customer.FirstName;
                    cmd.Parameters["lname"].Value = entities[i].Customer.LastName;
                    cmd.Parameters["pass"].Value = entities[i].Customer.GetPassword();
                    cmd.Parameters["salt"].Value = entities[i].Customer.GetSalt();
                    cmd.Parameters["cityId"].Value = entities[i].Customer.CityId;
                    cmd.Parameters["street"].Value = entities[i].Customer.Street;
                    cmd.Parameters["streetNumber"].Value = entities[i].Customer.StreetNumber;

                    using (CancellationTokenSource tkn = new CancellationTokenSource(5000))
                    {
                        //open connection
                        await con.OpenAsync(tkn.Token).ConfigureAwait(true);

                        //if unable to connect to the db, cancel the request
                        if (tkn.IsCancellationRequested)
                            return;
                    }

                    //execute the query
                    await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
                }
            }
            //TODO: useful logging
            catch (Exception)
            {
                throw;
            }
            finally
            {
                cmd.Dispose();
                await con.CloseAsync().ConfigureAwait(false);
            }
        }

        public async Task InsertAsync(IEnumerable<CustomerParams> entities, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            await InsertAsync(entities.ToArray()).ConfigureAwait(false);
        }

        public Customer Update(CustomerParams entity)
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

        public Task<Customer> UpdateAsync(CustomerParams entity)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(params CustomerParams[] entities)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(IEnumerable<CustomerParams> entities)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Customer>> FromSqlAsync(string sql, params object[] parameters)
        {
            throw new NotImplementedException();
        }
    }
}
