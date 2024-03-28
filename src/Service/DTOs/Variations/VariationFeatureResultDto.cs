using Service.DTOs.Categories;
using Service.DTOs.VariationOptions;

namespace Service.DTOs.Variations;

public class VariationFeatureResultDto
{
    public long Id { get; set; }
    public string Name { get; set; }
    public VariationOptionFeatureResult VariationOption { get; set; }
}
