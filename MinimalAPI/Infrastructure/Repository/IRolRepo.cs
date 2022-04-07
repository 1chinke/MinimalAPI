using MinimalAPI.Models;
using System.Data;

namespace MinimalAPI.Infrastructure.Database;

public interface IRolRepo
{
    Task<int> Delete(string username);
    Task<IEnumerable<Rol>> GetAll();
    Task<int> Insert(Rol model);
    Task<int> Update(int id, Rol model);

}