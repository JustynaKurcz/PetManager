namespace PetManager.Application.HealthRecords.Commands.AddAppointmentToHealthRecord;

internal sealed class
    AddAppointmentToHealthRecordCommandValidator : AbstractValidator<AddAppointmentToHealthRecordCommand>
{
    public AddAppointmentToHealthRecordCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title cannot be empty")
            .NotNull().WithMessage("Title cannot be null")
            .MaximumLength(100).WithMessage("Title must be at most 100 characters long");

        RuleFor(x => x.Diagnosis)
            .MaximumLength(500).WithMessage("Diagnosis must be at most 500 characters long");

        RuleFor(x => x.AppointmentDate)
            .NotNull().WithMessage("Appointment date cannot be null");

        RuleFor(x => x.Notes)
            .MaximumLength(1000).WithMessage("Notes must be at most 1000 characters long");
    }
}