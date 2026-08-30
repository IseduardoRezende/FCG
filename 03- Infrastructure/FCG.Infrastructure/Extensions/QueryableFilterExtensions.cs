using FCG.Domain.Enums;
using FCG.Domain.Filters;
using System.Linq.Dynamic.Core;

namespace FCG.Infrastructure.Extensions;

public static class QueryableFilterExtensions
{
    private static readonly ParsingConfig ParsingConfig = new()
    {
        IsCaseSensitive = false
    };

    public static IQueryable<T> ApplyOrdering<T, TFilter>(this IQueryable<T> query, TFilter filter) where TFilter : BaseFilter
    {
        var direction = filter.OrderType == OrderTypes.Asc ? "asc" : "desc";
        return query.OrderBy(ParsingConfig, $"{filter.OrderField} {direction}");
    }

    public static IQueryable<T> ApplyPagination<T, TFilter>(this IQueryable<T> query, TFilter filter) where TFilter : BaseFilter
    {
        return query
            .Skip((filter.CurrentPage - 1) * filter.PageSize)
            .Take(filter.PageSize);
    }
}
