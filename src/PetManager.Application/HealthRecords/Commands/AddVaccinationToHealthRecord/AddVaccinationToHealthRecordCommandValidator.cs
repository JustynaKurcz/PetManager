namespace PetManager.Application.HealthRecords.Commands.AddVaccinationToHealthRecord;

internal sealed class AddVaccinationToHealthRecordCommandValidator : AbstractValidator<AddVaccinationToHealthRecordCommand>
{
    public AddVaccinationToHealthRecordCommandValidator()
    {
        RuleFor(x => x.VaccinationName)
            .NotEmpty().WithMessage("Vaccination name cannot be empty")
            .NotNull().WithMessage("Vaccination name cannot be null")
            .MaximumLength(100).WithMessage("Vaccination name must be at most 100 characters long");

        RuleFor(x => x.VaccinationDate)
            .NotNull().WithMessage("Vaccination date cannot be null")
            .Must(vaccinationDate => vaccinationDate <= DateTimeOffset.UtcNow)
            .WithMessage("Vaccination date cannot be in the future");

        RuleFor(x => x.NextVaccinationDate)
            .NotNull().WithMessage("Next vaccination date cannot be null")
            .Must((command, nextDate) => nextDate > command.VaccinationDate)
            .WithMessage("Next vaccination date must be after vaccination date")
            .Must(nextDate => nextDate > DateTimeOffset.UtcNow)
            .WithMessage("Next vaccination date must be in the future");
    }
}