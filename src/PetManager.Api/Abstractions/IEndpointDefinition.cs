namespace PetManager.Abstractions;

public interface IEndpointDefinition
{
    void DefineEndpoint(IEndpointRouteBuilder app);
}