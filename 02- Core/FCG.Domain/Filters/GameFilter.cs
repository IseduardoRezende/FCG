using System.Text.Json.Serialization;

namespace FCG.Domain.Filters;

public class GameFilter : BaseFilter
{
    [JsonPropertyName("minPrice")]
    public decimal? MinPrice { get; set; }

    [JsonPropertyName("maxPrice")]
    public decimal? MaxPrice { get; set; }
}
