using MinimalAPI.Responses;

namespace MinimalAPI.Infrastructure.Integration;

public interface IHavaTahminiSvc
{
    Task<GenericResponse> Get(string applicableDate);
}