using Microsoft.Data.SqlClient;
//using Oracle.ManagedDataAccess.Client;
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
                //_connection = new OracleConnection(config.GetConnectionString("DefaultConnection"));
                _connection = new SqlConnection(config.GetConnectionString("DefaultConnection"));
            }
            return _connection;
        }
    }
}
