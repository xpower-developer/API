using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XPowerAPI.Logging
{
    public class DbLogger : ILogger
    {
		MySqlConnection con = new MySqlConnection("");
		MySqlCommand cmd;

		public DbLogger()
		{
			cmd = new MySqlCommand("INSERT INTO Log VALUES ($msg);", con);
		}

        public async Task Log(string msg)
        {
			try
			{
				await con.OpenAsync();
				cmd.Parameters.Add("$msg", MySqlDbType.VarChar);
				cmd.Parameters["$msg"].Value = msg;
				int res = await cmd.ExecuteNonQueryAsync();
			}
			catch (Exception)
			{
				throw;
			}
			finally {
				await con.CloseAsync();
				cmd.Parameters.Clear();
			}

			return;
        }
    }
}
