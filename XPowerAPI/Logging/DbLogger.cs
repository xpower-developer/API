using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace XPowerAPI.Logging
{
    public class DbLogger : ILogger, IDisposable
    {
        private bool isDisposed = false;
        MySqlConnection con;
        MySqlCommand cmd;

        public DbLogger([FromServices]MySqlConnection con)
        {
            this.con = con;
            cmd = new MySqlCommand("WriteLog", con) { CommandType = System.Data.CommandType.StoredProcedure };
            cmd.Parameters.Add("msg", MySqlDbType.VarChar);
        }

        ~DbLogger()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (isDisposed) return;

            if (disposing)
            {
                cmd.Dispose();
                con.Dispose();
            }
        }

        public void Log(string msg)
        {
            try
            {
                cmd.Parameters["msg"].Value = msg;
                con.Open();
                cmd.ExecuteNonQuery();
            }
            catch (DbException)
            {
                throw;
            }
            finally
            {
                con.Close();
            }
        }

        public async Task LogAsync(string msg)
        {
            try
            {
                cmd.Parameters["msg"].Value = msg;
                await con.OpenAsync().ConfigureAwait(false);
                await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
            }
            catch (DbException)
            {
                throw;
            }
            finally
            {
                await con.CloseAsync().ConfigureAwait(false);
            }
        }
    }
}
