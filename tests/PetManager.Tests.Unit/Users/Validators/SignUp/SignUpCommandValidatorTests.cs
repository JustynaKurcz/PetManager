using PetManager.Application.Users.Commands.SignUp;
using PetManager.Tests.Unit.Users.Factories;

namespace PetManager.Tests.Unit.Users.Validators.SignUp;

public class SignUpCommandValidatorTests
{
    [Fact]
    public void validate_sign_up_command_with_valid_data_should_return_no_errors()
    {
        //arrange
        var command = _factory.CreateSignUpCommand();

        //act
        var result = _validator.Validate(command);

        //assert
        result.IsValid.ShouldBeTrue();
        result.Errors.ShouldBeEmpty();
    }

    [Fact]
    public void validate_sign_up_command_with_invalid_email_should_return_error()
    {
        //arrange
        var command = new SignUpCommand("test.petmanager", "password123");

        //act
        var result = _validator.Validate(command);

        //assert
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldNotBeEmpty();
        result.Errors.ShouldContain(x => x.PropertyName == nameof(SignUpCommand.Email));
    }

    [Fact]
    public void validate_sign_up_command_with_empty_password_should_return_error()
    {
        //arrange
        var command = new SignUpCommand("test@petmanager.com", "");

        //act
        var result = _validator.Validate(command);

        //assert
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldNotBeEmpty();
        result.Errors.ShouldContain(x => x.PropertyName == nameof(SignUpCommand.Password));
    }

    [Fact]
    public void validate_sign_up_command_with_too_short_password_should_return_error()
    {
        //arrange
        var command = new SignUpCommand("test@petmanager.com", "short");

        //act
        var result = _validator.Validate(command);

        //assert
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldNotBeEmpty();
        result.Errors.ShouldContain(x => x.PropertyName == nameof(SignUpCommand.Password));
    }

    [Fact]
    public void validate_sign_up_command_with_too_long_password_should_return_error()
    {
        //arrange
        var command = new SignUpCommand("test@petmanager.com", "thispasswordistoolongforsignup");

        //act
        var result = _validator.Validate(command);

        //assert
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldNotBeEmpty();
        result.Errors.ShouldContain(x => x.PropertyName == nameof(SignUpCommand.Password));
    }

    [Fact]
    public void validate_sign_up_command_with_invalid_email_and_password_should_return_multiple_errors()
    {
        //arrange
        var command = new SignUpCommand("invalid.email", "short");

        //act
        var result = _validator.Validate(command);

        //assert
        result.IsValid.ShouldBeFalse();
        result.Errors.Count.ShouldBe(2);
        result.Errors.ShouldContain(x => x.PropertyName == nameof(SignUpCommand.Email));
        result.Errors.ShouldContain(x => x.PropertyName == nameof(SignUpCommand.Password));
    }

    private readonly IValidator<SignUpCommand> _validator = new SignUpCommandValidator();
    private readonly UserTestFactory _factory = new();
}