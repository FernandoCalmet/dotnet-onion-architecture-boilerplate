using MediatR;

namespace MyCompany.MyProduct.Application.Abstractions.Messaging;

public interface IEventHandler<in TEvent> : INotificationHandler<TEvent>
    where TEvent : INotification
{
}