namespace PetManager.Application.Common.Pagination;

public static class PaginationExtensions
{
    public static Task<PaginationResult<TDestination>> PaginateAsync<TDestination>(
        this IEnumerable<TDestination> enumerable, PaginationRequest req, CancellationToken cancellationToken = default)
    {
        return PaginationResult<TDestination>.CreateAsync(enumerable, req.PageNumber, req.PageSize, cancellationToken);
    }
}