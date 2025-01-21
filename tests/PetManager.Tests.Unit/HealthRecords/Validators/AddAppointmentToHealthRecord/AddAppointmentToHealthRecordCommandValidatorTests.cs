using PetManager.Application.HealthRecords.Commands.AddAppointmentToHealthRecord;
using PetManager.Tests.Unit.HealthRecords.Factories;

namespace PetManager.Tests.Unit.HealthRecords.Validators.AddAppointmentToHealthRecord;

public class AddAppointmentToHealthRecordCommandValidatorTests
{
    [Fact]
    public void validate_add_appointment_to_health_record_command_with_valid_data_should_return_no_errors()
    {
        //arrange
        var command = _factory.AddAppointmentToHealthRecordCommand();

        //act
        var result = _validator.Validate(command);

        //assert
        result.IsValid.ShouldBeTrue();
        result.Errors.ShouldBeEmpty();
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void validate_add_appointment_to_health_record_command_with_invalid_title_should_return_error(string title)
    {
        //arrange
        var command = new AddAppointmentToHealthRecordCommand(
            title,
            "Healthy",
            DateTimeOffset.UtcNow.AddDays(-1),
            "Everything looks good")
        {
            HealthRecordId = Guid.NewGuid()
        };

        //act
        var result = _validator.Validate(command);

        //assert
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldNotBeEmpty();
        result.Errors.ShouldContain(x => x.PropertyName == nameof(AddAppointmentToHealthRecordCommand.Title));
    }

    [Fact]
    public void validate_add_appointment_to_health_record_command_with_too_long_title_should_return_error()
    {
        //arrange
        var command = new AddAppointmentToHealthRecordCommand(
            new string('x', 101),
            "Healthy",
            DateTimeOffset.UtcNow.AddDays(-1),
            "Everything looks good")
        {
            HealthRecordId = Guid.NewGuid()
        };

        //act
        var result = _validator.Validate(command);

        //assert
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldNotBeEmpty();
        result.Errors.ShouldContain(x => x.PropertyName == nameof(AddAppointmentToHealthRecordCommand.Title));
    }

    [Fact]
    public void validate_add_appointment_to_health_record_command_with_too_long_diagnosis_should_return_error()
    {
        //arrange
        var command = new AddAppointmentToHealthRecordCommand(
            "Regular Checkup",
            new string('x', 501),
            DateTimeOffset.UtcNow.AddDays(-1),
            "Everything looks good")
        {
            HealthRecordId = Guid.NewGuid()
        };

        //act
        var result = _validator.Validate(command);

        //assert
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldNotBeEmpty();
        result.Errors.ShouldContain(x => x.PropertyName == nameof(AddAppointmentToHealthRecordCommand.Diagnosis));
    }
    [Fact]
    public void validate_add_appointment_to_health_record_command_with_too_long_notes_should_return_error()
    {
        //arrange
        var command = new AddAppointmentToHealthRecordCommand(
            "Regular Checkup",
            "Healthy",
            DateTimeOffset.UtcNow.AddDays(-1),
            new string('x', 1001))
        {
            HealthRecordId = Guid.NewGuid()
        };

        //act
        var result = _validator.Validate(command);

        //assert
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldNotBeEmpty();
        result.Errors.ShouldContain(x => x.PropertyName == nameof(AddAppointmentToHealthRecordCommand.Notes));
    }

    private readonly IValidator<AddAppointmentToHealthRecordCommand> _validator =
        new AddAppointmentToHealthRecordCommandValidator();

    private readonly HealthRecordTestFactory _factory = new();
}