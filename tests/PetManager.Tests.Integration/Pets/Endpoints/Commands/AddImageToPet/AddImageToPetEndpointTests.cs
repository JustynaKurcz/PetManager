using Microsoft.AspNetCore.Http;
using PetManager.Api.Endpoints.Pets;
using PetManager.Application.Pets.Commands.AddImageToPet;
using PetManager.Core.Pets.Entities;
using PetManager.Tests.Integration.Configuration;
using PetManager.Tests.Integration.Pets.Factories;
using PetManager.Tests.Integration.Pets.Helpers;
using PetManager.Tests.Integration.Users.Factories;

namespace PetManager.Tests.Integration.Pets.Endpoints.Commands.AddImageToPet;

public class AddImageToPetEndpointTests : IntegrationTestBase
{
    private readonly UserTestFactory _userFactory = new();
    private readonly PetTestFactory _petFactory = new();

    [Fact]
    public async Task post_add_image_to_pet_without_being_authorized_should_return_401_status_code()
    {
        // Arrange
        var command = _petFactory.AddImageToPetCommand();
        var content = CreateMultipartContent(command.File);

        // Act
        var response = await _client.PostAsync(
            PetEndpoints.AddImageToPet.Replace("{petId:guid}", command.PetId.ToString()),
            content);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task post_add_image_to_pet_given_non_existing_pet_should_return_400_status_code()
    {
        // Arrange
        var user = _userFactory.CreateUser();
        await AddAsync(user);
        await Authenticate(user.Id, user.Role.ToString());

        var command = _petFactory.AddImageToPetCommand();
        var content = CreateMultipartContent(command.File);

        // Act
        var response = await _client.PostAsync(
            PetEndpoints.AddImageToPet.Replace("{petId:guid}", command.PetId.ToString()),
            content);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task post_add_image_to_pet_given_valid_data_should_return_200_status_code()
    {
        // Arrange
        var user = _userFactory.CreateUser();
        await AddAsync(user);
        await Authenticate(user.Id, user.Role.ToString());

        var pet = _petFactory.CreatePet(user.Id);
        await AddAsync(pet);

        var command = _petFactory.AddImageToPetCommand();
        var content = CreateMultipartContent(command.File);

        // Act
        var response = await _client.PostAsync(
            PetEndpoints.AddImageToPet.Replace("{petId:guid}", pet.Id.ToString()),
            content);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<AddImageToPetResponse>();
        result.ShouldNotBeNull();
        result.ImageId.ShouldNotBe(Guid.Empty);
    }

    [Fact]
    public async Task
        post_add_image_to_pet_given_pet_with_existing_image_should_return_200_status_code_and_replace_image()
    {
        // Arrange
        var user = _userFactory.CreateUser();
        await AddAsync(user);
        await Authenticate(user.Id, user.Role.ToString());

        var pet = _petFactory.CreatePet(user.Id);
        await AddAsync(pet);

        var existingImage = Image.Create("old-image.jpg", "https://test-storage.com/old-image.jpg", pet.Id);
        await AddAsync(existingImage);

        pet.SetImage(existingImage);
        // await SaveChangesAsync();

        var command = new AddImageToPetCommand(pet.Id, FormFileGenerator.CreateTestFileFaker().Generate());
        var content = CreateMultipartContent(command.File);

        // Act
        var response = await _client.PostAsync(
            PetEndpoints.AddImageToPet.Replace("{petId:guid}", pet.Id.ToString()),
            content);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<AddImageToPetResponse>();
        result.ShouldNotBeNull();
        result.ImageId.ShouldNotBe(existingImage.Id);
    }

    private static MultipartFormDataContent CreateMultipartContent(IFormFile file)
    {
        var content = new MultipartFormDataContent();
        var fileContent = new StreamContent(file.OpenReadStream());
        content.Add(fileContent, "file", file.FileName);
        return content;
    }
}