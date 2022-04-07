using MediatR;
using Microsoft.Extensions.Caching.Memory;
using MinimalAPI.Infrastructure.Integration;

namespace MinimalAPI.Mediatr.Behaviors;

public class CachingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>, ICacheable
{
    private readonly IMemoryCache _cache;

    public CachingBehavior(IMemoryCache cache)
    {
        _cache = cache;
    }


    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {

        if (_cache.TryGetValue(request.CacheKey, out TResponse response))
        {
            return response;
        }

        response = await next();
        _cache.Set(request.CacheKey, response);
        return response;
    }
}
