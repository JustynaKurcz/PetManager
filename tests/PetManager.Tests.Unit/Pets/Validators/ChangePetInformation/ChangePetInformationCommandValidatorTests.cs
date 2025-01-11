using PetManager.Application.Pets.Commands.ChangePetInformation;
using PetManager.Core.Pets.Enums;
using PetManager.Tests.Unit.Pets.Factories;

namespace PetManager.Tests.Unit.Pets.Validators.ChangePetInformation;

public class ChangePetInformationCommandValidatorTests
{
    [Fact]
    public void validate_change_pet_information_command_with_valid_data_should_return_no_errors()
    {
        //arrange
        var command = _factory.ChangePetInformationCommand();

        //act
        var result = _validator.Validate(command);

        //assert
        result.IsValid.ShouldBeTrue();
        result.Errors.ShouldBeEmpty();
    }

    [Fact]
    public void validate_change_pet_information_command_with_invalid_species_should_return_error()
    {
        //arrange
        var command = new ChangePetInformationCommand((Species)999, "Labrador", Gender.Male)
        {
            PetId = Guid.NewGuid()
        };

        //act
        var result = _validator.Validate(command);

        //assert
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldNotBeEmpty();
        result.Errors.ShouldContain(x => x.PropertyName == nameof(ChangePetInformationCommand.Species));
    }

    [Fact]
    public void validate_change_pet_information_command_with_empty_breed_should_return_error()
    {
        //arrange
        var command = new ChangePetInformationCommand(Species.Dog, "", Gender.Male)
        {
            PetId = Guid.NewGuid()
        };

        //act
        var result = _validator.Validate(command);

        //assert
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldNotBeEmpty();
        result.Errors.ShouldContain(x => x.PropertyName == nameof(ChangePetInformationCommand.Breed));
    }

    [Fact]
    public void validate_change_pet_information_command_with_null_breed_should_return_error()
    {
        //arrange
        var command = new ChangePetInformationCommand(Species.Dog, null, Gender.Male)
        {
            PetId = Guid.NewGuid()
        };

        //act
        var result = _validator.Validate(command);

        //assert
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldNotBeEmpty();
        result.Errors.ShouldContain(x => x.PropertyName == nameof(ChangePetInformationCommand.Breed));
    }

    [Fact]
    public void validate_change_pet_information_command_with_too_long_breed_should_return_error()
    {
        //arrange
        var command = new ChangePetInformationCommand(Species.Dog, new string('x', 51), Gender.Male)
        {
            PetId = Guid.NewGuid()
        };

        //act
        var result = _validator.Validate(command);

        //assert
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldNotBeEmpty();
        result.Errors.ShouldContain(x => x.PropertyName == nameof(ChangePetInformationCommand.Breed));
    }

    [Fact]
    public void validate_change_pet_information_command_with_invalid_gender_should_return_error()
    {
        //arrange
        var command = new ChangePetInformationCommand(Species.Dog, "Labrador", (Gender)999)
        {
            PetId = Guid.NewGuid()
        };

        //act
        var result = _validator.Validate(command);

        //assert
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldNotBeEmpty();
        result.Errors.ShouldContain(x => x.PropertyName == nameof(ChangePetInformationCommand.Gender));
    }

    [Fact]
    public void validate_change_pet_information_command_with_multiple_invalid_fields_should_return_multiple_errors()
    {
        //arrange
        var command = new ChangePetInformationCommand((Species)999, "", (Gender)999)
        {
            PetId = Guid.Empty
        };

        //act
        var result = _validator.Validate(command);

        //assert
        result.IsValid.ShouldBeFalse();
        result.Errors.Count.ShouldBe(3); 
        result.Errors.ShouldContain(x => x.PropertyName == nameof(ChangePetInformationCommand.Species));
        result.Errors.ShouldContain(x => x.PropertyName == nameof(ChangePetInformationCommand.Breed));
        result.Errors.ShouldContain(x => x.PropertyName == nameof(ChangePetInformationCommand.Gender));
    }

    private readonly IValidator<ChangePetInformationCommand> _validator = new ChangePetInformationCommandValidator();
    private readonly PetTestFactory _factory = new();
}