using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace MinimalAPI.Infrastructure.Repository;

public class ConnectionManager : IConnectionManager
{
    private readonly IDbConnection _conn;

    public ConnectionManager(IConfiguration config)
    {
        string connString = $"Data Source=(DESCRIPTION = (ADDRESS = (PROTOCOL = TCP)(HOST = {config["Database:Connection:Host"]})(PORT = {config["Database:Connection:Port"]}))(CONNECT_DATA = (SERVER = DEDICATED)(SERVICE_NAME = {config["Database:Connection:ServiceName"]})));" +
                            $"User Id = { config["Database:Credentials:UserId"] }; Password = { config["Database:Credentials:Password"] };" + 
                            $"Min Pool Size = {config["Database:Pool:MinPoolSize"]};" +
                            $"Connection Lifetime = {config["Database:Pool:ConnectionLifetime"]};" +
                            $"Connection Timeout = {config["Database:Pool:ConnectionTimeout"]};" +
                            $"Incr Pool Size = {config["Database:Pool:IncrPoolSize"]};" +
                            $"Decr Pool Size = {config["Database:Pool:IncrPoolSize"]};";

        Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
        _conn = new OracleConnection(connString);
    }

    public IDbConnection GetConnection()
    {
        if (_conn.State == ConnectionState.Closed || _conn.State == ConnectionState.Broken)
        {
            _conn.Open();
        }

        return _conn;

    }
}
