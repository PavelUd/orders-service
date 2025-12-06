namespace Contracts.Events;


public record OrderCreatedEvent(
    Guid OrderId,
    Guid AccountId,
    IReadOnlyList<OrderItemDto> Items
) : IEvent;


public record OrderItemDto(
    Guid ProductId,
    int Quantity
);