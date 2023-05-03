using MediatR;

namespace MyCompany.MyProduct.Application.Abstractions.Messaging;

public interface IQuery<out TResponse> : IRequest<TResponse>
{
}