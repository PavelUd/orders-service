namespace Contracts.Commands;

public record CancelOrderCommand(Guid OrderId, string Reason);