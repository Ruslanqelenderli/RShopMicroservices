using Catalog.API.Models;
using Marten;
using Marten.Pagination;

namespace Catalog.API.Products.GetProducts
{


    public record GetProductQuery(int? PageNumber = 1, int? PageSize = 10) : IQuery<GetProductResult>;

    public record GetProductResult(IEnumerable<Product> Products);
    internal class GetProductsQueryHandler(IDocumentSession session) : IQueryHandler<GetProductQuery, GetProductResult>
    {
        public async Task<GetProductResult> Handle(GetProductQuery request, CancellationToken cancellationToken)
        {

            var products = await session.Query<Product>().ToPagedListAsync(request.PageNumber ?? 1,request.PageSize ?? 10,cancellationToken);

            return new GetProductResult(products);
        }
    }
}
