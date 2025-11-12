
namespace Basket.API.Baskets.GetBasket
{
    public record  GetBasketResponse(ShoppingCart ShoppingCart);
    public class GetBasketEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/basket/{username}", async (string username, ISender sender) =>
            {
                var result = await sender.Send(new GetBasketQuery(username));

                var response = result.Adapt<GetBasketResponse>();
                 
                return Results.Ok(response);

            })
            .WithName("GetBaskets")
            .Produces<GetBasketResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithDescription("Get Baskets")
            .WithSummary("Get Baskets");
        }
    }
}
