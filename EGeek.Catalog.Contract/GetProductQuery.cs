using MediatR;

namespace EGeek.Catalog.Contract;

public record GetProductQuery(string Id) : IRequest<GetProductResponse>;