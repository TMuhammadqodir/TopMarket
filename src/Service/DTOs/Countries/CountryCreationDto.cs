using Newtonsoft.Json;

namespace Service.DTOs.Countries;

public class CountryCreationDto
{
    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("code")]
    public string CountryCode { get; set; }
}
