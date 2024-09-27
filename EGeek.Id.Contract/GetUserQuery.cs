using MediatR;

namespace EGeek.Id.Contract;

public record GetUserQuery(string Email) : IRequest<GetUserResponse>;