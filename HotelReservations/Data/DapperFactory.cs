using Microsoft.Data.SqlClient;
using System.Data;
namespace HotelReservations.Data
{
    public sealed class DapperFactory(IConfiguration config)
    {
        private IDbConnection? _connection;
        public IDbConnection CreateConnection()
        {

            if (_connection == null)
            {
                _connection = new SqlConnection(config.GetConnectionString("DefaultConnection"));
            }
            return _connection;
        }
    }
}
