
namespace PetManager.Application.Common.Pagination;

public class PaginationResult<T>
{
    public List<T> Items { get; }
    public int PageIndex { get; }
    public int TotalPages { get; }
    public int TotalCount { get; }


    public PaginationResult(List<T> items, int count, int pageIndex, int pageSize)
    {
        PageIndex = pageIndex;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        TotalCount = count;
        Items = items;
    }

    public bool HasPreviousPage => PageIndex > 1;

    public bool HasNextPage => PageIndex < TotalPages;

    public static Task<PaginationResult<T>> CreateAsync(IEnumerable<T> source, int pageIndex, int pageSize,
        CancellationToken cancellationToken)
    {
        var count = source.ToList().Count;
        var items =  source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

        return Task.FromResult(new PaginationResult<T>(items, count, pageIndex, pageSize));
    }
}