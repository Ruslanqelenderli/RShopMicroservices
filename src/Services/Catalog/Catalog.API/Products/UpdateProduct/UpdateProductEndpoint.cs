
namespace Catalog.API.Products.UpdateProduct
{
    public record UpdateProductRequest(Guid Id, string Name, List<string> Categories, string Description, string ImageFile, decimal Price);

    public record UpdateProductResponse(bool IsSuccess);
    public class UpdateProductEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPut("/products",async (UpdateProductRequest request,ISender sender) =>
            {
                var product = request.Adapt<UpdateProductCommand>();

                var result = await sender.Send(product);

                var response = result.Adapt<UpdateProductResponse>();

                return Results.Ok(response);
            })
            .WithName("UpdateProduct")
            .Produces<UpdateProductResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithDescription("Update Product")
            .WithSummary("Update Product");
        }
    }
}
