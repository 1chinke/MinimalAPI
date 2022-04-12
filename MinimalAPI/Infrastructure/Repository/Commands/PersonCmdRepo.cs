using Dapper;
using MinimalAPI.Infrastructure.Database;
using MinimalAPI.Models;
using System.Data;


namespace MinimalAPI.Infrastructure.Repository.Commands;

public class PersonCmdRepo : IPersonCmdRepo
{
    private readonly IDbConnection _conn;

    public PersonCmdRepo(IConnectionManager connectionManager)
    {
        _conn = connectionManager.GetConnection();
    }

    
    public async Task<int> Insert(Person model)
    {
        string query = "Insert Into person values(:id, :firstName, :lastName)";

        var parameters = new { id = model.Id, firstName = model.FirstName, lastName = model.LastName };

        return await _conn.ExecuteAsync(query, param: parameters);
    }

    public async Task<int> Delete(string id)
    {
        string query = "Delete From person where id = :id";
        var parameters = new { id };

        return await _conn.ExecuteAsync(query, param: parameters);
    }

    public async Task<int> Update(Person model)
    {
        string query = @"Update person set 
            first_name = :firstName, 
            last_name = :lastName 
            Where id = :id";
        var parameters = new { id = model.Id, firstName = model.FirstName, lastName = model.LastName};


        return await _conn.ExecuteAsync(query, param: parameters);
    }

}
