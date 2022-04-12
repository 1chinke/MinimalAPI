using Dapper;
using MinimalAPI.Infrastructure.Database;
using MinimalAPI.Models;
using System.Data;

namespace MinimalAPI.Infrastructure.Repository.Queries;

public class RolQryRepo : IRolQryRepo
{
    private readonly IDbConnection _conn;

    public RolQryRepo(IConnectionManager connectionManager)
    {
        _conn = connectionManager.GetConnection();
    }

   
    public async Task<IEnumerable<Rol>> GetAll()
    {
        string query = "Select * from rol order by username";
        return await _conn.QueryAsync<Rol>(query);
    }
  
}
