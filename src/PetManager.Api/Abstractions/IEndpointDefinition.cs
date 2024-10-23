namespace PetManager.Api.Abstractions;

public interface IEndpointDefinition
{
    void DefineEndpoint(IEndpointRouteBuilder app);
}