namespace Contracts.Events;

public record ItemsReservedEvent(Guid OrderId, decimal TotalPrice) : IEvent;