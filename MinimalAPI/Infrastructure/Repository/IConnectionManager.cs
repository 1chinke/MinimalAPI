using System.Data;

namespace MinimalAPI.Infrastructure.Repository;

public interface IConnectionManager
{
    IDbConnection GetConnection();
}