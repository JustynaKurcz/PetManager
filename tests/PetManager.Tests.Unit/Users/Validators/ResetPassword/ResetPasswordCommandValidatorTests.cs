using PetManager.Application.Users.Commands.ResetPassword;
using PetManager.Tests.Unit.Users.Factories;

namespace PetManager.Tests.Unit.Users.Validators.ResetPassword;

public class ResetPasswordCommandValidatorTests
{
   [Fact]
   public void validate_reset_password_command_with_valid_data_should_return_no_errors()
   {
       //arrange
       var command = _factory.CreateResetPasswordCommand();

       //act
       var result = _validator.Validate(command);

       //assert
       result.IsValid.ShouldBeTrue();
       result.Errors.ShouldBeEmpty();
   }

   [Fact]
   public void validate_reset_password_command_with_invalid_email_should_return_error()
   {
       //arrange
       var command = new ResetPasswordCommand("test.petmanager", "password123");

       //act
       var result = _validator.Validate(command);

       //assert
       result.IsValid.ShouldBeFalse();
       result.Errors.ShouldNotBeEmpty();
       result.Errors.ShouldContain(x => x.PropertyName == nameof(ResetPasswordCommand.Email));
   }

   [Fact]
   public void validate_reset_password_command_with_null_email_should_return_error()
   {
       //arrange
       var command = new ResetPasswordCommand(null, "password123");

       //act
       var result = _validator.Validate(command);

       //assert
       result.IsValid.ShouldBeFalse();
       result.Errors.ShouldNotBeEmpty();
       result.Errors.ShouldContain(x => x.PropertyName == nameof(ResetPasswordCommand.Email));
   }

   [Fact]
   public void validate_reset_password_command_with_empty_email_should_return_error()
   {
       //arrange
       var command = new ResetPasswordCommand("", "password123");

       //act
       var result = _validator.Validate(command);

       //assert
       result.IsValid.ShouldBeFalse();
       result.Errors.ShouldNotBeEmpty();
       result.Errors.ShouldContain(x => x.PropertyName == nameof(ResetPasswordCommand.Email));
   }

   [Fact]
   public void validate_reset_password_command_with_empty_password_should_return_error()
   {
       //arrange
       var command = new ResetPasswordCommand("test@petmanager.com", "");

       //act
       var result = _validator.Validate(command);

       //assert
       result.IsValid.ShouldBeFalse();
       result.Errors.ShouldNotBeEmpty();
       result.Errors.ShouldContain(x => x.PropertyName == nameof(ResetPasswordCommand.NewPassword));
   }

   [Fact]
   public void validate_reset_password_command_with_null_password_should_return_error()
   {
       //arrange
       var command = new ResetPasswordCommand("test@petmanager.com", null);

       //act
       var result = _validator.Validate(command);

       //assert
       result.IsValid.ShouldBeFalse();
       result.Errors.ShouldNotBeEmpty();
       result.Errors.ShouldContain(x => x.PropertyName == nameof(ResetPasswordCommand.NewPassword));
   }
   
   [Fact]
   public void validate_reset_password_command_with_too_short_password_should_return_error()
   {
       //arrange
       var command = new ResetPasswordCommand("test@petmanager.com", "short");

       //act
       var result = _validator.Validate(command);

       //assert
       result.IsValid.ShouldBeFalse();
       result.Errors.ShouldNotBeEmpty();
       result.Errors.ShouldContain(x => x.PropertyName == nameof(ResetPasswordCommand.NewPassword));
   }

   [Fact]
   public void validate_reset_password_command_with_too_long_password_should_return_error()
   {
       //arrange
       var command = new ResetPasswordCommand("test@petmanager.com", "thispasswordistoolongforreset");

       //act
       var result = _validator.Validate(command);

       //assert
       result.IsValid.ShouldBeFalse();
       result.Errors.ShouldNotBeEmpty();
       result.Errors.ShouldContain(x => x.PropertyName == nameof(ResetPasswordCommand.NewPassword));
   }
   
   [Fact]
   public void validate_reset_password_command_with_invalid_email_and_password_should_return_multiple_errors()
   {
       //arrange
       var command = new ResetPasswordCommand("invalid.email", "short");

       //act
       var result = _validator.Validate(command);

       //assert
       result.IsValid.ShouldBeFalse();
       result.Errors.Count.ShouldBe(2);
       result.Errors.ShouldContain(x => x.PropertyName == nameof(ResetPasswordCommand.Email));
       result.Errors.ShouldContain(x => x.PropertyName == nameof(ResetPasswordCommand.NewPassword));
   }
   
   private readonly IValidator<ResetPasswordCommand> _validator = new ResetPasswordCommandValidator();
   private readonly UserTestFactory _factory = new();
}