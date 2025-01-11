using PetManager.Application.Pets.Commands.CreatePet;
using PetManager.Core.Pets.Enums;
using PetManager.Tests.Unit.Pets.Factories;

namespace PetManager.Tests.Unit.Pets.Validators.CreatePet;

public class CreatePetCommandValidatorTests
{
    [Fact]
    public void validate_create_pet_command_with_valid_data_should_return_no_errors()
    {
        //arrange
        var command = _factory.CreatePetCommand();

        //act
        var result = _validator.Validate(command);

        //assert
        result.IsValid.ShouldBeTrue();
        result.Errors.ShouldBeEmpty();
    }

    [Fact]
    public void validate_create_pet_command_with_empty_name_should_return_error()
    {
        //arrange
        var command = new CreatePetCommand("", Species.Dog, "Labrador", Gender.Male, DateTimeOffset.UtcNow.AddYears(-1));

        //act
        var result = _validator.Validate(command);

        //assert
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldNotBeEmpty();
        result.Errors.ShouldContain(x => x.PropertyName == nameof(CreatePetCommand.Name));
    }

    [Fact]
    public void validate_create_pet_command_with_null_name_should_return_error()
    {
        //arrange
        var command = new CreatePetCommand(null, Species.Dog, "Labrador", Gender.Male, DateTimeOffset.UtcNow.AddYears(-1));

        //act
        var result = _validator.Validate(command);

        //assert
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldNotBeEmpty();
        result.Errors.ShouldContain(x => x.PropertyName == nameof(CreatePetCommand.Name));
    }

    [Fact]
    public void validate_create_pet_command_with_too_long_name_should_return_error()
    {
        //arrange
        var command = new CreatePetCommand(
            new string('x', 51), 
            Species.Dog, 
            "Labrador", 
            Gender.Male, 
            DateTimeOffset.UtcNow.AddYears(-1));

        //act
        var result = _validator.Validate(command);

        //assert
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldNotBeEmpty();
        result.Errors.ShouldContain(x => x.PropertyName == nameof(CreatePetCommand.Name));
    }

    [Fact]
    public void validate_create_pet_command_with_invalid_species_should_return_error()
    {
        //arrange
        var command = new CreatePetCommand(
            "Max", 
            (Species)999, 
            "Labrador", 
            Gender.Male, 
            DateTimeOffset.UtcNow.AddYears(-1));

        //act
        var result = _validator.Validate(command);

        //assert
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldNotBeEmpty();
        result.Errors.ShouldContain(x => x.PropertyName == nameof(CreatePetCommand.Species));
    }

    [Fact]
    public void validate_create_pet_command_with_empty_breed_should_return_error()
    {
        //arrange
        var command = new CreatePetCommand("Max", Species.Dog, "", Gender.Male, DateTimeOffset.UtcNow.AddYears(-1));

        //act
        var result = _validator.Validate(command);

        //assert
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldNotBeEmpty();
        result.Errors.ShouldContain(x => x.PropertyName == nameof(CreatePetCommand.Breed));
    }

    [Fact]
    public void validate_create_pet_command_with_null_breed_should_return_error()
    {
        //arrange
        var command = new CreatePetCommand("Max", Species.Dog, null, Gender.Male, DateTimeOffset.UtcNow.AddYears(-1));

        //act
        var result = _validator.Validate(command);

        //assert
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldNotBeEmpty();
        result.Errors.ShouldContain(x => x.PropertyName == nameof(CreatePetCommand.Breed));
    }

    [Fact]
    public void validate_create_pet_command_with_too_long_breed_should_return_error()
    {
        //arrange
        var command = new CreatePetCommand(
            "Max", 
            Species.Dog, 
            new string('x', 51), 
            Gender.Male, 
            DateTimeOffset.UtcNow.AddYears(-1));

        //act
        var result = _validator.Validate(command);

        //assert
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldNotBeEmpty();
        result.Errors.ShouldContain(x => x.PropertyName == nameof(CreatePetCommand.Breed));
    }

    [Fact]
    public void validate_create_pet_command_with_invalid_gender_should_return_error()
    {
        //arrange
        var command = new CreatePetCommand(
            "Max", 
            Species.Dog, 
            "Labrador", 
            (Gender)999, 
            DateTimeOffset.UtcNow.AddYears(-1));

        //act
        var result = _validator.Validate(command);

        //assert
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldNotBeEmpty();
        result.Errors.ShouldContain(x => x.PropertyName == nameof(CreatePetCommand.Gender));
    }

    [Fact]
    public void validate_create_pet_command_with_future_birth_date_should_return_error()
    {
        //arrange
        var command = new CreatePetCommand(
            "Max", 
            Species.Dog, 
            "Labrador", 
            Gender.Male, 
            DateTimeOffset.UtcNow.AddYears(1));

        //act
        var result = _validator.Validate(command);

        //assert
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldNotBeEmpty();
        result.Errors.ShouldContain(x => x.PropertyName == nameof(CreatePetCommand.BirthDate));
    }

    [Fact]
    public void validate_create_pet_command_with_multiple_invalid_fields_should_return_multiple_errors()
    {
        //arrange
        var command = new CreatePetCommand("", Species.Dog, "", Gender.Male, DateTimeOffset.UtcNow.AddYears(1));

        //act
        var result = _validator.Validate(command);

        //assert
        result.IsValid.ShouldBeFalse();
        result.Errors.Count.ShouldBe(3);
        result.Errors.ShouldContain(x => x.PropertyName == nameof(CreatePetCommand.Name));
        result.Errors.ShouldContain(x => x.PropertyName == nameof(CreatePetCommand.Breed));
        result.Errors.ShouldContain(x => x.PropertyName == nameof(CreatePetCommand.BirthDate));
    }

    private readonly IValidator<CreatePetCommand> _validator = new CreatePetCommandValidator();
    private readonly PetTestFactory _factory = new();
}