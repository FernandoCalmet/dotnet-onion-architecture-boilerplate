using MediatR;
using MyCompany.MyProduct.Core.Primitives;

namespace MyCompany.MyProduct.Application.Abstractions.Messaging;

public interface IDomainEventHandler<TEvent> : INotificationHandler<TEvent>
    where TEvent : IDomainEvent
{
}