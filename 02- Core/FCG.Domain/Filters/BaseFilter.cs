using FCG.Domain.Enums;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace FCG.Domain.Filters;

public class BaseFilter
{
    const int MaxPageSize = 100;

    public BaseFilter()
    {
        PageSize = 15;
        CurrentPage = 1;
        OrderField = "id";
        OrderType = OrderTypes.Desc;
    }

    [JsonPropertyName("value")]
    public string? Value { get; set; }

    [JsonPropertyName("currentPage")]
    [DefaultValue(1)]
    public int CurrentPage
    {
        get;
        set
        {
            field = value < 1 ? 1 : value;
        }
    }

    [DefaultValue(15)]
    [JsonPropertyName("pageSize")]
    public int PageSize 
    { 
        get; 
        set
        {
            field = value > MaxPageSize ? MaxPageSize : value;
        }
    }

    [JsonPropertyName("orderField")]
    [DefaultValue("id")]
    public string OrderField { get; set; }   

    [JsonPropertyName("orderType")]

    [DefaultValue(OrderTypes.Desc)]
    public OrderTypes OrderType { get; set; }
}
