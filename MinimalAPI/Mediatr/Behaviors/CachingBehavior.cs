using MediatR;
using Microsoft.Extensions.Caching.Memory;
using MinimalAPI.Infrastructure.Integration;
using Microsoft.Extensions.Caching.Distributed;
using System.Text;
using System.Text.Json;


namespace MinimalAPI.Mediatr.Behaviors;

public class CachingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>, ICacheable
{
    //private readonly IMemoryCache _cache;
    private readonly IDistributedCache _cache;

    //public CachingBehavior(IMemoryCache cache)
    public CachingBehavior(IDistributedCache cache)
    {
        _cache = cache;
    }


    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {

        var cachedValue = await _cache.GetAsync(request.CacheKey, cancellationToken);
        
        TResponse response;

        if (cachedValue != null)
        {
            response = JsonSerializer.Deserialize<TResponse>(Encoding.UTF8.GetString(cachedValue));
        }
        else
        {
            response = await next();

            var options = new DistributedCacheEntryOptions()
                        .SetAbsoluteExpiration(DateTime.Now.AddMinutes(10))
                        .SetSlidingExpiration(TimeSpan.FromMinutes(1));
            await _cache.SetAsync(request.CacheKey, Encoding.UTF8.GetBytes(JsonSerializer.Serialize<TResponse>(response)), options);
        }
       
        return response;
    }
}
