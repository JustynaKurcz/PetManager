using PetManager.Application.HealthRecords.Commands.AddVaccinationToHealthRecord;
using PetManager.Tests.Unit.HealthRecords.Factories;

namespace PetManager.Tests.Unit.HealthRecords.Validators.AddVaccinationToHealthRecord;

public class AddVaccinationToHealthRecordCommandValidatorTests
{
    [Fact]
    public void validate_add_vaccination_to_health_record_command_with_valid_data_should_return_no_errors()
    {
        //arrange
        var command = _factory.AddVaccinationToHealthRecordCommand();

        //act
        var result = _validator.Validate(command);

        //assert
        result.IsValid.ShouldBeTrue();
        result.Errors.ShouldBeEmpty();
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void validate_add_vaccination_to_health_record_command_with_invalid_vaccination_name_should_return_error(
        string name)
    {
        //arrange
        var command = new AddVaccinationToHealthRecordCommand(
            name,
            DateTimeOffset.UtcNow.AddDays(-1),
            DateTimeOffset.UtcNow.AddYears(1))
        {
            HealthRecordId = Guid.NewGuid()
        };

        //act
        var result = _validator.Validate(command);

        //assert
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldNotBeEmpty();
        result.Errors.ShouldContain(x => x.PropertyName == nameof(AddVaccinationToHealthRecordCommand.VaccinationName));
    }

    [Fact]
    public void validate_add_vaccination_to_health_record_command_with_too_long_vaccination_name_should_return_error()
    {
        //arrange
        var command = new AddVaccinationToHealthRecordCommand(
            new string('x', 101),
            DateTimeOffset.UtcNow.AddDays(-1),
            DateTimeOffset.UtcNow.AddYears(1))
        {
            HealthRecordId = Guid.NewGuid()
        };

        //act
        var result = _validator.Validate(command);

        //assert
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldNotBeEmpty();
        result.Errors.ShouldContain(x => x.PropertyName == nameof(AddVaccinationToHealthRecordCommand.VaccinationName));
    }

    [Fact]
    public void
        validate_add_vaccination_to_health_record_command_with_multiple_invalid_fields_should_return_multiple_errors()
    {
        //arrange
        var command = new AddVaccinationToHealthRecordCommand(
            "",
            DateTimeOffset.UtcNow.AddDays(1),
            DateTimeOffset.UtcNow.AddDays(-1))
        {
            HealthRecordId = Guid.Empty
        };

        //act
        var result = _validator.Validate(command);

        //assert
        result.IsValid.ShouldBeFalse();
        result.Errors.Count.ShouldBe(1);
        result.Errors.ShouldContain(x => x.PropertyName == nameof(AddVaccinationToHealthRecordCommand.VaccinationName));
    }

    private readonly IValidator<AddVaccinationToHealthRecordCommand> _validator =
        new AddVaccinationToHealthRecordCommandValidator();

    private readonly HealthRecordTestFactory _factory = new();
}