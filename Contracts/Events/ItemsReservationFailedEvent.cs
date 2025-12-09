namespace Contracts.Events;

public record ItemsReservationFailedEvent(Guid OrderId, string Reason);