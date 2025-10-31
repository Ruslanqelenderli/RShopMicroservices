using Catalog.API.Models;
using Catalog.API.Products.GetProducts;

namespace Catalog.API.Products.GetProductById
{
    public record GetProductByIdResponse(Product product);
    public class GetProductByIdEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/products/{id}",async (Guid id,ISender sender) =>
            {
                var result = await sender.Send(new GetProductByIdQuery(id));

                var product =  result.Adapt<GetProductByIdResponse>();

                return Results.Ok(product);
            })
             .WithName("GetProductById")
            .Produces<GetProductByIdResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithDescription("Get Product by id")
            .WithSummary("Get Product by id");
        }
    }
}
