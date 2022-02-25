using System;
using System.Data;
using MySql.Data.MySqlClient;
using System.Threading.Tasks;

namespace SZY.Platform.WebApi.Data
{
    public abstract class BaseRepo
    {
        private readonly string _ConnectionString;

        protected BaseRepo(DapperOptions options)
        {
            _ConnectionString = options.ConnectionString;
        }

        protected async Task<T> WithConnection<T>(Func<IDbConnection, Task<T>> getData)
        {
            try
            {
                using (var connection = new MySqlConnection(_ConnectionString))
                {
                    await connection.OpenAsync();
                    return await getData(connection);
                }
            }
            catch (TimeoutException ex)
            {
                throw new Exception(ex.ToString());
            }
            catch (MySqlException ex)
            {

                throw new Exception(ex.ToString());
            }
        }
    }
}
