using System.Text.Json.Serialization;

namespace FCG.Domain.Filters;

public class UserFilter : BaseFilter
{
    [JsonPropertyName("userRoleId")]
    public long? UserRoleId { get; set; }
}
