using App.Domain.Models.Request;

namespace App.Domain.Models.Response;



public class PagedResponse<T>
{
    public PagedResponse() // Parameterless constructor
    {
    }
    public PagedResponse(int pageIndex, int pageSize)
    {
        Items = new List<T>();
        PageIndex = pageIndex;
        PageSize = pageSize;
    }

    public PagedResponse(List<T> items, int totalCount, int pageIndex, int pageSize)
    {
        Items = items;
        PageIndex = pageIndex;
        PageSize = pageSize;
        TotalCount = totalCount;
    }

    public PagedResponse(List<T> items, int totalCount, int totalFilteredCount, DataTableRequest param)
    {
        Items = items;
        TotalCount = totalCount;
        TotalFilteredCount = totalFilteredCount;
        Param = param;
    }

    public DataTableRequest Param { get; set; }
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public int TotalFilteredCount { get; set; }

    public int TotalPages
    {
        get
        {
            return (int)Math.Ceiling(TotalCount / (double)PageSize);
        }
    }
    public bool HasPreviousPage => PageIndex > 1;
    public bool HasNextPage => PageIndex < TotalPages;
    public List<T> Items { get; set; }
}
//}
//public class PagedResponse<TEntity>
//{
//    public IReadOnlyList<TEntity> Items { get; init; } = [];

//    public int PageIndex { get; init; }

//    public int PageSize { get; init; }

//    public int TotalCount { get; init; }

//    /// <summary>
//    /// Optional filtered count for DataTables/server-side filtering
//    /// </summary>
//    public int? TotalFilteredCount { get; init; }

//    public int TotalPages =>
//        PageSize <= 0
//            ? 0
//            : (int)Math.Ceiling(TotalCount / (double)PageSize);

//    public bool HasPreviousPage => PageIndex > 1;

//    public bool HasNextPage => PageIndex < TotalPages;

//    public PagedResponse()
//    {
//    }

//    public PagedResponse(
//        IReadOnlyList<TEntity> items,
//        int totalCount,
//        int pageIndex,
//        int pageSize)
//    {
//        Items = items;
//        TotalCount = totalCount;
//        PageIndex = pageIndex;
//        PageSize = pageSize;
//    }

//    public PagedResponse(
//        IReadOnlyList<TEntity> items,
//        int totalCount,
//        int totalFilteredCount,
//        int pageIndex,
//        int pageSize)
//    {
//        Items = items;
//        TotalCount = totalCount;
//        TotalFilteredCount = totalFilteredCount;
//        PageIndex = pageIndex;
//        PageSize = pageSize;
//    }

//    public static PagedResponse<TEntity> Empty(
//        int pageIndex,
//        int pageSize)
//    {
//        return new PagedResponse<TEntity>(
//            [],
//            0,
//            pageIndex,
//            pageSize);
//    }
//}