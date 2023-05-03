namespace MyCompany.MyProduct.Application.Abstractions.Messaging;

public interface IIntegrationEventPublisher
{
    void Publish(IIntegrationEvent integrationEvent);
}