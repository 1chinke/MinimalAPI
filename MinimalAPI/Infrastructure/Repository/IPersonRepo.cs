using MinimalAPI.Models;
using System.Data;

namespace MinimalAPI.Infrastructure.Database;

public interface IPersonRepo
{
    Task<int> Delete(string id);
    Task<IEnumerable<Person>> GetAll();
    Task<Person> GetById(string id);
    Task<Person> GetByFirstNameAndLastName(string firstName, string lastName);
    Task<int> Insert(Person model);
    Task<int> Update(Person model);


}
