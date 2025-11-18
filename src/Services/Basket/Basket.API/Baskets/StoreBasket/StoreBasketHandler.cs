

using Discount.Grpc;
using FluentValidation;

namespace Basket.API.Baskets.StoreBasket
{
    public record StoreBasketCommand(ShoppingCart Cart) : ICommand<StoreBasketResult>;

    public record StoreBasketResult(string UserName);

    public class StoreBasketValidator : AbstractValidator<StoreBasketCommand>
    {
        public StoreBasketValidator()
        {
            RuleFor(x => x.Cart).NotNull().WithMessage("Cart can not be null.");    
            RuleFor(x => x.Cart.UserName).NotEmpty().WithMessage("Username is required.");
        }
    }
    public class StoreBasketCommandHandler(IBasketRepository repository, DiscountProtoService.DiscountProtoServiceClient protoServiceClient)
        : ICommandHandler<StoreBasketCommand, StoreBasketResult>
    {
        public async Task<StoreBasketResult> Handle(StoreBasketCommand command, CancellationToken cancellationToken)
        {
            await DeductDiscount(command.Cart, cancellationToken);
            await repository.StoreBasket(command.Cart, cancellationToken);


            return new StoreBasketResult(command.Cart.UserName);
        }
        private async Task DeductDiscount(ShoppingCart cart, CancellationToken cancellation)
        {
            foreach (var item in cart.Items)
            {
                var coupon = await protoServiceClient.GetDiscountAsync(new GetDiscountRequest { ProductName = item.ProductName },cancellationToken:cancellation);
                item.Price -= coupon.Amount;
            }
        }
    }
    
}
