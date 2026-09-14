namespace Hermes.Domain.Operations.ValueObjects;

public enum OperationStatus
{
    Initialized = 1,
    Sent = 2,
    Validated = 3,
    Accepted = 4,
    Rejected = 5
}