using Catalog.API.Models;
using Catalog.API.Products.GetProductById;

namespace Catalog.API.Products.GetProductByCategory
{
    public record GetProductByCategoryResponse(IEnumerable<Product> Products);
    public class GetProductByCategoryEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/products/category/{category}",async(string category,ISender sender) =>
            {
                var result = await sender.Send(new GetProductByCategoryQuery(category));
                var products = result.Adapt<GetProductByCategoryResponse>();
                return Results.Ok(products);
            })
             .WithName("GetProductByCategory")
            .Produces<GetProductByIdResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithDescription("Get Product by Category")
            .WithSummary("Get Product by Category");
        }
    }
}
