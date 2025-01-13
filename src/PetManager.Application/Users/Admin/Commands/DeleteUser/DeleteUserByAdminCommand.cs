namespace PetManager.Application.Users.Admin.Commands.DeleteUser;

internal record DeleteUserByAdminCommand(Guid UserId) : IRequest;