using MinimalAPI.Models;

namespace MinimalAPI.Infrastructure.Repository.Queries;

public interface IPersonQryRepo
{
    Task<IEnumerable<Person>> GetAll();
    Task<Person> GetById(string id);
    Task<Person> GetByFirstNameAndLastName(string firstName, string lastName);
}
