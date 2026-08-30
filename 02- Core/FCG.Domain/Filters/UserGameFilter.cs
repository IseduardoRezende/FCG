using System.Text.Json.Serialization;

namespace FCG.Domain.Filters;

public class UserGameFilter : BaseFilter
{
    [JsonPropertyName("userId")]
    public long? UserId { get; set; }

    [JsonPropertyName("gameId")]
    public long? GameId { get; set; }

    [JsonPropertyName("purchasedFrom")]
    public DateTime? PurchasedFrom { get; set; }

    [JsonPropertyName("purchasedTo")]
    public DateTime? PurchasedTo { get; set; }
}
