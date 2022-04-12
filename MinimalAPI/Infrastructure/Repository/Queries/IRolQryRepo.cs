using MinimalAPI.Models;

namespace MinimalAPI.Infrastructure.Repository.Queries;

public interface IRolQryRepo
{
    Task<IEnumerable<Rol>> GetAll();
}