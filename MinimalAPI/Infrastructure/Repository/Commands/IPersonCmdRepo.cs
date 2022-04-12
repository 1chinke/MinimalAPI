using MinimalAPI.Models;

namespace MinimalAPI.Infrastructure.Repository.Commands;

public interface IPersonCmdRepo
{
    Task<int> Delete(string id);    
    Task<int> Insert(Person model);
    Task<int> Update(Person model);


}
