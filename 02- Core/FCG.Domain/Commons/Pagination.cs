namespace FCG.Domain.Commons;

public class Pagination<T>
{
    public Pagination(IEnumerable<T> items, int totalCount, int page, int pageSize)
    {
        Items = items;
        TotalCount = totalCount;
        Page = page;
        PageSize = pageSize;
    }

    public IEnumerable<T> Items { get; }

    public int TotalCount { get; }

    public int Page { get; }

    public int PageSize { get; }
}
