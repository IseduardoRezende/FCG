using FCG.Domain.Filters;
using System.Linq.Dynamic.Core;

namespace FCG.Infrastructure.Extensions;

public static class QueryableFilterExtensions
{
    public static IQueryable<T> ApplyOrdering<T>(this IQueryable<T> query, BaseFilter filter)
    {
        return query.OrderBy($"{filter.OrderField} {filter.OrderType}");
    }

    public static IQueryable<T> ApplyPagination<T>(this IQueryable<T> query, BaseFilter filter)
    {
        return query
            .Skip((filter.CurrentPage - 1) * filter.PageSize)
            .Take(filter.PageSize);
    }
}
