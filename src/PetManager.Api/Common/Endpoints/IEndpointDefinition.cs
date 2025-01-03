namespace PetManager.Api.Common.Endpoints;

public interface IEndpointDefinition
{
    void DefineEndpoint(IEndpointRouteBuilder app);
}