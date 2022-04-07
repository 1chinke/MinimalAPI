using System.Data;

namespace MinimalAPI.Infrastructure.Database;

public interface IConnectionManager
{
    IDbConnection GetConnection();
}