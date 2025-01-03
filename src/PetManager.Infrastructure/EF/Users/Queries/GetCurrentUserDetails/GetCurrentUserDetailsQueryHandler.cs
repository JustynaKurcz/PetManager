using PetManager.Application.Shared.Context;
using PetManager.Application.Users.Queries.GetCurrentUserDetails;
using PetManager.Application.Users.Queries.GetCurrentUserDetails.DTO;
using PetManager.Core.Users.Exceptions;
using PetManager.Core.Users.Repositories;

namespace PetManager.Infrastructure.EF.Users.Queries.GetCurrentUserDetails;

internal sealed class GetCurrentUserDetailsQueryHandler(IUserRepository userRepository, IContext context)
    : IRequestHandler<GetCurrentUserDetailsQuery, CurrentUserDetailsDto>
{
    public async Task<CurrentUserDetailsDto> Handle(GetCurrentUserDetailsQuery query,
        CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(context.UserId, cancellationToken)
                   ?? throw new UserNotFoundException(context.UserId);

        return user.AsDetailsDto();
    }
}