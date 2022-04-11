using Dapper;
using MinimalAPI.Models;
using System.Data;


namespace MinimalAPI.Infrastructure.Database;

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

    public async Task<Person> GetById(string id)
    {
        string query = "Select * from person where id = :id";

        var parameters = new { id };

        return await _conn.QueryFirstOrDefaultAsync<Person>(query, param: parameters);
    }

    public async Task<Person> GetByFirstNameAndLastName(string firstName, string lastName)
    {
        string query = @"Select * from person 
                         Where first_name = :firstName And
                               last_name = :lastName";

        var parameters = new { firstName, lastName};

        return await _conn.QueryFirstOrDefaultAsync<Person>(query, param: parameters);

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
