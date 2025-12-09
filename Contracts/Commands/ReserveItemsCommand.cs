using Contracts.Events;

namespace Contracts.Commands;

public record ReserveItemsCommand(Guid OrderId, IReadOnlyList<OrderItemDto> Items) : IEvent;