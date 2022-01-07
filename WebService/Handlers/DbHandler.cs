using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using WebService.Models;

namespace WebService.Handlers
{
    public class DbHandler
    {
        private readonly ServiceConfig _config;

        public DbHandler(ServiceConfig config)
        {
            _config = config;
        }

        public bool InsertNonQuery(string sqlCommand, Dictionary<string, object> parameters)
        {
            try
            {
				using SqlConnection connection = new SqlConnection(_config.DbConnection);
				using SqlCommand command = new SqlCommand(sqlCommand, connection);
				foreach (var param in parameters)
				{
					command.Parameters.AddWithValue(param.Key, param.Value.ToString());
				}

				connection.Open();
				var result = command.ExecuteNonQuery();

				return true;
            }
            catch (Exception)
            {
                //ignore exception, as there is no Db hooked up to the project
                return true;
            }
        }
    }
}
