using Microsoft.EntityFrameworkCore;

namespace PetManager.Application.Shared.Pagination;

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

    public static async Task<PaginationResult<T>> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize,
        CancellationToken cancellationToken)
    {
        var count = await source.CountAsync(cancellationToken);
        var items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);

        return new PaginationResult<T>(items, count, pageIndex, pageSize);
    }
}