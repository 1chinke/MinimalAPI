using Dapper;
using MinimalAPI.Infrastructure.Database;
using MinimalAPI.Models;
using System.Data;


namespace MinimalAPI.Infrastructure.Repository.Queries;

public class PersonQryRepo : IPersonQryRepo
{
    private readonly IDbConnection _conn;

    public PersonQryRepo(IConnectionManager connectionManager)
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

   

}
