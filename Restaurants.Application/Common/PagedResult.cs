namespace Restaurants.Application.Common;

public class PagedResult<T>
{
    public PagedResult(IEnumerable<T> items, int totalCount, int pageSize, int pageNumber)
    {
        Items = items;
        TotalItemsCount = totalCount;
        TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
        ItemsFrom = pageSize * (pageNumber - 1) + 1;
        ItemsTo = ItemsFrom + pageSize - 1;
    }
    
    public IEnumerable<T> Items { get; }

    public int TotalItemsCount { get; }

    public int TotalPages { get; }

    public int ItemsFrom { get; }

    public int ItemsTo { get; }
}