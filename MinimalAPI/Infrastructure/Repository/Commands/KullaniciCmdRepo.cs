using Dapper;
using MinimalAPI.Infrastructure.Database;
using MinimalAPI.Models;
using System.Data;

namespace MinimalAPI.Infrastructure.Repository.Commands;

public class KullaniciCmdRepo : IKullaniciCmdRepo
{
    private readonly IDbConnection _conn;

    public KullaniciCmdRepo(IConnectionManager connectionManager)
    {
        _conn = connectionManager.GetConnection();
    }

    public async Task<int> Delete(string username)
    {
        string query = "Delete From kullanici where username = :username";
        var parameters = new { username };

        return await _conn.ExecuteAsync(query, param: parameters);
    }

    public async Task<int> Insert(Kullanici model)
    {
        string query = "Insert Into kullanici values(:username ,:password, :emailAddress, :firstName, :lastName, :role)";

        var parameters = new { model.Username,model.Password, model.EmailAddress, model.FirstName, model.LastName, model.Role };

        return await _conn.ExecuteAsync(query, param: parameters);
    }

    public async Task<int> Update(string username, Kullanici model)
    {
        string query = @"Update kullanici set
            username = :username,
            password = :password,
            email_address = :emailAddress,
            first_name = :firstName,
            last_name = :lastName,
            role = :role
            Where username = :oldUsername";
        var parameters = new { model.Username, model.Password, model.EmailAddress, model.FirstName, model.LastName, model.Role, username };


        return await _conn.ExecuteAsync(query, param: parameters);
    }

    public async Task<int> InsertKullaniciRol(string username, IEnumerable<Rol> roller)
    {
        string query = "Insert Into kullanici_rol(username, rol) values(:username, :rol)";

        int result = 0;

        foreach (var rol in roller)
        {
            var parameters = new { username, rol};
            result += await _conn.ExecuteAsync(query, param: parameters);
        }

        return result;
    }

}
