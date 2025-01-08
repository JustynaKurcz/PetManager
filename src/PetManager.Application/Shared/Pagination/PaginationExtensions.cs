namespace PetManager.Application.Shared.Pagination;

public static class PaginationExtensions
{
    public static Task<PaginationResult<TDestination>> PaginateAsync<TDestination>(
        this IQueryable<TDestination> queryable, PaginationRequest req, CancellationToken cancellationToken = default)
    {
        return PaginationResult<TDestination>.CreateAsync(queryable, req.PageNumber, req.PageSize, cancellationToken);
    }
}