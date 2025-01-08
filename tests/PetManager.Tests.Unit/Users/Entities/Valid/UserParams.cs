using PetManager.Core.Users.Enums;

namespace PetManager.Tests.Unit.Users.Entities.Valid;

public sealed record UserParams(
    string Email,
    string Password,
    UserRole Role);