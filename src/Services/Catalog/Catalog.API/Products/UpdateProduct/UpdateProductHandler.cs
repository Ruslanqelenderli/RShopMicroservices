
using Catalog.API.Models;


namespace Catalog.API.Products.UpdateProduct
{
    public record UpdateProductCommand(Guid Id, string Name, List<string> Categories, string Description, string ImageFile, decimal Price) : ICommand<UpdateProductResult>;

    public record UpdateProductResult(bool IsSuccess);

    public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
    {
        public UpdateProductCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required.");
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required.");
            RuleFor(x => x.Categories).NotEmpty().WithMessage("Category is required.");
            RuleFor(x => x.ImageFile).NotEmpty().WithMessage("ImageFile is required.");
            RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price must be greater than 0.");
        }
    }
    internal class UpdateProductCommandHandler(IDocumentSession session)
        : ICommandHandler<UpdateProductCommand, UpdateProductResult>
    {
        public async Task<UpdateProductResult> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
           var product = await session.LoadAsync<Product>(request.Id, cancellationToken);

            if(product == null)
            {
                throw new ProductNotFoundException(request.Id);
            }

            product.Name = request.Name;
            product.Categories = request.Categories;
            product.Description = request.Description;
            product.ImageFile = request.ImageFile;
            product.Price = request.Price;

            session.Update(product);
            await session.SaveChangesAsync(cancellationToken);

            return new UpdateProductResult(true);
        }
    }
}
