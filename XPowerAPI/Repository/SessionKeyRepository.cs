using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using XPowerAPI.Models;
using XPowerAPI.Repository.Collections;
using XPowerAPI.Logging;
using MySql.Data.MySqlClient;
using Microsoft.AspNetCore.Mvc;
using XPowerAPI.Models.Params;
using System.Data.Common;

namespace XPowerAPI.Repository
{
    public class SessionKeyRepository : IRepository<SessionKey, SessionKeyParams>, IDisposable
    {
        bool isDisposed = false;

        MySqlConnection con;
        ILogger logger;

        public SessionKeyRepository(
            [FromServices]MySqlConnection con,
            [FromServices]ILogger logger)
        {
            this.logger = logger;
            this.con = con;
        }

        ~SessionKeyRepository()
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

        public void Delete(SessionKeyParams entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(params SessionKeyParams[] entities)
        {
            throw new NotImplementedException();
        }

        public void Delete(IEnumerable<SessionKeyParams> entities)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(object id)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(SessionKeyParams entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(params SessionKeyParams[] entities)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(IEnumerable<SessionKeyParams> entities)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (isDisposed)
                return;

            if (disposing)
            {
                con.Dispose();
                logger.Dispose();
            }
        }

        public bool Exists(object key)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ExistsAsync(object key)
        {
            throw new NotImplementedException();
        }

        public SessionKey Find(params object[] keyValues)
        {
            if (keyValues[0] == null)
                throw new ArgumentNullException(nameof(keyValues));

            string val = keyValues[0].ToString();

            if (string.IsNullOrEmpty(val))
                throw new ArgumentNullException(nameof(keyValues));

            SessionKey key = null;
            MySqlCommand cmd = new MySqlCommand();

            //check whether the value is an email
            if (val.Contains('@', StringComparison.OrdinalIgnoreCase))
            {
                cmd = new MySqlCommand("GetFullSessionKeyByEmail", con) { CommandType = System.Data.CommandType.StoredProcedure };
                cmd.Parameters.Add("email", MySqlDbType.VarChar);
                cmd.Parameters["email"].Value = val;
                cmd.Parameters["email"].Direction = System.Data.ParameterDirection.Input;
            }
            //or a uuid
            else if (val.Length == 36)
            {
                cmd = new MySqlCommand("GetFullSessionKeyById", con) { CommandType = System.Data.CommandType.StoredProcedure };
                cmd.Parameters.Add("keyid", MySqlDbType.VarChar);
                cmd.Parameters["keyid"].Value = val;
                cmd.Parameters["keyid"].Direction = System.Data.ParameterDirection.Input;

            }

            try
            {
                con.Open();
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    key = new SessionKey(
                        reader.GetString(
                            reader.GetOrdinal("SessionKeyId")),
                        reader.GetString(
                            reader.GetOrdinal("ApiKeyId")),
                        reader.GetDateTime(
                            reader.GetOrdinal("ExpirationDate")));
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

            return key;
        }

        public async Task<SessionKey> FindAsync(params object[] keyValues)
        {
            if (keyValues[0] == null)
                throw new ArgumentNullException(nameof(keyValues));

            string val = keyValues[0].ToString();

            if (string.IsNullOrEmpty(val))
                throw new ArgumentNullException(nameof(keyValues));

            SessionKey key = null;
            MySqlCommand cmd = new MySqlCommand();

            //check whether the value is an email
            if (val.Contains('@', StringComparison.OrdinalIgnoreCase))
            {
                cmd = new MySqlCommand("GetFullSessionKeyByEmail", con) { CommandType = System.Data.CommandType.StoredProcedure };
                cmd.Parameters.Add("email", MySqlDbType.VarChar);
                cmd.Parameters["email"].Value = val;
                cmd.Parameters["email"].Direction = System.Data.ParameterDirection.Input;
            }
            //or a uuid
            else if (val.Length == 36)
            {
                cmd = new MySqlCommand("GetFullSessionKeyById", con) { CommandType = System.Data.CommandType.StoredProcedure };
                cmd.Parameters.Add("keyid", MySqlDbType.VarChar);
                cmd.Parameters["keyid"].Value = val;
                cmd.Parameters["keyid"].Direction = System.Data.ParameterDirection.Input;

            }

            try
            {
                con.Open();
                DbDataReader reader = await cmd.ExecuteReaderAsync().ConfigureAwait(false);

                while (reader.Read())
                {
                    key = new SessionKey(
                        reader.GetString(
                            reader.GetOrdinal("SessionKeyId")),
                        reader.GetString(
                            reader.GetOrdinal("ApiKeyId")),
                        reader.GetDateTime(
                            reader.GetOrdinal("ExpirationDate")));
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

            return key;
        }

        public async Task<SessionKey> FindAsync(object[] keyValues, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            return await FindAsync(keyValues).ConfigureAwait(false);
        }

        public IEnumerable<SessionKey> FromSql(string sql, params object[] parameters)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<SessionKey> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<SessionKey>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public IPagedList<SessionKey> GetPagedList(object[] keyValues, int pageNumber = 0, int pageSize = 20)
        {
            throw new NotImplementedException();
        }

        public Task<IPagedList<SessionKey>> GetPagedListAsync(object[] keyValues, int pageNumber = 0, int pageSize = 20, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        //fix out params
        public SessionKey Insert(SessionKeyParams entity)
        {
            //check that the email is non null
            if (entity == null || entity.Email.Length == 0)
                throw new ArgumentNullException(nameof(entity));

            //create command
            MySqlCommand cmd = new MySqlCommand("CreateSessionKey", con)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };

            //add parameters
            cmd.Parameters.Add("email", MySqlDbType.VarChar);
            cmd.Parameters.Add("sessionkey", MySqlDbType.VarChar);
            cmd.Parameters.Add("expirationdate", MySqlDbType.DateTime);

            cmd.Parameters["email"].Value = entity.Email;
            cmd.Parameters["email"].Direction = System.Data.ParameterDirection.Input;
            cmd.Parameters["sessionkey"].Direction = System.Data.ParameterDirection.Output;
            cmd.Parameters["expirationdate"].Direction = System.Data.ParameterDirection.Output;

            //create temporary object
            SessionKey ses = null;
            try
            {
                //create key
                con.Open();
                int res = cmd.ExecuteNonQuery();

                if (res != 0)
                {
                    //fill object if any rows were affected
                    ses = new SessionKey(
                        (string)cmd.Parameters["sessionkey"].Value,
                        (DateTime)cmd.Parameters["expirationdate"].Value);
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

            return ses;
        }

        //fix out params
        public void Insert(params SessionKeyParams[] entities)
        {
            //check that the email is non null
            if (entities == null || entities.Length == 0)
                throw new ArgumentNullException(nameof(entities));

            //create command
            MySqlCommand cmd = new MySqlCommand("CreateSessionKey", con)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };

            //add parameters
            cmd.Parameters.Add("email", MySqlDbType.VarChar);
            cmd.Parameters.Add("sessionkey", MySqlDbType.VarChar);
            cmd.Parameters.Add("expirationdate", MySqlDbType.DateTime);
            cmd.Parameters["sessionkey"].Direction = System.Data.ParameterDirection.Output;
            cmd.Parameters["expirationdate"].Direction = System.Data.ParameterDirection.Output;

            try
            {
                //open connection
                con.Open();
                for (int i = 0; i < entities.Length; i++)
                {
                    //check each entity email
                    if (entities[i].Email.Length == 0)
                        throw new ArgumentNullException(nameof(entities));

                    //change parameters
                    cmd.Parameters["email"].Value = entities[i].Email;
                    cmd.Parameters["email"].Direction = System.Data.ParameterDirection.Input;
                    //execute query
                    cmd.ExecuteNonQuery();
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
        }

        //fix out params
        public void Insert(IEnumerable<SessionKeyParams> entities)
        {
            Insert(entities);
        }

        public async Task<SessionKey> InsertAsync(SessionKeyParams entity)
        {
            //check that the email is non null
            if (entity == null || entity.Email.Length == 0)
                throw new ArgumentNullException(nameof(entity));

            //create command
            MySqlCommand cmd = new MySqlCommand("CreateSessionKey", con)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };

            //add parameters
            cmd.Parameters.Add("email", MySqlDbType.VarChar);
            cmd.Parameters.Add("sessionkey", MySqlDbType.VarChar);
            cmd.Parameters.Add("expirationdate", MySqlDbType.DateTime);

            cmd.Parameters["email"].Value = entity.Email;
            cmd.Parameters["email"].Direction = System.Data.ParameterDirection.Input;
            cmd.Parameters["sessionkey"].Direction = System.Data.ParameterDirection.Output;
            cmd.Parameters["expirationdate"].Direction = System.Data.ParameterDirection.Output;

            //create temporary object
            SessionKey ses = null;
            try
            {
                //create key

                using (CancellationTokenSource tkn = new CancellationTokenSource(5000))
                {
                    //open connection
                    await con.OpenAsync(tkn.Token).ConfigureAwait(true);

                    //if unable to connect to the db, cancel the request
                    if (tkn.IsCancellationRequested)
                        return null;
                }

                DbDataReader reader = await cmd.ExecuteReaderAsync().ConfigureAwait(false);

                while (reader.Read())
                {
                    ses = new SessionKey(
                        reader.GetString(
                            reader.GetOrdinal("SessionKeyId")),
                        reader.GetDateTime(
                            reader.GetOrdinal("ExpirationDate")));
                }

                //await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);

                /*if (cmd.Parameters["sessionkey"] != null) throw new ArgumentNullException(cmd.Parameters["sessionkey"].ToString());
                if (cmd.Parameters["expirationdate"] != null) throw new ArgumentNullException(cmd.Parameters["expirationdate"].ToString());
                if (cmd.Parameters["sessionkey"] != null)
                {
                    //fill object if any rows were affected
                    ses = new SessionKey(
                        (string)cmd.Parameters["sessionkey"].Value,
                        (DateTime)cmd.Parameters["expirationdate"].Value);
                }*/
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

            return ses;
        }

        //fix out params
        public async Task InsertAsync(params SessionKeyParams[] entities)
        {
            //check that the email is non null
            if (entities == null || entities.Length == 0)
                throw new ArgumentNullException(nameof(entities));

            //create command
            MySqlCommand cmd = new MySqlCommand("CreateSessionKey", con)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };

            //add parameters
            cmd.Parameters.Add("email", MySqlDbType.VarChar);
            cmd.Parameters.Add("sessionkey", MySqlDbType.VarChar);
            cmd.Parameters.Add("expirationdate", MySqlDbType.DateTime);
            cmd.Parameters["sessionkey"].Direction = System.Data.ParameterDirection.Output;
            cmd.Parameters["expirationdate"].Direction = System.Data.ParameterDirection.Output;

            try
            {
                using (CancellationTokenSource tkn = new CancellationTokenSource(5000))
                {
                    //open connection
                    await con.OpenAsync(tkn.Token).ConfigureAwait(true);

                    //if unable to connect to the db, cancel the request
                    if (tkn.IsCancellationRequested)
                        return;
                }

                for (int i = 0; i < entities.Length; i++)
                {
                    //check each entity email
                    if (entities[i].Email.Length == 0)
                        throw new ArgumentNullException(nameof(entities));

                    //change parameters
                    cmd.Parameters["email"].Value = entities[i].Email;
                    cmd.Parameters["email"].Direction = System.Data.ParameterDirection.Input;
                    //execute query
                    await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
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
        }

        //fix out params
        public async Task InsertAsync(IEnumerable<SessionKeyParams> entities, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            await InsertAsync(entities).ConfigureAwait(false);
        }

        public SessionKey Update(SessionKeyParams entity)
        {
            throw new NotImplementedException();
        }

        public void Update(params SessionKeyParams[] entities)
        {
            throw new NotImplementedException();
        }

        public void Update(IEnumerable<SessionKeyParams> entities)
        {
            throw new NotImplementedException();
        }

        public Task<SessionKey> UpdateAsync(SessionKeyParams entity)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(params SessionKeyParams[] entities)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(IEnumerable<SessionKeyParams> entities)
        {
            throw new NotImplementedException();
        }
    }
}
