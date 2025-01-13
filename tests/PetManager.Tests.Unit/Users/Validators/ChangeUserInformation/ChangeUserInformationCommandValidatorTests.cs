using PetManager.Application.Users.Commands.ChangeUserInformation;
using PetManager.Tests.Unit.Users.Factories;

namespace PetManager.Tests.Unit.Users.Validators.ChangeUserInformation;

public class ChangeUserInformationCommandValidatorTests
{
    [Fact]
    public void validate_change_user_information_command_with_valid_data_should_return_no_errors()
    {
        //arrange
        var command = _factory.CreateChangeUserInformationCommand();

        //act
        var result = _validator.Validate(command);

        //assert
        result.IsValid.ShouldBeTrue();
        result.Errors.ShouldBeEmpty();
    }

    [Fact]
    public void validate_change_user_information_command_with_empty_first_name_should_return_error()
    {
        //arrange
        var command = new ChangeUserInformationCommand("", "Test");

        //act
        var result = _validator.Validate(command);

        //assert
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldNotBeEmpty();
        result.Errors.ShouldContain(x => x.PropertyName == nameof(ChangeUserInformationCommand.FirstName));
    }

    [Fact]
    public void validate_change_user_information_command_with_null_first_name_should_return_error()
    {
        //arrange
        var command = new ChangeUserInformationCommand(null, "Test");

        //act
        var result = _validator.Validate(command);

        //assert
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldNotBeEmpty();
        result.Errors.ShouldContain(x => x.PropertyName == nameof(ChangeUserInformationCommand.FirstName));
    }

    [Fact]
    public void validate_change_user_information_command_with_too_long_first_name_should_return_error()
    {
        //arrange
        var command = new ChangeUserInformationCommand(new string('x', 51), "Test");

        //act
        var result = _validator.Validate(command);

        //assert
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldNotBeEmpty();
        result.Errors.ShouldContain(x => x.PropertyName == nameof(ChangeUserInformationCommand.FirstName));
    }

    [Fact]
    public void validate_change_user_information_command_with_empty_last_name_should_return_error()
    {
        //arrange
        var command = new ChangeUserInformationCommand("Test", "");

        //act
        var result = _validator.Validate(command);

        //assert
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldNotBeEmpty();
        result.Errors.ShouldContain(x => x.PropertyName == nameof(ChangeUserInformationCommand.LastName));
    }

    [Fact]
    public void validate_change_user_information_command_with_null_last_name_should_return_error()
    {
        //arrange
        var command = new ChangeUserInformationCommand("Test", null);

        //act
        var result = _validator.Validate(command);

        //assert
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldNotBeEmpty();
        result.Errors.ShouldContain(x => x.PropertyName == nameof(ChangeUserInformationCommand.LastName));
    }

    [Fact]
    public void validate_change_user_information_command_with_too_long_last_name_should_return_error()
    {
        //arrange
        var command = new ChangeUserInformationCommand("Test", new string('x', 51));

        //act
        var result = _validator.Validate(command);

        //assert
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldNotBeEmpty();
        result.Errors.ShouldContain(x => x.PropertyName == nameof(ChangeUserInformationCommand.LastName));
    }

    [Fact]
    public void
        validate_change_user_information_command_with_invalid_first_name_and_last_name_should_return_multiple_errors()
    {
        //arrange
        var command = new ChangeUserInformationCommand("", "");

        //act
        var result = _validator.Validate(command);

        //assert
        result.IsValid.ShouldBeFalse();
        result.Errors.Count.ShouldBe(2);
        result.Errors.ShouldContain(x => x.PropertyName == nameof(ChangeUserInformationCommand.FirstName));
        result.Errors.ShouldContain(x => x.PropertyName == nameof(ChangeUserInformationCommand.LastName));
    }

    private readonly IValidator<ChangeUserInformationCommand> _validator = new ChangeUserInformationCommandValidator();
    private readonly UserTestFactory _factory = new();
}