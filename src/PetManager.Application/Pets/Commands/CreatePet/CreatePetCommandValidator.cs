namespace PetManager.Application.Pets.Commands.CreatePet;

internal sealed class CreatePetCommandValidator : AbstractValidator<CreatePetCommand>
{
    public CreatePetCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Pet name cannot be empty")
            .NotNull().WithMessage("Pet name cannot be null")
            .MaximumLength(50).WithMessage("Pet name must be at most 50 characters long");

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

        RuleFor(x => x.BirthDate)
            .NotNull().WithMessage("Birth date cannot be null")
            .Must(birthDate => birthDate <= DateTimeOffset.UtcNow)
            .WithMessage("Birth date cannot be in the future");
    }
}