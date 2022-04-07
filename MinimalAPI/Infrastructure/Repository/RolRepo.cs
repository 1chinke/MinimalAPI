using Dapper;
using MinimalAPI.Models;
using System.Data;

namespace MinimalAPI.Infrastructure.Database;

public class RolRepo : IRolRepo
{
    private readonly IDbConnection _conn;

    public RolRepo(IConnectionManager connectionManager)
    {
        _conn = connectionManager.GetConnection();
    }

    public async Task<int> Delete(string username)
    {
        string query = "Delete From rol where username = :username";
        var parameters = new { username };

        return await _conn.ExecuteAsync(query, param: parameters);
    }

    public async Task<IEnumerable<Rol>> GetAll()
    {
        string query = "Select * from rol order by username";
        return await _conn.QueryAsync<Rol>(query);
    }

    public async Task<int> Insert(Rol model)
    {
        string query = "Insert Into rol values(:id ,:adi, :aciklama, :admin)";

        var parameters = new { model.Id, model.Adi, model.Aciklama, model.Admin };

        return await _conn.ExecuteAsync(query, param: parameters);
    }

    public async Task<int> Update(int id, Rol model)
    {
        string query = @"Update rol set
            id = :id,
            adi = :adi,
            aciklama = :aciklama,
            admin = :admin
            Where id = :oldId";
        var parameters = new { model.Id, model.Adi, model.Aciklama, model.Admin, id };

        return await _conn.ExecuteAsync(query, param: parameters);
    }

}
