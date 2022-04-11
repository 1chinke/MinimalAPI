using MediatR;
using MinimalAPI.Infrastructure.Integration;
using MinimalAPI.Responses;

namespace MinimalAPI.Mediatr.Queries.HavaTahminiQueries;

public record GetHavaTahminiQry(string ApplicableDate) : IRequest<GenericResponse>, ICacheable
{
    public string CacheKey => $"GetHavaTahmini-{ApplicableDate}";
}


