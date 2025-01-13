using PetManager.Application.Users.Commands.ForgotPassword;
using PetManager.Tests.Unit.Users.Factories;

namespace PetManager.Tests.Unit.Users.Validators.ForgotPassword;

public class ForgotPasswordCommandValidatorTests
{
    [Fact]
    public void validate_forgot_password_command_with_valid_data_should_return_no_errors()
    {
        //arrange
        var command = _factory.CreateForgotPasswordCommand();

        //act
        var result = _validator.Validate(command);

        //assert
        result.IsValid.ShouldBeTrue();
        result.Errors.ShouldBeEmpty();
    }

    [Fact]
    public void validate_forgot_password_command_with_invalid_email_should_return_error()
    {
        //arrange
        var command = new ForgotPasswordCommand("test.petmanager");

        //act
        var result = _validator.Validate(command);

        //assert
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldNotBeEmpty();
        result.Errors.ShouldContain(x => x.PropertyName == nameof(ForgotPasswordCommand.Email));
    }

    [Fact]
    public void validate_forgot_password_command_with_null_email_should_return_error()
    {
        //arrange
        var command = new ForgotPasswordCommand(null);

        //act
        var result = _validator.Validate(command);

        //assert
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldNotBeEmpty();
        result.Errors.ShouldContain(x => x.PropertyName == nameof(ForgotPasswordCommand.Email));
    }

    [Fact]
    public void validate_forgot_password_command_with_empty_email_should_return_error()
    {
        //arrange
        var command = new ForgotPasswordCommand("");

        //act
        var result = _validator.Validate(command);

        //assert
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldNotBeEmpty();
        result.Errors.ShouldContain(x => x.PropertyName == nameof(ForgotPasswordCommand.Email));
    }

    private readonly IValidator<ForgotPasswordCommand> _validator = new ForgotPasswordCommandValidator();
    private readonly UserTestFactory _factory = new();
}