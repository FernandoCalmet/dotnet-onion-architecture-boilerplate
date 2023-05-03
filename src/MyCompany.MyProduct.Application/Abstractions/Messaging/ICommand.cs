using MediatR;

namespace MyCompany.MyProduct.Application.Abstractions.Messaging;

public interface ICommand<out TResponse> : IRequest<TResponse>
{
}