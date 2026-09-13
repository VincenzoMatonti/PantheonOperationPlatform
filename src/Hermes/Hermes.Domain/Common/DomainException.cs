namespace Hermes.Domain.Common;

public abstract class DomainException<TCode>(TCode code, string message) : Exception(message) where TCode : Enum
{
    public TCode Code { get; } = code;
}