using Dapper;
using MinimalAPI.Infrastructure.Database;
using MinimalAPI.Models;
using System.Data;

namespace MinimalAPI.Infrastructure.Repository.Queries;

public class KullaniciQryRepo : IKullaniciQryRepo
{
    private readonly IDbConnection _conn;

    public KullaniciQryRepo(IConnectionManager connectionManager)
    {
        _conn = connectionManager.GetConnection();
    }

    

    public async Task<IEnumerable<Kullanici>> GetAll()
    {
        string query = "Select * from kullanici order by username";
        return await _conn.QueryAsync<Kullanici>(query);
    }

    public async Task<Kullanici> GetByUsername(string username)
    {
        string query = "Select * from kullanici where username = :username";

        var parameters = new { username };

        return await _conn.QueryFirstOrDefaultAsync<Kullanici>(query, param: parameters);
    }

    public async Task<Kullanici> Login(string username, string password)
    {
        string query = @"Select * from kullanici 
                         Where username = :username and 
                               password = :password";

        var parameters = new { username, password };

        return await _conn.QueryFirstOrDefaultAsync<Kullanici>(query, param: parameters);
    }
   
    public async Task<IEnumerable<Rol>> GetKullaniciRoles(string username)
    {
        string query = @"Select r.*
                         From kullanici k, rol r, kullanici_rol kr
                         Where kr.username = k.username And
                               kr.rol = r.id And
                               k.username = :username
                         Order By r.id ";

        var parameters = new { username };

        return await _conn.QueryAsync<Rol>(query, param: parameters);
    }
}
