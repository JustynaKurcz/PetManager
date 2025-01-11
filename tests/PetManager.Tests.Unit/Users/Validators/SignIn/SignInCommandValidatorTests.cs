using PetManager.Application.Users.Commands.SignIn;
using PetManager.Tests.Unit.Users.Factories;

namespace PetManager.Tests.Unit.Users.Validators.SignIn;

public class SignInCommandValidatorTests
{
    [Fact]
    public void validate_sign_in_command_with_valid_data_should_return_no_errors()
    {
        //arrange
        var command = _factory.CreateSignInCommand();

        //act
        var result = _validator.Validate(command);

        //assert
        result.IsValid.ShouldBeTrue();
        result.Errors.ShouldBeEmpty();
    }

    [Fact]
    public void validate_sign_in_command_with_invalid_email_should_return_error()
    {
        //arrange
        var command = new SignInCommand("test.petmanager", "password");

        //act
        var result = _validator.Validate(command);

        //assert
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldNotBeEmpty();
        result.Errors.ShouldContain(x => x.PropertyName == nameof(SignInCommand.Email));
    }

    [Fact]
    public void validate_sign_in_command_with_invalid_password_should_return_error()
    {
        //arrange
        var command = new SignInCommand("test@petmanager.com", "");

        //act
        var result = _validator.Validate(command);

        //assert
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldNotBeEmpty();
        result.Errors.ShouldContain(x => x.PropertyName == nameof(SignInCommand.Password));
    }
    
    [Fact]
    public void validate_sign_in_command_with_too_short_password_should_return_error()
    {
        //arrange
        var command = new SignInCommand("test@petmanager.com", "short");

        //act
        var result = _validator.Validate(command);

        //assert
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldNotBeEmpty();
        result.Errors.ShouldContain(x => x.PropertyName == nameof(SignInCommand.Password));
    }
    
    [Fact]
    public void validate_sign_in_command_with_invalid_email_and_password_should_return_multiple_errors()
    {
        //arrange
        var command = new SignInCommand("invalid.email", "short");

        //act
        var result = _validator.Validate(command);

        //assert
        result.IsValid.ShouldBeFalse();
        result.Errors.Count.ShouldBe(2);
        result.Errors.ShouldContain(x => x.PropertyName == nameof(SignInCommand.Email));
        result.Errors.ShouldContain(x => x.PropertyName == nameof(SignInCommand.Password));
    }
    
    private readonly IValidator<SignInCommand> _validator = new SignInCommandValidator();
    private readonly UserTestFactory _factory = new();
}