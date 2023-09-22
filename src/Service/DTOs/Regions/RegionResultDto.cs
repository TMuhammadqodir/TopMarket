﻿using Service.DTOs.Countries;

namespace Service.DTOs.Regions;

public class RegionResultDto
{
    public long Id { get; set; }
    public string NameUz { get; set; }
    public string NameOz { get; set; }
    public string NameRu { get; set; }
    public CountryResultDto Country { get; set; }
}
