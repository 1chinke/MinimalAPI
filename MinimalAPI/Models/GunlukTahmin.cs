using System.Text.Json.Serialization;

namespace MinimalAPI.Models;

public class GunlukTahmin
{
    [JsonPropertyName("weather_state_name")]
    public string WeatherStateName { get; set; }

    [JsonPropertyName("applicable_date")]
    public string ApplicableDate { get; set; }

    [JsonPropertyName("min_temp")]
    public double MinTemp { get; set; }

    [JsonPropertyName("max_temp")]
    public double MaxTemp { get; set; }

}


