namespace PetManager.Application.Users.Commands.DeleteUser;

internal record DeleteUserCommand(Guid UserId) : IRequest;