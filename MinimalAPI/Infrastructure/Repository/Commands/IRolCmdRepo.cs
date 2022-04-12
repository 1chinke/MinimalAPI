using MinimalAPI.Models;

namespace MinimalAPI.Infrastructure.Repository.Commands;

public interface IRolCmdRepo
{
    Task<int> Delete(string username);
    Task<int> Insert(Rol model);
    Task<int> Update(int id, Rol model);

}