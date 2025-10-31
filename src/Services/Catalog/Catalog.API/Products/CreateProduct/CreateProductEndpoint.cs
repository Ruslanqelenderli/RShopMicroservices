namespace Catalog.API.Products.CreateProduct
{
    public record CreateProductRequest(string Name,string Description, List<string> Categories, string ImageUrl, decimal Price) : ICommand<CreateProductResult>;

    public record CreateProductResponse(Guid Id);
    public class CreateProductEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/products", async (CreateProductRequest request, ISender sender) =>
            {
                var command = request.Adapt<CreateProductCommand>();

                var response = await sender.Send(command);

                var result = response.Adapt<CreateProductResult>();

                return Results.Created($"/Products/{response}", response);
            })
            .WithName("CreateProduct")
            .Produces<CreateProductResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithDescription("Create Product")
            .WithSummary("Create Product");
        }
    }
}
