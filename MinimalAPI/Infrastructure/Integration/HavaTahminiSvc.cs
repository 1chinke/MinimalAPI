using MinimalAPI.Models;
using MinimalAPI.Responses;

namespace MinimalAPI.Infrastructure.Integration;

public class HavaTahminiSvc : IHavaTahminiSvc
{
    private readonly IHttpClientFactory _httpClientFactory;

    public HavaTahminiSvc(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<GenericResponse> Get(String applicableDate)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "https://www.metaweather.com/api/location/2343732/");

        var client = _httpClientFactory.CreateClient();

        var response = await client.SendAsync(request);

        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<HavaTahmini>();

            result.ConsolidatedWeather.RemoveAll(item => item.ApplicableDate != applicableDate);

            return new GenericResponse(result);
        }
        else
        {
            return new GenericResponse(StatusCode: response.StatusCode, Error: response.ReasonPhrase);
        }
    }

}
