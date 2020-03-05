using Neo4jClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using XPowerAPI.Models;
using MySql.Data.MySqlClient;
using System.Text;
using XPowerAPI.Repository.Collections;

namespace XPowerAPI.Repository
{
    /// <summary>
    /// A device repository capable of finding devices based on their uuid
    /// </summary>
    public class DeviceRepository : IRepository<Device>
    {
        GraphClient client = new GraphClient(new Uri(""), "user", "pass");
        MySqlConnection con = new MySqlConnection("");

        public int Count()
        {
            int res = 0;
            /*long res = client.Cypher
                .Match("(n:Device)")
                .Return((n) => new { num = n.Count() }).Results.FirstOrDefault().num;*/
            //return res == 0 ? 0 : (int)res;

            try
            {
                MySqlCommand cmd = new MySqlCommand("select count(*) as num from Device;");
                con.Open();
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    res = (int)reader["num"];
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                con.Close();
            }

            return res;
        }

        public void Delete(object id)
        {
            try
            {
                client.Cypher
                    .Match("(n:Device)")
                    .Where((Device n) => n.uuid == (string)id)
                    .DetachDelete("n")
                    .ExecuteWithoutResults();

                con.Open();
                new MySqlCommand("delete from Device where DeviceUUID = " + (string)id + ";").ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                con.Close();
            }
        }

        public void Delete(Device entity)
        {
            try
            {
                client.Cypher
                    .Match("(n:Device)")
                    .Where((Device n) => n.uuid == entity.uuid)
                    .DetachDelete("n")
                    .ExecuteWithoutResults();

                con.Open();
                new MySqlCommand("delete from Device where DeviceUUID = " + entity.uuid + ";").ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                con.Close();
            }
        }

        public void Delete(params Device[] entities)
        {
            try
            {
                string[] ids = entities.Select(e => e.uuid).ToArray();
                client.Cypher
                    .Match("(n:Device)")
                    .Where("n.uuid IN {ids}")
                    .WithParams(new { ids })
                    .DetachDelete("n")
                    .ExecuteWithoutResults();

                StringBuilder builder = new StringBuilder("delete from Device where ");
                for (int i = 0; i < ids.Length; i++)
                {
                    builder.Append("DeviceUUID = " + ids[i] + " or ");
                }

                con.Open();
                new MySqlCommand(builder.ToString()[..^3] + ';').ExecuteNonQuery();

            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                con.Close();
            }
        }

        public void Delete(IEnumerable<Device> entities)
        {
            Delete(entities.ToArray());
        }

        public bool Exists(object key)
        {
            try
            {
                con.Open();
                return new MySqlCommand("select * from Device where uuid = " + (string)key + " limit 1;").ExecuteNonQuery() != 0;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                con.Close();
            }
        }

        public Task<bool> ExistsAsync(object key)
        {
            throw new NotImplementedException();
        }

        public Device Find(params object[] keyValues)
        {
            Device d = new Device();

            try
            {
                MySqlCommand cmd = new MySqlCommand("select * from Device where uuid = " + keyValues[0] + " limit 1;");
                con.Open();
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    d.name = reader.GetString(reader.GetOrdinal("name"));
                    d.uuid = reader.GetString(reader.GetOrdinal("uuid"));
                    d.state = reader.GetBoolean(reader.GetOrdinal("state"));
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                con.Close();
            }

            return string.IsNullOrEmpty(d.uuid) ? null : d;
        }

        public Task<Device> FindAsync(params object[] keyValues)
        {
            throw new NotImplementedException();
        }

        public Task<Device> FindAsync(object[] keyValues, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Device> FromSql(string sql, params object[] parameters)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Device> GetAll()
        {
            IList<Device> devices;

            try
            {
                MySqlCommand cmd = new MySqlCommand("select DeviceUUID, Name, State from Device");
                con.Open();
                MySqlDataReader reader = cmd.ExecuteReader();
                int count = Count();
                devices = new List<Device>(count);
                for (int i = 0; i < count; i++)
                {
                    Device device = new Device()
                    {
                        name = reader.GetString(reader.GetOrdinal("Name")),
                        uuid = reader.GetString(reader.GetOrdinal("DeviceUUID")),
                        state = reader.GetBoolean(reader.GetOrdinal("State")),
                    };

                    devices[i] = device;
                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                con.Close();
            }

            return devices;
        }

        public Task<IEnumerable<Device>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public IPagedList<Device> GetPagedList(object[] keyValues, int pageIndex = 0, int pageSize = 20)
        {
            throw new NotImplementedException();
        }

        public Task<IPagedList<Device>> GetPagedListAsync(object[] keyValues, int pageIndex = 0, int pageSize = 20, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Device Insert(Device entity)
        {
            try
            {
                con.Open();
                int res = new MySqlCommand("insert into Device values (" + entity.uuid + ',' + entity.name + ',' + entity.state + ");").ExecuteNonQuery();
                if (res != 0)
                    return entity;
                else
                    return null;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                con.Close();
            }
        }

        public void Insert(params Device[] entities)
        {
            try
            {
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < entities.Length; i++)
                {
                    builder.Append($"({entities[i].uuid}, {entities[i].name}, {(entities[i].state ? '1' : '0')}),");
                }

                con.Open();
                new MySqlCommand("insert into Device values " + builder.ToString()[..^1] + ';').ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                con.Close();
            }
        }

        public void Insert(IEnumerable<Device> entities)
        {
            Insert(entities.ToArray());
        }

        public Task InsertAsync(params Device[] entities)
        {
            throw new NotImplementedException();
        }

        public Task InsertAsync(IEnumerable<Device> entities, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public void Update(Device entity)
        {
            try
            {
                con.Open();
                new MySqlCommand($"update Device set Name = {entity.name}, State = {entity.state} where DeviceUUID = {entity.uuid};").ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                con.Close();
            }
        }

        public void Update(params Device[] entities)
        {
            try
            {
                con.Open();
                for (int i = 0; i < entities.Length; i++)
                {
                    new MySqlCommand($"update Device set Name = {entities[i].name}, State = {entities[i].state} where DeviceUUID = {entities[i].uuid};").ExecuteNonQuery();
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                con.Close();
            }
        }

        public void Update(IEnumerable<Device> entities)
        {
            Update(entities.ToArray());
        }
    }
}
