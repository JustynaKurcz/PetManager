namespace PetManager.Application.HealthRecords.Commands.AddVaccinationToHealthRecord;

internal sealed class
    AddVaccinationToHealthRecordCommandValidator : AbstractValidator<AddVaccinationToHealthRecordCommand>
{
    public AddVaccinationToHealthRecordCommandValidator()
    {
        RuleFor(x => x.VaccinationName)
            .NotEmpty().WithMessage("Vaccination name cannot be empty")
            .NotNull().WithMessage("Vaccination name cannot be null")
            .MaximumLength(100).WithMessage("Vaccination name must be at most 100 characters long");

        RuleFor(x => x.VaccinationDate)
            .NotNull().WithMessage("Vaccination date cannot be null");
    }
}