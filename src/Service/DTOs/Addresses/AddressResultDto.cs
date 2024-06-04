using Service.DTOs.Countries;
using Service.DTOs.Districts;
using Service.DTOs.Regions;

namespace Service.DTOs.Addresses;

public class AddressResultDto
{
    public long Id { get; set; }
    public string Street { get; set; }
    public string Floor { get; set; }
    public string Home { get; set; }
    public string DoorCode { get; set; }
    public CountryResultDto Country { get; set; }
    public RegionResultDto Region { get; set; }
    public DistrictResultDto District { get; set; }
}