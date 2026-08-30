using FCG.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FCG.Domain.Filters;

public class BaseFilter
{
    const int MaxPageSize = 100;

    public BaseFilter()
    {
        PageSize = 10;
        CurrentPage = 1;
        OrderField = "Id";
        OrderType = OrderTypes.Desc;
    }

    [JsonPropertyName("value")]
    public string? Value { get; set; }

    [JsonPropertyName("currentPage")]
    public int CurrentPage { get; set; }

    [JsonPropertyName("pageSize"), MaxLength(MaxPageSize)]
    public int PageSize { get; set; }

    [JsonPropertyName("orderField")]
    public string OrderField { get; set; }

    [JsonPropertyName("orderType")]
    public OrderTypes OrderType { get; set; } 
}
