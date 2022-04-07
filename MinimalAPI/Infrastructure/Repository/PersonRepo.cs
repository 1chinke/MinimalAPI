using Dapper;
using MinimalAPI.Models;
using System.Data;


namespace MinimalAPI.Infrastructure.Repository;

public class PersonRepo : IPersonRepo
{
    private readonly IDbConnection _conn;

    public PersonRepo(IConnectionManager connectionManager)
    {
        _conn = connectionManager.GetConnection();
    }


    public async Task<IEnumerable<Person>> GetAll()
    {
        string query = "Select * from person order by id";
        return await _conn.QueryAsync<Person>(query);
    }

    public async Task<Person> GetById(int id)
    {
        string query = "Select * from person where id = :id";

        var parameters = new { id };

        return await _conn.QueryFirstOrDefaultAsync<Person>(query, param: parameters);
    }

    public async Task<int> Insert(int id, string firstName, string lastName)
    {
        string query = "Insert Into person values(:id, :firstName, :lastName)";

        var parameters = new { id, firstName, lastName };

        return await _conn.ExecuteAsync(query, param: parameters);
    }

    public async Task<int> Delete(int id)
    {
        string query = "Delete From person where id = :id";
        var parameters = new { id };

        return await _conn.ExecuteAsync(query, param: parameters);
    }

    public async Task<int> Update(int id, Person model)
    {
        string query = @"Update person set 
            id = :id,
            first_name = :firstName, 
            last_name = :lastName 
            Where id = :oldId";
        var parameters = new { id = model.Id, firstName = model.FirstName, lastName = model.LastName, oldId = id };


        return await _conn.ExecuteAsync(query, param: parameters);
    }

}
