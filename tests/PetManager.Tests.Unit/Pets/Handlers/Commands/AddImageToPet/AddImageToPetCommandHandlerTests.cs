using NSubstitute.ExceptionExtensions;
using PetManager.Application.Common.Integrations.BlobStorage;
using PetManager.Application.Pets.Commands.AddImageToPet;
using PetManager.Core.Pets.Entities;
using PetManager.Core.Pets.Exceptions;
using PetManager.Core.Pets.Repositories;
using PetManager.Tests.Unit.Pets.Factories;

namespace PetManager.Tests.Unit.Pets.Handlers.Commands.AddImageToPet;

public sealed class AddImageToPetCommandHandlerTests
{
    private async Task<AddImageToPetResponse> Act(AddImageToPetCommand command)
        => await _handler.Handle(command, CancellationToken.None);

    [Fact]
    public async Task Handle_GivenInvalidPetId_ShouldThrowPetNotFoundException()
    {
        // Arrange
        var command = _petTestFactory.AddImageToPetCommand();
        _petRepository
            .GetAsync(Arg.Any<Expression<Func<Pet, bool>>>(), Arg.Any<CancellationToken>())
            .ReturnsNull();

        // Act
        var exception = await Record.ExceptionAsync(() => Act(command));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<PetNotFoundException>();
        await _petRepository
            .Received(1)
            .GetAsync(Arg.Any<Expression<Func<Pet, bool>>>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenPetHasNoImage_ShouldUploadImageAndSaveToRepository()
    {
        // Arrange
        var command = _petTestFactory.AddImageToPetCommand();
        var pet = _petTestFactory.CreatePet();
        var blobUrl = "https://test-storage.com/image.jpg";

        _petRepository
            .GetAsync(Arg.Any<Expression<Func<Pet, bool>>>(), Arg.Any<CancellationToken>())
            .Returns(pet);

        _blobStorageService
            .UploadImageAsync(command.File, Arg.Any<CancellationToken>())
            .Returns(blobUrl);

        // Act
        var result = await Act(command);

        // Assert
        result.ShouldNotBeNull();
        await _blobStorageService
            .Received(1)
            .UploadImageAsync(command.File, Arg.Any<CancellationToken>());
        await _imageRepository
            .Received(1)
            .AddAsync(Arg.Is<Image>(i =>
                    i.FileName == command.File.FileName &&
                    i.BlobUrl == blobUrl &&
                    i.PetId == pet.Id),
                Arg.Any<CancellationToken>());
        await _imageRepository
            .Received(1)
            .SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenPetHasExistingImage_ShouldDeleteOldImageAndUploadNew()
    {
        // Arrange
        var command = _petTestFactory.AddImageToPetCommand();
        var pet = _petTestFactory.CreatePet();
        var existingImage = Image.Create("old-image.jpg", "https://test-storage.com/old-image.jpg", pet.Id);
        pet.SetImage(existingImage);

        var newBlobUrl = "https://test-storage.com/new-image.jpg";

        _petRepository
            .GetAsync(Arg.Any<Expression<Func<Pet, bool>>>(), Arg.Any<CancellationToken>())
            .Returns(pet);

        _blobStorageService
            .UploadImageAsync(command.File, Arg.Any<CancellationToken>())
            .Returns(newBlobUrl);

        // Act
        var result = await Act(command);

        // Assert
        result.ShouldNotBeNull();
        await _blobStorageService
            .Received(1)
            .DeleteImageAsync(existingImage.BlobUrl, Arg.Any<CancellationToken>());
        await _imageRepository
            .Received(1)
            .DeleteAsync(existingImage, Arg.Any<CancellationToken>());
        await _blobStorageService
            .Received(1)
            .UploadImageAsync(command.File, Arg.Any<CancellationToken>());
        await _imageRepository
            .Received(2) // Once for delete, once for add
            .SaveChangesAsync(Arg.Any<CancellationToken>());
        await _imageRepository
            .Received(1)
            .AddAsync(Arg.Is<Image>(i =>
                    i.FileName == command.File.FileName &&
                    i.BlobUrl == newBlobUrl &&
                    i.PetId == pet.Id),
                Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenBlobStorageUploadFails_ShouldPropagateException()
    {
        // Arrange
        var command = _petTestFactory.AddImageToPetCommand();
        var pet = _petTestFactory.CreatePet();
        var expectedException = new Exception("Upload failed");

        _petRepository
            .GetAsync(Arg.Any<Expression<Func<Pet, bool>>>(), Arg.Any<CancellationToken>())
            .Returns(pet);

        _blobStorageService
            .UploadImageAsync(command.File, Arg.Any<CancellationToken>())
            .ThrowsAsync(expectedException);

        // Act
        var exception = await Record.ExceptionAsync(() => Act(command));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBe(expectedException);
        await _imageRepository
            .DidNotReceive()
            .AddAsync(Arg.Any<Image>(), Arg.Any<CancellationToken>());
        await _imageRepository
            .DidNotReceive()
            .SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    private readonly IBlobStorageService _blobStorageService;
    private readonly IPetRepository _petRepository;
    private readonly IImageRepository _imageRepository;
    private readonly IRequestHandler<AddImageToPetCommand, AddImageToPetResponse> _handler;
    private readonly PetTestFactory _petTestFactory = new();

    public AddImageToPetCommandHandlerTests()
    {
        _blobStorageService = Substitute.For<IBlobStorageService>();
        _petRepository = Substitute.For<IPetRepository>();
        _imageRepository = Substitute.For<IImageRepository>();

        _handler = new AddImageToPetCommandHandler(_blobStorageService, _petRepository, _imageRepository);
    }
}