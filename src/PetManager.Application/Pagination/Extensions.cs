namespace PetManager.Application.Pagination;

public static class Extensions
{
    public static Task<PaginationResult<TDestination>> PaginateAsync<TDestination>(
        this IQueryable<TDestination> queryable, PaginationRequest req, CancellationToken cancellationToken = default)
    {
        return PaginationResult<TDestination>.CreateAsync(queryable, req.PageNumber, req.PageSize, cancellationToken);
    }
}
