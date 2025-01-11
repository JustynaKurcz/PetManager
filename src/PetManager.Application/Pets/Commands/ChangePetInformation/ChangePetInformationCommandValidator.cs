namespace PetManager.Application.Pets.Commands.ChangePetInformation;

internal sealed class ChangePetInformationCommandValidator : AbstractValidator<ChangePetInformationCommand>
{
    public ChangePetInformationCommandValidator()
    {
        RuleFor(x => x.Species)
            .IsInEnum().WithMessage("Invalid species value")
            .NotNull().WithMessage("Species cannot be null");

        RuleFor(x => x.Breed)
            .NotEmpty().WithMessage("Breed cannot be empty")
            .NotNull().WithMessage("Breed cannot be null")
            .MaximumLength(50).WithMessage("Breed must be at most 50 characters long");

        RuleFor(x => x.Gender)
            .IsInEnum().WithMessage("Invalid gender value")
            .NotNull().WithMessage("Gender cannot be null");
    }
}