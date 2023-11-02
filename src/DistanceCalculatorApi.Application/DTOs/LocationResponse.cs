namespace DistanceCalculatorApi.Application.DTOs;

using Newtonsoft.Json;

public class LocationResponse
{
    public string Ip { get; set; } = null!;
    
    public string Type { get; set; } = null!;
    
    [JsonProperty("continent_code")]
    public string ContinentCode { get; set; } = null!;
    
    [JsonProperty("continent_name")]
    public string ContinentName { get; set; } = null!;

    [JsonProperty("country_code")]
    public string CountryCode { get; set; } = null!;
    
    [JsonProperty("country_name")]
    public string CountryName { get; set; } = null!;
}