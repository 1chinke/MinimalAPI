using System.Text.Json.Serialization;

namespace MinimalAPI.Models;

public class HavaTahmini
{
    [JsonPropertyName("consolidated_weather")]
    public List<GunlukTahmin> ConsolidatedWeather { get; set; }

    [JsonPropertyName("time")]
    public DateTime Time { get; set; }

    [JsonPropertyName("sun_rise")]
    public DateTime SunRise { get; set; }

    [JsonPropertyName("sun_set")]
    public DateTime SunSet { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; }

    [JsonPropertyName("timezone")]
    public string Timezone { get; set; }
}



