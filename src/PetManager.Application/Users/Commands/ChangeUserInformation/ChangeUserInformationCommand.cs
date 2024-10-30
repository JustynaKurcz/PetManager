namespace PetManager.Application.Users.Commands.ChangeUserInformation;

internal record ChangeUserInformationCommand(
    string FirstName,
    string LastName
) : IRequest;