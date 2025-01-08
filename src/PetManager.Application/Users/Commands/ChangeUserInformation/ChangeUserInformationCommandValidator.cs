namespace PetManager.Application.Users.Commands.ChangeUserInformation;

internal class ChangeUserInformationCommandValidator : AbstractValidator<ChangeUserInformationCommand>
{
    public ChangeUserInformationCommandValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name cannot be empty")
            .NotNull().WithMessage("First name cannot be null")
            .MaximumLength(50).WithMessage("First name must be at most 50 characters long");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name cannot be empty")
            .NotNull().WithMessage("Last name cannot be null")
            .MaximumLength(50).WithMessage("Last name must be at most 50 characters long");
    }
}